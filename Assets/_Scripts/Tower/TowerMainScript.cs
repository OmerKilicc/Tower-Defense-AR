using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TowerMainScript : MonoBehaviour
{
    public List<Transform> enemiesInRange = new List<Transform>();
    public Transform bulletPrefab;
    public Transform shootingPoint;
    public Transform target;
    public float fireRate = 1f; //saniye ba��na at�lacak mermi
    private bool isShooting = false;
    TargetPoint targetp;
    public float damagePerSecond = 65f;

    public float shootCooldown = 1.0f; //mermiler aras� beklenecek s�re
    private float currentCooldown = 0.0f; //mermiler aras� ge�en s�re

    private GameObject randomEnemy;

    void Update()
    {
        target = ChooseTarget();

        if (target != null && !target.gameObject.activeInHierarchy)
        {
            target = null; // �u anki target inactive ise targeti s�f�rla
        }
        //if (target == null)
        //{
        //    target = ChooseTarget();
        //}
        if (target != null)
        {
            currentCooldown -= Time.deltaTime;

            if (currentCooldown <= 0)
            {
                Shoot();
                currentCooldown = shootCooldown; //cooldown reset
            }

            //d��mana bakmas� i�in
            Vector3 targetDirection = target.position - shootingPoint.position;
            targetDirection.y = 0; //yerinden yukar� a�a�� oynamas�n 
            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
            shootingPoint.rotation = Quaternion.Slerp(shootingPoint.rotation, lookRotation, Time.deltaTime * 5f);
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
        enemiesInRange.RemoveAll(enemy => enemy == null);

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
        Transform bulletInstance = Instantiate(bulletPrefab, shootingPoint.position, lookRotation);

        bulletInstance.GetComponent<Bullet>().Initialize(target.position);



        //cooldown
        isShooting = false;

        //SoundManager.Instance.PlayOneShot(SoundManager.Sounds.PistolFire);
    }

    //void Shoot()
    //{
    //    Vector3 point = target.position;
    //    shootingPoint.LookAt(point);
    //    bulletPrefab.localRotation = shootingPoint.localRotation;

    //    float d = Vector3.Distance(shootingPoint.position, point);
    //    bulletPrefab.localPosition =
    //        shootingPoint.localPosition + 0.5f * d * bulletPrefab.forward;
    //    //targetp.Enemy.ApplyDamage(damagePerSecond * Time.deltaTime);
    //}
}
