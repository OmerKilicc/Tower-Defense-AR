using Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementController : MonoBehaviour
{
	[SerializeField]
	PlayerDataSO _playerData = null;

	[SerializeField]
	int costOfTower = 100;

	public static PlacementController Instance { get; private set; }

	public ARRaycastManager _arRaycastManager;

	[SerializeField]
	Camera arCamera;

	[SerializeField]
	private GameObject _gameScene;

	public static List<ARRaycastHit> hits = new List<ARRaycastHit>();

	[SerializeField]
	private GameObject towerPrefab;

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
		if (state == GameManager.GameState.PlaceGameScene)
		{
			_canvasObject.text = "Subscribed";
			InputManager.Instance.OnTouchedScreen += ReplaceGameSceneWithTouchPosition;
		}
		else
		{
			InputManager.Instance.OnTouchedScreen -= ReplaceGameSceneWithTouchPosition;
		}

		if (state == GameManager.GameState.Playing)
		{
			InputManager.Instance.OnTouchedScreen += PlaceTowers;

		}
		else
		{
			InputManager.Instance.OnTouchedScreen -= PlaceTowers;

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

	/*
    //PlacementController.TrySpawnObjectOnTouchPosition(touchPosition); 
    public static bool TrySpawnObjectOnTouchPosition(GameObject objectToCreate,Vector2 touchPosition)
    {
        if (Instance._arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
			UIManager.Instance.DebugAText(touchPosition.ToString());
			//var hitPose = hits[0].pose;

			// En yakýn raycast hitini al
			ARRaycastHit hit = hits[0];
			Pose hitPose = hit.pose;

			// TrackableId ile hit edilen trackable objeyi al
			TrackableId trackableId = hit.trackableId;
			var trackable = Instance._arRaycastManager.trackables[trackableId];
			if (trackable != null)
			{
				GameObject hitObject = trackable.gameObject;

				// Hit edilen objeyi kontrol et
                // TODO: Para Checklemeyi unutma
				if (hitObject.CompareTag("Wall"))
				{
					Instantiate(objectToCreate, hitObject.transform.position, hitPose.rotation);
                    return true;
					
                    //GameTileContent tileContent = hitObject.GetComponent<GameTileContent>();
                    //if (tileContent.isTowerPlacable)
                   // {
						// Nesneyi yerleþtir
						Vector3 yOffset = new Vector3(0, 1f, 0);
						//tileContent.isTowerPlacable = false;
                        return true;
					//}
                    //return false;
                    
				}
				//Instantiate(objectToCreate, hitPose.position, hitPose.rotation);
			}
        }
        return false;
    }
    */
	private void TrySpawnObjectOnTouchPosition(GameObject objectToCreate, Vector2 touchPosition)
	{
		Ray ray = Instance.arCamera.ScreenPointToRay(touchPosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			UIManager.Instance.DebugAText("Raycast successful at position: " + touchPosition.ToString());
			GameObject hitObject = hit.collider.gameObject;
			// Hit edilen objeyi kontrol et
			if (hitObject.layer == LayerMask.NameToLayer("Wall") && _playerData.CurrentMoney >= costOfTower)
			{
				UIManager.Instance.DebugAText("Hit Wall.");
				GameTileContent content = hitObject.GetComponent<GameTileContent>();
				Vector3 offset = new Vector3(0, .07f, 0);

				// Nesneyi yerleþtir
				Instantiate(objectToCreate, hit.collider.gameObject.transform.position - offset, Quaternion.identity);
				_playerData.CurrentMoney -= costOfTower;
				UIManager.Instance.DebugAText("Tower placed at: " + hit.point.ToString());
			}
			else
			{
				UIManager.Instance.DebugAText("Hit object is not a Wall.");
			}
		}
		else
		{
			UIManager.Instance.DebugAText("Raycast failed.");
		}

	}

	private void PlaceTowers(Vector2 position, RaycastHit? hitinfo)
	{
		TrySpawnObjectOnTouchPosition(towerPrefab, position);
	}

	public void ReplaceGameSceneWithTouchPosition(Vector2 touchPosition, RaycastHit? hitInfo)
	{
		_canvasObject.text = "EventTriggered";
		if (Instance._arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
		{
			_canvasObject.text = "ReplacedScene";
			var hitPose = hits[0].pose;
			var positionOffset = new Vector3(0, 0.01f, 0);
			_gameScene.transform.position = hitPose.position + positionOffset;
			_gameScene.SetActive(true);
			GameManager.Instance.UpdateGameState(GameManager.GameState.Playing);
		}
	}

	bool TryGetTouchPosition(out Vector2 touchPosition)
	{
		if (Input.touchCount > 0)
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
		var instantiated = Instantiate(_objectToSpawn, spawns[0], Quaternion.identity);
		var instantiated2 = Instantiate(_objectToSpawn, spawns[1], Quaternion.identity);

		//_canvasObject.gameObject.SetActive(true);
		//_canvasObject.text = instantiated.transform.position + " " + instantiated2.transform.position; 
	}

}
