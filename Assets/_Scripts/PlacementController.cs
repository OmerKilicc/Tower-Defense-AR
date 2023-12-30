using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))] 
public class PlacementController : MonoBehaviour
{
    public static PlacementController Instance { get; private set; }

    private ARRaycastManager _arRaycastManager;

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
}
