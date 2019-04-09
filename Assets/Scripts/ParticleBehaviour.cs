using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehaviour : MonoBehaviour {

    private AudioSource audioSource;
    public AudioClip[] clipArray;

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(WaitThenDie());
        PlaySound(Random.Range(0, clipArray.Length), 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator WaitThenDie()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }

    private void PlaySound(int clipIndex, float delay)
    {
        if(clipIndex < clipArray.Length)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = clipArray[clipIndex];
            audioSource.pitch = (Random.Range(0.8f, 1.2f));
            audioSource.volume = (Random.Range(0.5f, .8f));
            audioSource.PlayDelayed(delay);
        }
    }
}
