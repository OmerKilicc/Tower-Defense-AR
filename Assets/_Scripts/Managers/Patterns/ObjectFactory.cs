using System;
using System.Collections.Generic;
using UnityEngine;

// Factory pattern implementation
public static class GameObjectFactory
{
	private static Dictionary<Type, object> pools = new Dictionary<Type, object>();

	public static void RegisterPool<T>(int maxSize, T prefab) where T : MonoBehaviour
	{
		if (!pools.ContainsKey(typeof(T)))
		{
			pools[typeof(T)] = new ObjectPool<T>(maxSize, prefab);
		}
	}

	public static T CreateObject<T>() where T : MonoBehaviour
	{
		if (pools.ContainsKey(typeof(T)))
		{
			return (pools[typeof(T)] as ObjectPool<T>).GetObject();
		}
		else
		{
			throw new ArgumentException($"No pool registered for {typeof(T)}");
		}
	}

	public static void ReturnObject<T>(T obj) where T : MonoBehaviour
	{
		if (pools.ContainsKey(typeof(T)))
		{
			(pools[typeof(T)] as ObjectPool<T>).ReturnObject(obj);
		}
		else
		{
			throw new ArgumentException($"No pool registered for {typeof(T)}");
		}
	}
}
