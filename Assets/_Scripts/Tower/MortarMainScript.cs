using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarMainScript : MonoBehaviour
{
    public List<Transform> enemiesInRange = new List<Transform>();
    public Transform ballPrefab;
    public Transform shootingPoint;
    public Transform target;
    public float fireRate = 0.5f; //saniye ba��na at�lacak mermi
    private bool isShooting = false;

    public float shootCooldown = 2f; //mermiler aras� beklenecek s�re
    private float currentCooldown = 0.0f; //mermiler aras� �u an ge�en s�re

    void Update()
    {
        target = ChooseTarget();

        if (target != null && !target.gameObject.activeInHierarchy)
        {
            target = null; // �u anki target inactive ise targeti s�f�rla
        }

        if (target != null)
        {
            currentCooldown -= Time.deltaTime;

            if (currentCooldown <= 0)
            {
                Shoot();
                currentCooldown = shootCooldown; //cooldown reset
            }

            //d��mana bakmas� i�in
            Vector3 targetDirection = target.position - transform.position;
            //targetDirection.y = 0; //yerinden yukar� a�a�� oynamas�n 
            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.transform); //target listesine ekle
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
            enemiesInRange.Remove(other.transform); //target listesinden c�kar
            if (other.transform == target) //az once rangeden ��kan enemy as�l hedefimizdiyse her �eyi s�f�rla ba�tan hedef se� vs vs
            {
                isShooting = false;
                target = null;
            }
            Debug.Log("enemy out of range");
        }
    }

    Transform ChooseTarget()
    {
        enemiesInRange.RemoveAll(enemy => enemy.gameObject.activeInHierarchy == false);

        if (enemiesInRange.Count > 0)
        {
            return enemiesInRange[0]; //FIFO us�l� d��man se�imi
        }
        return null;
    }

    void Shoot()
    {
        Debug.Log("Ate�!");
        isShooting = true;

        //y�n�n� belirleme merminin
        Vector3 directionToTarget = (target.position - shootingPoint.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        //mermi instantiate etme
        Transform ballInstance = Instantiate(ballPrefab, shootingPoint.position, lookRotation);

        ballInstance.GetComponent<Ball>().Initialize(target.position);

        //cooldown
        isShooting = false;

        //SoundManager.Instance.PlayOneShot(SoundManager.Sounds.PistolFire);
    }
}
