using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnBehaviour : MonoBehaviour {

    public bool selected;
    public GameObject shaft;
    public GameObject platform;

    public ConstantForce verticalForce;
    private Vector3 riseForce;
    private Vector3 fallForce;
	// Use this for initialization
	void Start () {
        selected = false;
        verticalForce.force = Vector3.zero;

        riseForce = new Vector3(0, 15f, 0);
        fallForce = new Vector3(0, -8f, 0);
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

        if(transform.position.y < 0.15f)
        {
            verticalForce.force = Vector3.zero;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, 0.15f, transform.position.z);
        }
        else if(transform.position.y > 3.02)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            verticalForce.force = fallForce;
        }
	}

    public void Rise()
    {
        verticalForce.force = riseForce;
    }
}
