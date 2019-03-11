using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class LaserPointer : MonoBehaviour
{

    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean shootAction;

    public LineRenderer laser;
    private Vector3 hitPoint;

    public GameObject segmentPrefab;
    public GameObject handPrefab;
    public GameObject projectilePrefab;
    public int numPoints = 60;

    private Vector3 xVector;
    private Vector3 yVector;
    private GameObject[] segments;
    private GameObject hand;
    private Vector3 connectToPoint;
    private Vector3 connectFromPoint;
    private Vector3 columnHitOffset;
    //private Vector3 handForward;
    // private Vector3 handlaunchPoint;



    public LayerMask columnMask;
    public ColumnBehaviour selectedColumn;
    public ColumnBehaviour grabbedColumn;

    public float handTime;

    public LayerMask soulMask;
    private int numSouls;

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
        numSouls = 0;
    }

    // Update is called once per frame
    void Update()
    {
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

        if (selectedColumn && grabAction.GetStateDown(handType))
        {
            grabbedColumn = selectedColumn;
            columnHitOffset = hitPoint - grabbedColumn.transform.position;
            connectFromPoint = transform.position;
            handTime = 0;


        }
        else if (grabAction.GetStateUp(handType))
        {
            grabbedColumn = null;
            connectFromPoint = hand.transform.position;
            handTime = 0;
        }

        if (grabbedColumn)
        {
            //connectToPoint = grabbedColumn.platform.transform.position;
            connectToPoint = grabbedColumn.transform.position + columnHitOffset;
        }
        else
        {
            connectToPoint = transform.position;
        }

        if (shootAction.GetStateDown(handType))
        {
            Shoot();
        }

        UpdateLaser();
        UpdateConnectionLine();
        handTime += Time.deltaTime * 4f;


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

        //////

        float timeTotal = 2 * Vo * Mathf.Sin(angle * Mathf.Deg2Rad) / 9.8f;
        float interval = timeTotal / segments.Length;
        for (int i = 0; i < segments.Length; i++)
        {
            float time = interval * i;

            Vector3 xPos = xVector * Vx * time;
            Vector3 yPos = yVector * (Vy * time - 0.5f * 9.8f * time * time);

            segments[i].transform.position = transform.position + xPos + yPos;
        }

        //// Move column based on controller velocity
        if (grabbedColumn && angle > 20f)
        {

            if ((grabbedColumn.isUpColumn && (controllerPose.GetVelocity().y > 2f || controllerPose.GetAngularVelocity().x < -7f)) ||
               (!grabbedColumn.isUpColumn && (controllerPose.GetVelocity().y < -2f || controllerPose.GetAngularVelocity().x > 7f)))
            {
                grabbedColumn.Pulverize();
                grabbedColumn.movedBySource = transform;
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
    }
}
