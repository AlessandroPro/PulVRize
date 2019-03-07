using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreature : MonoBehaviour {

    public LayerMask columnMask;
    public GameObject explosionPrefab;
    public Transform playerPos;
    private Vector3 target;
    private Animator anim;

    private GameObject stuckColumn;
    private float yDiff;
    private bool isDestroyed;
    public float speed;
	// Use this for initialization
	void Start ()
    {
        isDestroyed = false;
        speed = Random.Range(0.1f, 0.3f);

        float targetX = playerPos.position.x + Random.Range(-5, 5);
        float targetY = playerPos.position.y + Random.Range(0, 3f);
        float targetZ = playerPos.position.z + Random.Range(-3, 3);
        target = new Vector3(targetX, targetY, targetZ);

        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {

        if((playerPos.position - transform.position).magnitude < 7)
        {
            target = playerPos.position;
        }

		if(stuckColumn)
        {
            float yPos = stuckColumn.transform.position.y + yDiff;
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        }
        else if((playerPos.position - transform.position).magnitude > 3)
        {
            transform.position += transform.forward * Time.deltaTime * speed;
        }
        Quaternion q = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 30 * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (null != anim)
            {
                anim.SetBool("AttackMode", true);
                Debug.Log("HELP");
            }
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
                if(Physics.Raycast(transform.position, Vector3.down, 1, columnMask) ||
                    Physics.Raycast(transform.position, Vector3.up, 1, columnMask))
                {
                    stuckColumn = other.gameObject;
                    yDiff = transform.position.y - stuckColumn.transform.position.y;
                }
            }
        }
    }

    private void DestroySelf()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        isDestroyed = true;
        Destroy(this.gameObject);
    }
}
