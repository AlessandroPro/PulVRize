using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeBehaviour : MonoBehaviour {

    public int maxEnemies;
    public int numEnemies;
    private float spawnRate;

    //public GameObject redEnemyPrefab;
    //public GameObject darkEnemyPrefab;
    public Transform playerPos;

    public GameObject[] enemyPrefabs;

	// Use this for initialization
	void Start ()
    {
        maxEnemies = 40;
        numEnemies = 0;
        spawnRate = 0.5f;
        
        StartCoroutine(SpawnEnemy());
	}
	
	// Update is called once per frame
	void Update ()
    {
        numEnemies = transform.childCount;
	}

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            if (numEnemies < maxEnemies)
            {
                float randX = Random.Range(-20f, 20f);
                float randY = Random.Range(0.4f, 3.03f);
                Vector3 pos = new Vector3(randX, randY, 12);
                Quaternion rot = Quaternion.identity;

                int enemyType = Random.Range(0, enemyPrefabs.Length);
                GameObject newEnemy = Instantiate(enemyPrefabs[enemyType], transform);
                newEnemy.transform.position = pos;
                newEnemy.GetComponent<EnemyCreature>().playerPos = playerPos;
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
