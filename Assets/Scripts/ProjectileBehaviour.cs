using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {

    private float speed;
    private float timeAlive;
    private float maxTime;
	// Use this for initialization
	void Start ()
    {
        speed = 10f;
        timeAlive = 0;
        maxTime = 3f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += transform.forward * Time.deltaTime * speed;

        timeAlive += Time.deltaTime;
        
        if(timeAlive > maxTime)
        {
            Destroy(this.gameObject);
        }
	}
}
