using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Valve.VR;

public class Player : MonoBehaviour {

    private int numDefeated;
    private float timeAlive;
    private int healthPoints;
    private int totalhealthPoints;
    private bool isDead;

    public GameObject lifeOrbPrefab;

    public LaserPointer leftHand;
    public LaserPointer rightHand;

    public MenuUI menu;

    // Use this for initialization
    void Start ()
    {
        numDefeated = 0;
        timeAlive = 0;
        healthPoints = 10;
        totalhealthPoints = healthPoints;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timeAlive += Time.deltaTime;

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

        return System.Math.Round(minutes).ToString() + ":" + System.Math.Round(seconds).ToString();
    }

    public void RemoveHealthPoint(Transform enemyTransform)
    {
        GameObject orb = Instantiate(lifeOrbPrefab, transform.position, transform.rotation);
        LifeOrbBehaviour lifeOrb = orb.GetComponent<LifeOrbBehaviour>();
        lifeOrb.SetTarget(enemyTransform);
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
        StartCoroutine(ScreenFade());
    }

    // Creates a fade out and fade in effect for the entire display
    IEnumerator ScreenFade()
    {
        SteamVR_Fade.Start(Color.black, 2f);

        yield return new WaitForSeconds(2f);

        menu.gameObject.SetActive(true);
        menu.EndGame(GetNumDefeatedString(), GetTimeString(), 5);

        SteamVR_Fade.Start(Color.clear, 0.4f);

    }
}
