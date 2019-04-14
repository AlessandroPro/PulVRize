using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreature : MonoBehaviour
{

    public LayerMask columnMask;
    public LayerMask forceFieldMask;
    public LayerMask lifeOrbMask;
    public GameObject explosionPrefab;
    public GameObject soulPrefab;
    public Transform playerPos;
    public Player player;
    private Vector3 target;
    private Animator anim;
    public Rigidbody body;

    private ColumnBehaviour stuckColumn;
    private float yDiff;
    private bool isDestroyed;
    public float speed;

    public float maxHeight;
    public float minHeight;
    public bool isLarge;

    private bool pushedAway;
    private bool hitColumnSide;
    private bool attackRecharge;
    private float pushAwaySpeed;
    private Vector3 pushAwayDir;
    private Vector3 pushHitpoint;

    private AudioSource audioSource;
    public AudioClip[] clipArray;

    // Use this for initialization
    void Start()
    {
        isDestroyed = false;
        speed = Random.Range(0.2f, 0.6f);

        float targetX = playerPos.position.x + Random.Range(-5, 5);
        float targetY = playerPos.position.y + Random.Range(0, 3f);
        float targetZ = playerPos.position.z + Random.Range(-3, 3);
        target = new Vector3(targetX, targetY, targetZ);

        anim = GetComponent<Animator>();

        if (isLarge)
        {
            minHeight = 0.67f;
            maxHeight = 2.75f;
        }
        else
        {
            minHeight = 0.4f;
            maxHeight = 3.03f;
        }

        pushedAway = false;
        hitColumnSide = false;
        attackRecharge = false;
        pushAwaySpeed = 10f;
        pushAwayDir = Vector3.zero;
        pushHitpoint = Vector3.zero;

        if (null != anim)
        {
            // Randomly selects idle animation
            int idleMode = Random.Range(1, 3);
            anim.SetInteger("IdleMode", idleMode);
        }

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = (playerPos.position - transform.position).magnitude;

        if (distanceToPlayer < 7)
        {
            target = playerPos.position;
        }

        Quaternion q = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 30 * Time.deltaTime);

        if (stuckColumn)
        {
            float yPos = stuckColumn.transform.position.y + yDiff;
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        }
        else if (pushedAway)
        {
            transform.position += pushAwayDir * Time.deltaTime * pushAwaySpeed;
        }
        else if (!hitColumnSide)
        {
            if (distanceToPlayer > 2.2f)
            {
                StopAnimAttack();
                transform.position += transform.forward * Time.deltaTime * speed;
            }
            else
            {
                StartAnimAttack();
                if (!attackRecharge)
                {
                    StartCoroutine("Attack");
                }
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

    private void FixedUpdate()
    {
        if (body.velocity.magnitude > 0)
        {
            body.velocity = Vector3.zero;
        }
    }

    private void PlaySound(int clipIndex, float delay)
    {
        if (clipIndex < clipArray.Length)
        {
            audioSource.clip = clipArray[clipIndex];
            audioSource.pitch = (Random.Range(0.8f, 1.1f));
            audioSource.volume = (Random.Range(0.3f, .4f));
            audioSource.PlayDelayed(delay);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & columnMask) != 0)
        {
            if (stuckColumn && !isDestroyed)
            {
                DestroySelf(stuckColumn.movedBySource);
            }
            else
            {
                ColumnBehaviour column = other.gameObject.GetComponent<ColumnBehaviour>();
                if ((column.isUpColumn && transform.position.y > column.platform.transform.position.y) ||
                    (!column.isUpColumn && transform.position.y < column.platform.transform.position.y))
                {
                    stuckColumn = column;
                    yDiff = transform.position.y - stuckColumn.transform.position.y;
                }
                else
                {
                    hitColumnSide = true;
                    pushedAway = false;
                }
            }
        }
        else if ((((1 << other.gameObject.layer) & forceFieldMask) != 0) && !hitColumnSide)
        {
            pushedAway = true;
            pushHitpoint = other.gameObject.transform.position;
            pushAwayDir = other.gameObject.transform.parent.transform.forward;
            if (null != anim)
            {
                anim.SetTrigger("ForceFieldHit");
                PlaySound(Random.Range(0, clipArray.Length + 5), 0);
            }
        }

        if(((1 << other.gameObject.layer) & lifeOrbMask) != 0)
        {
            CollectLifeOrb(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & columnMask) != 0)
        {
            hitColumnSide = false;
            stuckColumn = null;
        }
    }


    private void DestroySelf(Transform soulTarget)
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        GameObject soul = Instantiate(soulPrefab, transform.position, transform.rotation);
        soul.GetComponent<SoulBehaviour>().SetTarget(soulTarget);
        isDestroyed = true;
        Destroy(this.gameObject);
    }

    private void StartAnimAttack()
    {
        if (null != anim)
        {
            anim.SetBool("AttackMode", true);
        }
    }

    private void StopAnimAttack()
    {
        if (null != anim)
        {
            anim.SetBool("AttackMode", false);
        }
    }

    IEnumerator Attack()
    {
        attackRecharge = true;
        player.RemoveHealthPoint(transform);
        yield return new WaitForSeconds(2);
        attackRecharge = false;
    }

    private void CollectLifeOrb(GameObject lifeOrb)
    {
        var ps = lifeOrb.GetComponent<ParticleSystem>();
        ps.Stop();
        var main = ps.main;
        main.stopAction = ParticleSystemStopAction.Destroy;
    }
}
