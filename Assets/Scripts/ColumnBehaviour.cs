using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnBehaviour : MonoBehaviour {

    public bool selected;
    public bool moving;
    public bool isUpColumn;
    public bool isColliding;
    public GameObject shaft;
    public GameObject platform;
    public LayerMask columnMask;

    public ConstantForce verticalForce;
    public Vector3 riseForce;
    public Vector3 fallForce;
    private float initialPosY;
    public Transform movedBySource;

    public Material regularMat;
    public Material highlightMat;

    public GameObject dustPuffPrefab;

    private AudioSource audioSource;
    public AudioClip[] clipArray;

    // Use this for initialization
    void Start () {
        selected = false;
        moving = false;
        isColliding = false;

        verticalForce.force = Vector3.zero;

        initialPosY = transform.position.y;

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

        if ((isUpColumn && transform.position.y < initialPosY) ||
            (!isUpColumn && transform.position.y > initialPosY))
        {
            StopMoving();
        }
	}

    private void PlaySound(int clipIndex, float delay)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clipArray[clipIndex];
        audioSource.pitch = (Random.Range(0.8f, 1.1f));
        audioSource.volume = (Random.Range(0.3f, .4f));
        audioSource.PlayDelayed(delay);
    }

    public void Pulverize()
    {
        if(isUpColumn)
        {
            Rise();
        }
        else
        {
            Fall();
        }
    }


    private void Rise()
    {
        if(!isColliding)
        {
            PlaySound(0, 0.05f);
            verticalForce.force = Vector3.zero;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            verticalForce.force = riseForce;
            moving = true;
        }
    }

    private void Fall()
    {
        PlaySound(1, 0.05f);
        if (!isColliding)
        {
            verticalForce.force = Vector3.zero;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            verticalForce.force = fallForce;
            moving = true;
        }
    }

    private void StopMoving()
    {
        verticalForce.force = Vector3.zero;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, initialPosY, transform.position.z);
        moving = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & columnMask) != 0)
        {
            if (isUpColumn)
            {
                Fall();
            }
            else
            {
                Rise();
            }

            isColliding = true;
            Instantiate(dustPuffPrefab, platform.transform.position, dustPuffPrefab.transform.rotation);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & columnMask) != 0)
        {
            isColliding = false;
        }
    }

    public void Highlight()
    {
        shaft.GetComponent<MeshRenderer>().material = highlightMat;
    }

    public void RemoveHighlight()
    {
        shaft.GetComponent<MeshRenderer>().material = regularMat;
    }
}
