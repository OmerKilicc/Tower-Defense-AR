using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Linq;
using System;
using TMPro;

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
    private TMP_Text _sizeText;

    [SerializeField]
    private ARPlaneManager _planeManager;

    public Action OnPlaneDetectionDone;

    [Tooltip("Maximum plane size by m^2")]
    public float MaxPlaneSize;
    private float _maxPlaneSize => MaxPlaneSize;

    private float _currentPlaneSize;

    private bool _isDetectionDone = false;

    void TogglePlaneDetection(bool isOpen) 
    {
        _planeManager.enabled = isOpen;
    }

    public void BoundariesUpdated(float size)
    {
        _currentPlaneSize += size;
        _sizeText.text = _currentPlaneSize.ToString();
        if (HasSizeExceededMaxLimit && !_isDetectionDone) 
        {
            OnPlaneDetectionDone?.Invoke();
            TogglePlaneDetection(false);
            _isDetectionDone = true;
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
        Instantiate(PlacementController.Instance.PointAtTheEdge, pointAtTheEdge, Quaternion.identity);

        var mesh = plane.GetComponent<MeshFilter>().sharedMesh;

        Vector3 closestVertex = Vector3.zero, furthestVertex = Vector3.zero;
        
        float closestVertexDistance = Int32.MaxValue;
        float furthestVertexDistance = Int32.MinValue;

        foreach (var vertex in mesh.vertices) 
        {
            if(Vector3.Distance(vertex,pointAtTheEdge) < closestVertexDistance) 
            {
                closestVertexDistance = Vector3.Distance(vertex, pointAtTheEdge);
                closestVertex = vertex;
            }
            if(Vector3.Distance(vertex,pointAtTheEdge) > furthestVertexDistance) 
            {
                furthestVertexDistance = Vector3.Distance(vertex, pointAtTheEdge);
                furthestVertex = vertex;
            }
        }
        closestVertex = new Vector3(closestVertex.x,plane.transform.position.y,closestVertex.z);
        furthestVertex = new Vector3(furthestVertex.x,plane.transform.position.y,furthestVertex.z);

        PlacementController.Instance._locationDebug.text = "Closest: " + closestVertex + "Furthest: " + furthestVertex;
        Instantiate(PlacementController.Instance.PointAtTheClosest,closestVertex, Quaternion.identity);
        Instantiate(PlacementController.Instance.PointAtTheFurthest,furthestVertex, Quaternion.identity);

        return new List<Vector3> {closestVertex,furthestVertex};
    }

    private bool HasSizeExceededMaxLimit => _currentPlaneSize >= MaxPlaneSize;

}
