using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a generic object pool that works with queue to ensure optimization in game
/// </summary>
/// <typeparam name="T"></typeparam>

public class ObjectPool<T> where T : MonoBehaviour
{
	//Yine tipi fark etmeyen bir Queue ve prefab var
	private Queue<T> _pool;
	private int _maxSize;
	private T _prefab;

	//Constructor
	public ObjectPool(int maxSize, T prefab)
	{
		this._maxSize = maxSize;
		this._prefab = prefab;
		_pool = new Queue<T>();
	}

	//her tipten objeyi dondurebilen bir metot
	public T GetObject()
	{

		//pool bos degils, tipi fark etmeyen objeyi pooldan cýkart ve active et sonra da active ettigini dondur
		if (_pool.Count > 0)
		{
			T obj = _pool.Dequeue();
			obj.gameObject.SetActive(true); // Activate GameObject
											// No need to reset state since we're assuming it's reset upon deactivation
			return obj;
		}
		//pool bos ise prefabý verilen tipten  yeni bir obje yarat
		else
		{
			T newObj = GameObject.Instantiate(_prefab); // Instantiate new GameObject
			return newObj;
		}
	}

	//Tipi fark etmeyen bir obje alýyor parametre olarak
	public void ReturnObject(T obj)
	{
		//aldýgý objeyi deactive ediyor, eger pool dolu ise siliyor, degilse poola ekliyor
		obj.gameObject.SetActive(false); // Deactivate GameObject
		if (_pool.Count < _maxSize)
		{
			_pool.Enqueue(obj);
		}
		else
		{
			GameObject.Destroy(obj.gameObject); // Destroy excess objects
		}
	}
}
