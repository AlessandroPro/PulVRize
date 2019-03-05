using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreature : MonoBehaviour {

    public LayerMask columnMask;
    public GameObject explosionPrefab;

    private GameObject stuckColumn;
    private float yDiff;
    private bool isDestroyed;
	// Use this for initialization
	void Start ()
    {
        isDestroyed = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(stuckColumn)
        {
            float yPos = stuckColumn.transform.position.y + yDiff;
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(((1 << other.gameObject.layer) & columnMask) != 0)
        {
            if(stuckColumn && !isDestroyed)
            {
                DestroySelf();
            }
            else
            {
                stuckColumn = other.gameObject;
                yDiff = transform.position.y - stuckColumn.transform.position.y;
            }
        }
    }

    private void DestroySelf()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        isDestroyed = true;
        //GetComponent<ParticleSystem>().Play();
        //Destroy(this.gameObject);
    }
}
