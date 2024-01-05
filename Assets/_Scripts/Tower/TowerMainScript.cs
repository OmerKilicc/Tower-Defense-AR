using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMainScript : MonoBehaviour
{
    public List<Transform> enemiesInRange = new List<Transform>();
    public Transform bulletPrefab;
    public Transform shootingPoint;
    public Transform target;
    public float fireRate = 1f; //saniye baþýna atýlacak mermi
    private bool isShooting = false;

    public float shootCooldown = 1.0f; // cooldown between shots
    private float currentCooldown = 0.0f;

    void Update()
    {
        target = ChooseTarget();

        if (target != null)
        {
            currentCooldown -= Time.deltaTime;

            if (currentCooldown <= 0)
            {
                Shoot();
                currentCooldown = shootCooldown; // reset the cooldown
            }

            //düþmana bakmasý için
            Vector3 targetDirection = target.position - transform.position;
            targetDirection.y = 0; //yerinden yukarý aþaðý oynamasýn 
            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.transform);
            if (!isShooting)
            {
                Shoot();
            }
            Debug.Log("enemy in range");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.transform);
            if (other.transform == target)
            {
                isShooting = false;
                target = null;
            }
            Debug.Log("enemy out of range");
        }
    }

    Transform ChooseTarget()
    {
        if (enemiesInRange.Count > 0)
        {
            return enemiesInRange[0]; //FIFO usülü düþman seçimi
        }
        return null;
    }

    void Shoot()
    {
        Debug.Log("Ateþ!");
        isShooting = true;

        //yönünü belirleme merminin
        Vector3 directionToTarget = (target.position - shootingPoint.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        //mermi instantiate etme
        Transform bulletInstance = Instantiate(bulletPrefab, shootingPoint.position, lookRotation);

        bulletInstance.GetComponent<Bullet>().Initialize(target.position);

        //cooldown
        isShooting = false;
    }
}
