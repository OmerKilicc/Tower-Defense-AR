using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * This script makes the enemy move between given waypoints
 * with a Linear Interpolation between them using Coroutines
 * and Vector3.Lerp
 * 
 */

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] List<Waypoint> tiles = new List<Waypoint>();
    [SerializeField] [Range(0f,5f)] float speed = 1f;

    void Start()
    {
        StartCoroutine(FollowPath());
        
    }

    IEnumerator FollowPath()
    {
        //iterating on all the waypoint
        foreach (var waypoint in tiles)
        {

			Vector3 startPosition = transform.position;
            Vector3 endPosition = waypoint.transform.position;
            float distanceCovered = 0f;

            //making it look at the direction which its moving
            transform.LookAt(endPosition);

            while (distanceCovered < 1f)
            {
                distanceCovered += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, distanceCovered);
				yield return new WaitForEndOfFrameUnit();
			}

        }
    }


}
