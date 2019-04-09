using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {

    private float speed;
    private float timeAlive;
    private float maxTime;

    private AudioSource audioSource;
    private int clipIndex;
    public AudioClip[] clipArray;
	// Use this for initialization
	void Start ()
    {
        speed = 10f;
        timeAlive = 0;
        maxTime = 3f;

        audioSource = GetComponent<AudioSource>();
        clipIndex = Random.Range(0, clipArray.Length);
        audioSource.clip = clipArray[clipIndex];
        audioSource.pitch = (Random.Range(1f, 2f));
        audioSource.volume = (Random.Range(0.4f, .7f));
        audioSource.Play();
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
