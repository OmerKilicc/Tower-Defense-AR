using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{

    public float Speed = 1;
    public float Amplitude = 1;

    Vector3 _startScale;
    Vector3 _startRotation;

    // Start is called before the first frame update
    void Start()
    {
        _startScale = transform.localScale;
        _startRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = _startScale + Vector3.one * Mathf.Sin(Time.time*Speed)*Amplitude;
        transform.rotation.SetEulerAngles(_startRotation.x,_startRotation.y,_startRotation.z + Mathf.Sin(Time.time * Speed) * Amplitude);
    }
}
