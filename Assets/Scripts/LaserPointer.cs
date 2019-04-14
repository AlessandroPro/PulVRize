using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Valve.VR;

public class LaserPointer : MonoBehaviour
{

    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean shootAction;
    public SteamVR_Action_Vibration hapticAction;

    public GameObject title;
    public LineRenderer laser;
    private Vector3 hitPoint;

    public GameObject segmentPrefab;
    public GameObject handPrefab;
    public GameObject projectilePrefab;
    public GameObject grabLightPrefab;
    private int numPoints = 100;

    private Vector3 xVector;
    private Vector3 yVector;
    private GameObject[] segments;
    private GameObject hand;
    private GameObject grabLight;
    public GameObject soulRing;
    private ParticleSystem soulRingParticles;
    private Vector3 connectToPoint;
    private Vector3 connectFromPoint;
    private Vector3 columnHitOffset;

    public Player player;
    public LayerMask columnMask;
    public ColumnBehaviour selectedColumn;
    public ColumnBehaviour grabbedColumn;

    public float handTime;

    public LayerMask soulMask;
    private int numSouls;
    public Text score;

    private AudioSource audioSource;
    public AudioClip[] clipArray;

    // Use this for initialization
    void Start()
    {
        laser.enabled = true;
        hitPoint = transform.forward * 100;
        handTime = 0;

        connectToPoint = transform.position;

        segments = new GameObject[numPoints];
        for (int i = 0; i < segments.Length; i++)
        {
            segments[i] = Instantiate(segmentPrefab);
        }

        hand = Instantiate(handPrefab);
        grabLight = Instantiate(grabLightPrefab);
        grabLight.SetActive(false);
        numSouls = 5;

        soulRingParticles = soulRing.GetComponent<ParticleSystem>();

        audioSource = GetComponent<AudioSource>();

        var emission = soulRingParticles.emission;
        emission.rateOverTime = numSouls;
    }

