using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Linq;
using System;

public class ARManager : MonoBehaviour
{
    #region Singleton

    public static ARManager Instance;

   

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    
    [SerializeField]
    private ARPlaneManager _planeManager;

    [Tooltip("Maximum plane size by m^2")]
    public float MaxPlaneSize;
    private float _maxPlaneSize => MaxPlaneSize;

    private float _currentPlaneSize;

    void TogglePlaneDetection(bool isOpen) 
    {
        _planeManager.enabled = isOpen;
    }

    public void BoundariesUpdated(float size)
    {
        _currentPlaneSize += size;
        if (HasSizeExceededMaxLimit) 
        {
            TogglePlaneDetection(false);
        }
    }

    public ARPlane TryGetBiggestARPlane() 
    {
        ARPlane currentPlane = null;
        foreach(var plane in _planeManager.trackables) 
        {
            if(currentPlane == null || plane.size.x * plane.size.y >= currentPlane.size.x * currentPlane.size.y)
                currentPlane = plane;
        }
        return currentPlane;
    }

    public List<Vector3> TryGetPlaneEdges(ARPlane plane) 
    {
        Vector3 pointAtTheEdge = Vector3.zero;

        if (plane.size.x > plane.size.y)
        {
            pointAtTheEdge = new  Vector3(plane.transform.position.x + plane.size.x / 2, plane.transform.position.y, plane.transform.position.z);
        }
        else
        {
            pointAtTheEdge = new  Vector3(plane.transform.position.x, plane.transform.position.y, plane.transform.position.z  + plane.size.y / 2);
        }

        var mesh = plane.GetComponent<MeshFilter>().sharedMesh;

        Vector3 closestVertex = Vector3.zero, furthestVertex = Vector3.zero;
        
        float closestVertexDistance = Int32.MaxValue;
        float furthestVertexDistance = Int32.MinValue;

        foreach (var vertex in mesh.vertices) 
        {
            if(Vector3.Distance(vertex + plane.transform.position,pointAtTheEdge) < closestVertexDistance) 
            {
                closestVertexDistance = Vector3.Distance(vertex + plane.transform.position, pointAtTheEdge);
                closestVertex = vertex;
            }
            if(Vector3.Distance(vertex + plane.transform.position,pointAtTheEdge) > furthestVertexDistance) 
            {
                furthestVertexDistance = Vector3.Distance(vertex + plane.transform.position, pointAtTheEdge);
                furthestVertex = vertex;
            }
        }

            return new List<Vector3> {closestVertex,furthestVertex};
    }

    private bool HasSizeExceededMaxLimit => _currentPlaneSize >= MaxPlaneSize;

}
