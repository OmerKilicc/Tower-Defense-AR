using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraLocationDebug : MonoBehaviour
{
    [SerializeField]
    TMP_Text _text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = transform.position.ToString();
    }
}
