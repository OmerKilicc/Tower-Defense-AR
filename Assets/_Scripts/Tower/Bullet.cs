using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int bulletDamage = 25;
    public float speed = 50f;
    private Vector3 targetPosition;
    EnemyData enemyData;

    //towerdan çaðýrýlan fonksiyon
    public void Initialize(Vector3 targetPos)
    {
        targetPosition = targetPos;
    }

    void Update()
    {
        //mermi hareketi
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemyData = other.GetComponent<EnemyData>();
            enemyData.Health -= bulletDamage;
            
            if(enemyData.Health <= 0) 
            {
				//TODO: Add enemyData.Reward to total money of player
                Destroy(other.gameObject);
			}

			//BURAYA DAMAGE EKLENECEK
			Destroy(gameObject); //mermiyi destroy
        }
    }
}


