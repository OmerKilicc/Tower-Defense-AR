using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f;
    private Vector3 targetPosition;

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

        //vurdu mu vurmadý mý kontrolü
        if (transform.position == targetPosition)
        {
            //BURAYA DAMAGE EKLENECEK
            Destroy(gameObject); //mermiyi destroy
        }
    }
}


