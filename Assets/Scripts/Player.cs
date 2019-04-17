using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Valve.VR;

public class Player : MonoBehaviour {

    public int numDefeated;
    public float timeAlive;
    private int healthPoints;
    private int totalhealthPoints;
    private bool isDead;

    public GameObject lifeOrbPrefab;

    public LaserPointer leftHand;
    public LaserPointer rightHand;

    public MenuUI menu;

    private AudioSource audioSource;

    // Use this for initialization
    void Start ()
    {
        numDefeated = 0;
        timeAlive = 0;
        healthPoints = 70;
        totalhealthPoints = healthPoints;
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(menu.invalid)
        {
            timeAlive += Time.deltaTime;
        }
        

        if(healthPoints <= 0 && !isDead)
        {
            Die();
        }
	}

    public void increaseNumDefeated(int num)
    {
        numDefeated += num;
    }

    public string GetNumDefeatedString()
    {
        return numDefeated.ToString();
    }

    public string GetTimeString()
    {
        double minutes = Mathf.Floor(timeAlive / 60);
        double seconds = timeAlive % 60;

        string minutesString = System.Math.Round(minutes).ToString();
        string secondsString = System.Math.Round(seconds).ToString();

        if(System.Math.Round(seconds) < 10)
        {
            secondsString = "0" + secondsString;
        }
        return minutesString + ":" + secondsString;
    }

    public void RemoveHealthPoint(Transform enemyTransform)
    {
        GameObject orb = Instantiate(lifeOrbPrefab, transform.position, transform.rotation);
        LifeOrbBehaviour lifeOrb = orb.GetComponent<LifeOrbBehaviour>();
        lifeOrb.SetTarget(enemyTransform);
        lifeOrb.SetSource(transform);
        lifeOrb.SetSpeed(1);
        healthPoints--;
        if(healthPoints < 0)
        {
            healthPoints = 0;
        }
        lifeOrb.SetNewGradient(healthPoints / (float)totalhealthPoints);
    }

    public void Die()
    {
        isDead = true;
        audioSource.Play();
        leftHand.RemoveAllSouls();
        rightHand.RemoveAllSouls();

        StartCoroutine(ScreenFade());
    }

    // Creates a fade out and fade in effect for the entire display
    IEnumerator ScreenFade()
    {
        SteamVR_Fade.Start(Color.black, 2f);

        yield return new WaitForSeconds(4f);

        menu.gameObject.SetActive(true);
        int finalScore = numDefeated + (int) Mathf.Pow(timeAlive, 1.1f);
        menu.EndGame(GetNumDefeatedString(), GetTimeString(), finalScore);

        SteamVR_Fade.Start(Color.clear, 0.6f);

    }
}
