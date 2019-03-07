﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeBehaviour : MonoBehaviour {

    public int maxEnemies;
    public int numEnemies;
    private float spawnRate;

    public GameObject enemyPrefab;
    public Transform playerPos;
   

	// Use this for initialization
	void Start ()
    {
        maxEnemies = 50;
        numEnemies = 0;
        spawnRate = 1;
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
                float randX = Random.Range(-10f, 10f);
                float randY = Random.Range(playerPos.position.y, playerPos.position.y + 3f);
                Vector3 pos = new Vector3(randX, randY, 15);
                Quaternion rot = Quaternion.identity;
                GameObject newEnemy = Instantiate(enemyPrefab, transform);
                newEnemy.transform.position = pos;
                newEnemy.GetComponent<EnemyCreature>().playerPos = playerPos;
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }
}