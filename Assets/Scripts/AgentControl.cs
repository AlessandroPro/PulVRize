using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentControl : MonoBehaviour {

    public Transform home;
    NavMeshAgent agent;

	// Use this for initialization
	void Start ()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(home.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