    // Update is called once per frame
    void Update()
    {
        soulRing.transform.RotateAround(transform.position, transform.forward, 5);

        if (handType == SteamVR_Input_Sources.LeftHand)
        {
            score.text = player.GetNumDefeatedString();
        }
        else
        {
            score.text = player.GetTimeString();
        }


        RaycastHit hit;

        if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, Mathf.Infinity, columnMask))
        {
            hitPoint = hit.point;
            GameObject hitColumn = hit.collider.gameObject;

            // There is already a selectedColumn
            if (selectedColumn != null)
            {
                // If the selectedColumn and hitColumn are different, set the selectedColumn to hitColumn
                if (hitColumn.GetInstanceID() != selectedColumn.gameObject.GetInstanceID())
                {
                    DeselectColumn();
                    SelectColumn(hitColumn);
                }
            }
            // If there is no selectedColumn, set the hitColumn to selectedColumn
            else
            {
                SelectColumn(hitColumn);
            }
        }
        // If there is no hitColumn, set selectedColumn to null
        else
        {
            hitPoint = transform.forward * 100;
            if (selectedColumn != null)
            {
                DeselectColumn();
            }
        }

        if (grabAction.GetStateDown(handType))
        {
            if (selectedColumn)
            {
                LaunchHand();
            }
            else
            {
                RetractHand(false);
            }
        }

        if (shootAction.GetStateDown(handType))
        {
            Shoot();
            if(title)
            {
                title.SetActive(false);
            }
        }

        UpdateConnectionPoint();
        UpdateLaser();
        UpdateConnectionLine();
        handTime += Time.deltaTime * 5f;
        grabLight.transform.position = hand.transform.position;


    }

    private void PlaySound(int clipIndex, float delay)
    {
        audioSource.clip = clipArray[clipIndex];
        audioSource.pitch = (Random.Range(0.4f, 0.8f));
        audioSource.volume = (Random.Range(0.2f, .4f));
        audioSource.PlayDelayed(delay);
    }

    private void LaunchHand()
    {
        RetractHand(true);
        grabbedColumn = selectedColumn;
        columnHitOffset = hitPoint - grabbedColumn.transform.position;
        connectFromPoint = controllerPose.transform.position;
        handTime = 0;
        //PlaySound(0, 0);
    }

    private void RetractHand(bool instant)
    {
        grabbedColumn = null;
        handTime = 0;

        if(instant)
        {
            connectFromPoint = controllerPose.transform.position;
        }
        else
        {
            connectFromPoint = hand.transform.position; 
        }
    }

    private void UpdateConnectionPoint()
    {
        if (grabbedColumn)
        {
            connectToPoint = grabbedColumn.transform.position + columnHitOffset;
        }
        else
        {
            connectToPoint = controllerPose.transform.position;
        }
    }

    private void UpdateLaser()
    {
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, hitPoint);
    }

    private void SelectColumn(GameObject column)
    {
        selectedColumn = column.GetComponent<ColumnBehaviour>();
        //selectedColumn.selected = true;
        selectedColumn.Highlight();
    }

    private void DeselectColumn()
    {
        //selectedColumn.selected = false;
        if (selectedColumn)
        {
            selectedColumn.RemoveHighlight();
        }
        selectedColumn = null;
    }

    private void UpdateConnectionLine()
    {
        /*
        xVector = hand.transform.position - transform.position;

        float distance = xVector.magnitude;
        Vector3 hypVector = transform.forward;
        float angle = Vector3.Angle(xVector, hypVector);
        if (angle > 25f)
        {
            hypVector = Vector3.RotateTowards(transform.forward, xVector, (angle * Mathf.Deg2Rad) - (25f * Mathf.Deg2Rad), 0.0f);
            hypVector.Normalize();
            angle = Vector3.Angle(xVector, hypVector);
        }
        float hyp = distance / Mathf.Cos(angle * Mathf.Deg2Rad);


        yVector = (hypVector * hyp) - xVector;

        xVector.Normalize();
        yVector.Normalize();

        float Vo = Mathf.Sqrt((distance * 9.8f) / Mathf.Sin(2 * angle * Mathf.Deg2Rad));
        float Vx = Vo * Mathf.Cos(angle * Mathf.Deg2Rad);
        float Vy = Vo * Mathf.Sin(angle * Mathf.Deg2Rad);


        float timeTotal = 2 * Vo * Mathf.Sin(angle * Mathf.Deg2Rad) / 9.8f;
        float interval = timeTotal / segments.Length;
        for (int i = 0; i < segments.Length; i++)
        {
            float time = interval * i;

            Vector3 xPos = xVector * Vx * time;
            Vector3 yPos = yVector * (Vy * time - 0.5f * 9.8f * time * time);

            segments[i].transform.position = transform.position + xPos + yPos;
        }
        */
        ////
        /*
        if (grabAction.GetStateDown(handType))
        {
            hand.transform.position = transform.position;
            handlaunchPoint = transform.position;
            handForward = (connectedPoint - transform.position).normalized;
        }

        if ((connectedPoint - handlaunchPoint).magnitude <= (hand.transform.position - handlaunchPoint).magnitude)
        {
            hand.transform.position = connectedPoint;
        }
        else
        {
            hand.transform.position += handForward * Time.deltaTime * 25f;
        }
        */

        hand.transform.position = Vector3.Lerp(connectFromPoint, connectToPoint, handTime);

        ///// Bezier curve /////

        Vector3 cp1 = transform.position;
        Vector3 cp3 = hand.transform.position;

        xVector = cp3 - cp1;
        Vector3 hypVector = transform.forward;
        float angle = Vector3.Angle(xVector, hypVector);

        if (angle >= 45f)
        {
            angle = 45f;
            hypVector = Vector3.RotateTowards(xVector, transform.forward, Mathf.Deg2Rad * angle, 0);
            hypVector.Normalize();
        }

        float x = xVector.magnitude / 2f;
        float z = x / Mathf.Cos(Mathf.Deg2Rad * angle);

        Vector3 cp2 = cp1 + (z * hypVector);

        float interval = 1f / segments.Length;
        float t = 0;

        Vector3 b12, b23;
        for (int i = 0; i < segments.Length; i++)
        {
            b12 = Vector3.Lerp(cp1, cp2, t);
            b23 = Vector3.Lerp(cp2, cp3, t);

            segments[i].transform.position = Vector3.Lerp(b12, b23, t);
            t += interval;
        }



        

        //////



        //// Move column based on controller velocity
        grabLight.SetActive(false);
        if (grabbedColumn)
   
        //if (grabbedColumn && angle > 20f)
            {
            //float maxVelocity = 2;
            //float maxAngVelocity = 7;

            float maxVelocity = 1;
            float maxAngVelocity = 4;

            if ((grabbedColumn.isUpColumn && (controllerPose.GetVelocity().y > maxVelocity || controllerPose.GetAngularVelocity().x < -maxAngVelocity)) ||
               (!grabbedColumn.isUpColumn && (controllerPose.GetVelocity().y < -maxVelocity || controllerPose.GetAngularVelocity().x > maxAngVelocity)))
            {
                grabbedColumn.Pulverize();
                grabbedColumn.movedBySource = transform;
                RetractHand(false);
                Pulse(0.1f, 50, 75);
            }

            if ((hand.transform.position - connectToPoint).magnitude < 0.01 && !grabLight.activeSelf)
            {
                grabLight.SetActive(true);
            }
        }
    }

    private void Shoot()
    {
        if (numSouls > 0)
        {
            Instantiate(projectilePrefab, transform.position, transform.rotation);
            numSouls--;
        }
        var emission = soulRingParticles.emission;
        emission.rateOverTime = numSouls;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & soulMask) != 0)
        {
            CollectSoul(other.gameObject);
        }
    }

    private void CollectSoul(GameObject soul)
    {
        var ps = soul.GetComponent<ParticleSystem>();
        ps.Stop();
        var main = ps.main;
        main.stopAction = ParticleSystemStopAction.Destroy;
        numSouls++;
        player.increaseNumDefeated(1);

        var emission = soulRingParticles.emission;
        emission.rateOverTime = numSouls;
    }

    public void Pulse(float duration, float frequency, float amplitude)
    {
        hapticAction.Execute(0, duration, frequency, amplitude, handType);
    }
}
