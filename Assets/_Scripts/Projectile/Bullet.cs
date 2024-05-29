using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour, IProjectile
{
    [SerializeField] private int _bulletDamage = 10000;
    private Vector3 _targetPosition;

    private float _speed = 1.55f;
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
        if(Vector3.Distance(_targetPosition,transform.position) < 0.03f) 
        {
            Invoke("DestroyShell", 0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<NewEnemy>().ApplyDamage(_bulletDamage);
            //OnEnemyDamaged.Invoke(_bulletDamage);

            //TODO : make bullets inactive by adding them to objectpool
            DestroyShell();

        }
	}

    private void DestroyShell() 
    {
        Destroy(gameObject);
    }
}


