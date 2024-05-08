using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{

    [SerializeField]
    private float _seconds;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyObject", _seconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroyObject() 
    {
        Destroy(gameObject);
    }
}
