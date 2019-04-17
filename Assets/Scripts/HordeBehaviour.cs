using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeBehaviour : MonoBehaviour {

    public int maxEnemies;
    public int numEnemies;
    private float spawnRate;

    public Transform playerPos;
    public Player player;

    private bool started;

    public GameObject[] enemyPrefabs;

	// Use this for initialization
	void Start ()
    {
        maxEnemies = 10;
        numEnemies = 0;
        spawnRate = 4f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        numEnemies = transform.childCount;
        if(spawnRate > 0.5f && started)
        {
            spawnRate -= 0.05f * Time.deltaTime;
        }

        maxEnemies = 10 + player.numDefeated;
        if(maxEnemies > 55)
        {
            maxEnemies = 55;
        }
	}

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            if (numEnemies < maxEnemies)
            {
                float randX = Random.Range(-20f, 20f);
                float randY = Random.Range(0.4f, 3.03f);
                Vector3 pos = new Vector3(randX, randY, 9);

                int enemyType = Random.Range(0, enemyPrefabs.Length);
                GameObject newEnemy = Instantiate(enemyPrefabs[enemyType], transform);
                newEnemy.transform.position = pos;
                EnemyCreature enemy = newEnemy.GetComponent<EnemyCreature>();
                enemy.playerPos = playerPos;
                enemy.player = player;

                float enemySpeed = (Mathf.Floor(player.timeAlive / 60) + 1) * 0.4f;
                if(enemySpeed > 3f)
                {
                    enemySpeed = 3f;
                }
                enemy.SetSpeed(Random.Range(0.2f, enemySpeed));
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }

    public void StartHorde()
    {
        StartCoroutine(SpawnEnemy());
        started = true;
    }
}
