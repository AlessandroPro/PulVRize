using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBehaviour : MonoBehaviour {

    protected Transform target;
    protected Vector3 targetPoint;

    protected float forwardSpeed;
    protected float turnSpeed;

    // Use this for initialization
    public void Start ()
    {
        targetPoint = Vector3.zero;
        forwardSpeed = 4;
        turnSpeed = 300;
    }

    // Update is called once per frame
    public virtual void Update ()
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

    public void SetSpeed(float speed)
    {
        forwardSpeed = speed;
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    public IEnumerator WaitThenDie()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
