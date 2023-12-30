using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlaneHandler : MonoBehaviour
{
    private ARPlane _plane;

    private void Awake()
    {
        _plane = GetComponent<ARPlane>();
        _plane.boundaryChanged += OnPlaneBoundariesChanged;
    }

    private void OnPlaneBoundariesChanged(ARPlaneBoundaryChangedEventArgs args)
    {
        if (ARManager.Instance != null)
        {
            ARManager.Instance.BoundariesUpdated(_plane.size.x * _plane.size.y);
        }
    }
}
