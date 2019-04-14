using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private int numDefeated;
    private float timeAlive;
    private int healthPoints;

    public GameObject lifeOrbPrefab;

    public LaserPointer leftHand;
    public LaserPointer rightHand;

    public Gradient gradient;

    // Use this for initialization
    void Start ()
    {
        numDefeated = 0;
        timeAlive = 0;
        healthPoints = 20;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timeAlive = Time.deltaTime;
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
        SoulBehaviour lifeOrb = orb.GetComponent<SoulBehaviour>();
        lifeOrb.SetTarget(enemyTransform);
        lifeOrb.SetSpeed(1);
        healthPoints--;

        //leftHand.Pulse(2f, 50, 200);
        //rightHand.Pulse(2f, 50, 200);
    }
}
