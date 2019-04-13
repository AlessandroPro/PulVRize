using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBehaviour : MonoBehaviour {

    public Transform target;
    private Vector3 targetPoint;

    private float forwardSpeed;
    private float turnSpeed;
    // Use this for initialization
    void Start ()
    {
        targetPoint = Vector3.zero;
        forwardSpeed = 4;
        turnSpeed = 300;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (target)
        {
            targetPoint = target.position;
        }
        else
        {
            Destroy(this.gameObject);
        }
        transform.position += transform.forward * Time.deltaTime * forwardSpeed;
        Quaternion q = Quaternion.LookRotation(targetPoint - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, turnSpeed * Time.deltaTime);
    }
}
