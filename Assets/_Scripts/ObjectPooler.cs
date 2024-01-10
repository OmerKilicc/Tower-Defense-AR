using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] int poolSize = 5;
	[SerializeField] float spawnTimer = 1f;

	GameObject[] pool;

	void Awake()
	{
		PopulatePool(enemyPrefab);
		//TODO: Populate the pool with bullets
	}

	void Start()
	{
		StartCoroutine(SpawnObject(enemyPrefab));
	}

	void PopulatePool(GameObject prefabToPopulate)
	{
		pool = new GameObject[poolSize];

		for (int i = 0; i < pool.Length; i++)
		{
			pool[i] = Instantiate(prefabToPopulate, transform);
			pool[i].SetActive(false);
		}
	}

	void EnableObjectInPool()
	{
		for (int i = 0; i < pool.Length; i++)
		{
			if (pool[i].activeInHierarchy == false)
			{
				pool[i].SetActive(true);
				return;
			}
		}
	}

	IEnumerator SpawnObject(GameObject prefabToSpawn)
	{
		while (true)
		{
			Instantiate(prefabToSpawn, transform);
			EnableObjectInPool();
			yield return new WaitForSeconds(spawnTimer);
		}
	}

}
