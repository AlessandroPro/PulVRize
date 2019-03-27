using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public Rigidbody testBody;

	// Use this for initialization
	void Start () {


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        
        if (testBody.velocity.magnitude > 0.5f)
        {
            //testBody.velocity = Vector3.zero;
        }

    }
}
