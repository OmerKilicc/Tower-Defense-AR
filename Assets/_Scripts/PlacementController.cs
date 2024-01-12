using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))] 
public class PlacementController : MonoBehaviour
{
    public static PlacementController Instance { get; private set; }

    private ARRaycastManager _arRaycastManager;


    #region DebuggingPurposes
    [SerializeField]
    private GameObject _objectToSpawn;

    [SerializeField]
    private TMP_Text _canvasObject;
    [SerializeField]
    public TMP_Text _locationDebug;

    public GameObject PointAtTheEdge;
    public GameObject PointAtTheClosest;
    public GameObject PointAtTheFurthest;
    public GameObject PointAtEveryVertex;

    public GameObject Scene;

    #endregion


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

        _arRaycastManager = GetComponent<ARRaycastManager>();
        ARManager.Instance.OnPlaneDetectionDone += OnPlaneDetectionDone;
    }

    private void OnPlaneDetectionDone()
    {
        SpawnHeadquarterAndSoldierSpawn();
    }

    static List<ARRaycastHit> hits= new List<ARRaycastHit>();

    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;
    }

    //PlacementController.TrySpawnObjectOnTouchPosition(touchPosition); 
    public static bool TrySpawnObjectOnTouchPosition(GameObject objectToCreate,Vector2 touchPosition)
    {
        if (Instance._arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            Instantiate(objectToCreate, hitPose.position, hitPose.rotation);
            return true;
        }
        return false;
    }


    bool TryGetTouchPosition(out Vector2 touchPosition) 
    {
        if(Input.touchCount  > 0) 
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }

    void SpawnHeadquarterAndSoldierSpawn() 
    {
        var plane = ARManager.Instance.TryGetBiggestARPlane();
      
        var spawns = ARManager.Instance.TryGetPlaneEdges(plane);
        var instantiated = Instantiate(_objectToSpawn, spawns[0],Quaternion.identity);
        var instantiated2 = Instantiate(_objectToSpawn, spawns[1],Quaternion.identity);

        Scene.transform.position = instantiated.transform.position;
        Scene.gameObject.SetActive(true);
        _canvasObject.gameObject.SetActive(true);
        _canvasObject.text = instantiated.transform.position + " " + instantiated2.transform.position; 
    }
}
