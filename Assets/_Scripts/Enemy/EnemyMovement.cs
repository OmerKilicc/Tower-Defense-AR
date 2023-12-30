using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] List<Waypoint> tiles = new List<Waypoint>();
    [SerializeField] float waitTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FollowPath());
        
    }

    IEnumerator FollowPath()
    {
        foreach (var waypoint in tiles)
        {
            transform.position = waypoint.transform.position;
            Debug.Log("z");
            yield return new WaitForSeconds(waitTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
