using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreature : MonoBehaviour {

    public LayerMask columnMask;
    public LayerMask forceFieldMask;
    public GameObject explosionPrefab;
    public Transform playerPos;
    private Vector3 target;
    private Animator anim;

    private GameObject stuckColumn;
    private float yDiff;
    private bool isDestroyed;
    public float speed;

    public float maxHeight;
    public float minHeight;

    private bool pushedAway;
    private float pushAwaySpeed;
    private Vector3 pushAwayDir;
    private Vector3 pushHitpoint;
	// Use this for initialization
	void Start ()
    {
        isDestroyed = false;
        speed = Random.Range(0.2f, 0.7f);

        float targetX = playerPos.position.x + Random.Range(-5, 5);
        float targetY = playerPos.position.y + Random.Range(0, 3f);
        float targetZ = playerPos.position.z + Random.Range(-3, 3);
        target = new Vector3(targetX, targetY, targetZ);

        anim = GetComponent<Animator>();

        minHeight = 0.4f;
        maxHeight = 3.03f;

        pushedAway = false;
        pushAwaySpeed = 10f;
        pushAwayDir = Vector3.zero;
        pushHitpoint = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update ()
    {
        float distanceToPlayer = (playerPos.position - transform.position).magnitude;

        if ( distanceToPlayer < 7)
        {
            target = playerPos.position;
        }

		if(stuckColumn)
        {
            float yPos = stuckColumn.transform.position.y + yDiff;
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        }
        else if(pushedAway)
        {
            transform.position += pushAwayDir * Time.deltaTime * pushAwaySpeed;
        }
        else if(distanceToPlayer > 0.5)
        {
            transform.position += transform.forward * Time.deltaTime * speed;
            Quaternion q = Quaternion.LookRotation(target - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 30 * Time.deltaTime);
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (null != anim)
            {
                anim.SetBool("AttackMode", true);
                Debug.Log("HELP");
            }
        }

        if (!stuckColumn)
        {
            if (transform.position.y > maxHeight)
            {
                transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);
                pushedAway = false;
            }
            else if (transform.position.y < minHeight)
            {
                transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
                pushedAway = false;
            }
        }

        if ((transform.position - pushHitpoint).magnitude > 5f)
        {
            pushedAway = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & columnMask) != 0)
        {
            if (stuckColumn && !isDestroyed)
            {
                DestroySelf();
            }
            else
            {
                if (Physics.Raycast(transform.position, Vector3.down, 1, columnMask) ||
                    Physics.Raycast(transform.position, Vector3.up, 1, columnMask))
                {
                    stuckColumn = other.gameObject;
                    yDiff = transform.position.y - stuckColumn.transform.position.y;
                }
            }
        }
        else if (((1 << other.gameObject.layer) & forceFieldMask) != 0)
        {
            pushedAway = true;
            pushHitpoint = other.gameObject.transform.position;
            pushAwayDir = other.gameObject.transform.parent.transform.forward;
        }
    }


    private void DestroySelf()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        isDestroyed = true;
        Destroy(this.gameObject);
    }
}
