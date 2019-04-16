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
        maxEnemies = 45;
        numEnemies = 0;
        spawnRate = 2.5f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        numEnemies = transform.childCount;
        if(spawnRate > 0.5f && started)
        {
            spawnRate -= 0.1f * Time.deltaTime;
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
