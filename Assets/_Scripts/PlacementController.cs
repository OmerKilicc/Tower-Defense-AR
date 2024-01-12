using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))] 
public class PlacementController : MonoBehaviour
{
    public static PlacementController Instance { get; private set; }

    private ARRaycastManager _arRaycastManager;

    [SerializeField]
    private GameObject _gameScene;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

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
        GameManager.OnGameStateChanged += OnGameStateChanged;
        ARManager.Instance.OnPlaneDetectionDone += OnPlaneDetectionDone;
    }

    private void OnGameStateChanged(GameManager.GameState state)
    {
        if(state == GameManager.GameState.PlaceGameScene) 
        {
            _canvasObject.text = "Subscribed";
            InputManager.Instance.OnTouchedScreen += ReplaceGameSceneWithTouchPosition;
        }
        else 
        {
            InputManager.Instance.OnTouchedScreen -= ReplaceGameSceneWithTouchPosition;
        }
    }

    private void OnDisable()
    {
        InputManager.Instance.OnTouchedScreen -= ReplaceGameSceneWithTouchPosition;
    }

    private void OnPlaneDetectionDone()
    {
        SpawnHeadquarterAndSoldierSpawn();
    }

    

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

    public void ReplaceGameSceneWithTouchPosition(Vector2 touchPosition) 
    {
        _canvasObject.text = "EventTriggered";
        if (Instance._arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            _canvasObject.text = "ReplacedScene";
            var hitPose = hits[0].pose;
            _gameScene.transform.position = hitPose.position;
            _gameScene.SetActive(true);
        }
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

        //_canvasObject.gameObject.SetActive(true);
        //_canvasObject.text = instantiated.transform.position + " " + instantiated2.transform.position; 
    }

}
