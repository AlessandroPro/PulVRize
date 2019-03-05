using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class LaserPointer : MonoBehaviour {

    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;

    public LineRenderer laser;
    private Vector3 hitPoint;

    public GameObject projectilePrefab;
    public GameObject hand;
    public int numPoints = 120;

    private Vector3 xVector;
    private Vector3 yVector;
    private GameObject[] projectiles;
    private Vector3 connectedPoint;
    private Vector3 handForward;
    private Vector3 handlaunchPoint;

    public LayerMask columnMask;
    public ColumnBehaviour selectedColumn;
    public ColumnBehaviour grabbedColumn;
    

    // Use this for initialization
    void Start ()
    {
        laser.enabled = true;
        hitPoint = transform.forward * 100;

        connectedPoint = new Vector3(0, 0, 0);

        projectiles = new GameObject[numPoints];
        for (int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i] = Instantiate(projectilePrefab);
        }

        hand = Instantiate(hand);
        handForward = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update ()
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
            if(selectedColumn != null)
            {
                DeselectColumn();
            }
        }

        if (selectedColumn & grabAction.GetStateDown(handType))
        {
            grabbedColumn = selectedColumn;
        }

        if(grabbedColumn)
        {
            connectedPoint = grabbedColumn.platform.transform.position;
        }

        UpdateLaser();
        UpdateConnectionLine();


    }

    private void UpdateLaser()
    { 
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, hitPoint);
    }

    private void SelectColumn(GameObject column)
    {
        selectedColumn = column.GetComponent<ColumnBehaviour>();
        selectedColumn.selected = true;
    }

    private void DeselectColumn()
    {
        selectedColumn.selected = false;
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
        //////

        float timeTotal = 2 * Vo * Mathf.Sin(angle * Mathf.Deg2Rad) / 9.8f;
        float interval = timeTotal / projectiles.Length;
        for (int i = 0; i < projectiles.Length; i++)
        {
            float time = interval * i;

            Vector3 xPos = xVector * Vx * time;
            Vector3 yPos = yVector * (Vy * time - 0.5f * 9.8f * time * time);

            projectiles[i].transform.position = transform.position + xPos + yPos;
        }

        //// Raise column based on controller velocity
        if(grabbedColumn && angle > 24f)
        {
            if((grabbedColumn.isUpColumn && controllerPose.GetVelocity().y > 3f) ||
               (!grabbedColumn.isUpColumn && controllerPose.GetVelocity().y < -3f))
            {
                grabbedColumn.Pulverize();
            }
        }
    }
    
}
