using System;
using System.Collections.Generic;
using UnityEngine;

// Factory pattern implementation
public static class ObjectFactory
{
	//Key olarak Type alan, value olarak obje alan bir Dictionary yaratior poolu takip etmek icin
	private static Dictionary<Type, object> pools = new Dictionary<Type, object>();

	public static void RegisterPool<T>(int maxSize, T prefab) where T : MonoBehaviour
	{
		//Poolda T tipinde bir anahtar  yoksa
		if (!pools.ContainsKey(typeof(T)))
		{
			//o keye bir object pool ata,yani eger mesela bir tower tipinde pool yoksa
			//tower tipi i�in bir pool olu�tur diyore
			pools[typeof(T)] = new ObjectPool<T>(maxSize, prefab);
		}
	}

	//MonoBehaviour t�r�nde bir obje d�nd�ren fonksiyon
	public static T CreateObject<T>() where T : MonoBehaviour
	{
		//eger poolda T tipinde key varsa
		if (pools.ContainsKey(typeof(T)))
		{
			//o tipe ait object poolun i�inden o objeyi ver
			return (pools[typeof(T)] as ObjectPool<T>).GetObject();
		}
		else
		{
			throw new ArgumentException($"No pool registered for {typeof(T)}");
		}
	}

	public static void ReturnObject<T>(T obj) where T : MonoBehaviour
	{
		//eger poolda T tipinde key varsa
		if (pools.ContainsKey(typeof(T)))
		{
			//o tipe ait object poolun i�ine o objeyi uyut
			(pools[typeof(T)] as ObjectPool<T>).ReturnObject(obj);
		}
		else
		{
			throw new ArgumentException($"No pool registered for {typeof(T)}");
		}
	}
}
