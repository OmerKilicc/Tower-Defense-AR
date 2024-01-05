using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMainScript : MonoBehaviour
{
    public List<Transform> enemiesInRange = new List<Transform>();
    public Transform bulletPrefab;
    public Transform shootingPoint;
    public Transform target;

    void Start()
    {
        
    }

    void Update()
    {
        Transform target = ChooseTarget();
        if (target != null)
        {
            //d��mana bakmas� i�in
            Vector3 targetDirection = target.position - transform.position;
            targetDirection.y = 0; //yerinden yukar� a�a�� oynamas�n 
            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.transform);
            Shoot();
            Debug.Log("enemy in range");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.transform);
            //BURDA ATE�� KESME EKLENECEK
            Debug.Log("enemy out of range");
        }
    }

    Transform ChooseTarget()
    {
        if (enemiesInRange.Count > 0)
        {
            return enemiesInRange[0]; //FIFO us�l� d��man se�imi
        }
        return null;
    }

    void Shoot()
    {
        //bullet instantiate etme
        Transform bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);

        //at�� y�n�
        Vector3 shootingDirection = (target.position - shootingPoint.position).normalized;

        //at�� hedefi
        bullet.GetComponent<Bullet>().Initialize(target.position);
    }
}
