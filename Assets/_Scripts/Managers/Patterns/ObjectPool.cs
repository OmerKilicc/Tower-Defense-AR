using System;
using System.Collections.Generic;
using UnityEngine;

// Object pool implementation
public class ObjectPool<T> where T : MonoBehaviour
{
	private Queue<T> pool;
	private int maxSize;
	private T prefab;

	public ObjectPool(int maxSize, T prefab)
	{
		this.maxSize = maxSize;
		this.prefab = prefab;
		pool = new Queue<T>();
	}

	public T GetObject()
	{
		if (pool.Count > 0)
		{
			T obj = pool.Dequeue();
			obj.gameObject.SetActive(true); // Activate GameObject
											// No need to reset state since we're assuming it's reset upon deactivation
			return obj;
		}
		else
		{
			T newObj = GameObject.Instantiate(prefab); // Instantiate new GameObject
			return newObj;
		}
	}

	public void ReturnObject(T obj)
	{
		obj.gameObject.SetActive(false); // Deactivate GameObject
		if (pool.Count < maxSize)
		{
			pool.Enqueue(obj);
		}
		else
		{
			GameObject.Destroy(obj.gameObject); // Destroy excess objects
		}
	}
}
