using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IProjectile
{
    [SerializeField] private int _bulletDamage = 25;
    private Vector3 _targetPosition;

	private float _speed = 0.4f;
    public static event Action<int> OnEnemyDamaged;

	//towerdan çaðýrýlan fonksiyon
	public void Initialize(Vector3 targetPos)
    {
        _targetPosition = targetPos;
    }

    void Update()
    {
        //mermi hareketi
        float step = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            OnEnemyDamaged.Invoke(_bulletDamage);

			//TODO : make bullets inactive by adding them to objectpool
			Destroy(gameObject);
		}
	}
}


