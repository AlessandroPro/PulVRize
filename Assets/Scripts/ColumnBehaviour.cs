using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnBehaviour : MonoBehaviour {

    public bool selected;
    public bool moving;
    public bool isUpColumn;
    public bool isColliding;
    public GameObject shaft;
    public GameObject platform;
    public LayerMask columnMask;

    public ConstantForce verticalForce;
    public Vector3 riseForce;
    public Vector3 fallForce;
    private float initialPosY;
	// Use this for initialization
	void Start () {
        selected = false;
        moving = false;
        isColliding = false;

        verticalForce.force = Vector3.zero;

        initialPosY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update ()
    {

		if(selected)
        {
            platform.SetActive(true);
        }
        else
        {
            platform.SetActive(false);
        }

        if ((isUpColumn && transform.position.y < initialPosY) ||
            (!isUpColumn && transform.position.y > initialPosY))
        {
            StopMoving();
        }
	}

    public void Pulverize()
    {
        if(isUpColumn)
        {
            Rise();
        }
        else
        {
            Fall();
        }
    }


    private void Rise()
    {
        if(!isColliding)
        {
            verticalForce.force = Vector3.zero;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            verticalForce.force = riseForce;
            moving = true;
        }
    }

    private void Fall()
    {
        if (!isColliding)
        {
            verticalForce.force = Vector3.zero;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            verticalForce.force = fallForce;
            moving = true;
        }
    }

    private void StopMoving()
    {
        verticalForce.force = Vector3.zero;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, initialPosY, transform.position.z);
        moving = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & columnMask) != 0)
        {
            if (isUpColumn)
            {
                Fall();
            }
            else
            {
                Rise();
            }

            isColliding = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & columnMask) != 0)
        {
            isColliding = false;
        }
    }
}
