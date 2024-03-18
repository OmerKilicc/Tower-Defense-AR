//using System.Reflection.Emit;
//using TMPro;
//using UnityEngine;

//[ExecuteAlways]
//public class CoordinateLabeler : MonoBehaviour
//{
//	[SerializeField] Color defaultLabelColor = Color.white;
//	[SerializeField] Color blockedLabelColor = Color.red;

//	TextMeshPro _gridLabel;
//	Vector2Int _coordinates = new Vector2Int();
//	Waypoint _waypoint;

//	void Awake()
//	{
//		_gridLabel = GetComponent<TextMeshPro>();
//		_gridLabel.enabled = false;

//		_waypoint = GetComponentInParent<Waypoint>();
//		DisplayCoordinates();
//	}

//	void Update()
//	{
//		if (!Application.isPlaying)
//		{
//			DisplayCoordinates();
//			UpdateObjectName();
//		}

//		ColorCoordinates();
//		ToggleLabels();
//	}

//	//Debug tool to hide and make visible the labels
//	void ToggleLabels() 
//	{
//		if(Input.GetKeyDown(KeyCode.C)) 
//		{
//			_gridLabel.enabled = !_gridLabel.IsActive();
//		}
//	}
//	void ColorCoordinates()
//	{
//		if (_waypoint.IsPlacable)
//			_gridLabel.color = defaultLabelColor;
//		else
//			_gridLabel.color = blockedLabelColor;
//	}

//	void DisplayCoordinates()
//	{
//		var snapSettingsX = UnityEditor.EditorSnapSettings.move.x;
//		var snapSettingsY = UnityEditor.EditorSnapSettings.move.z;

//		_coordinates.x = Mathf.RoundToInt(transform.parent.position.x / snapSettingsX);
//		_coordinates.y = Mathf.RoundToInt(transform.parent.position.z / snapSettingsY);

//		_gridLabel.text = $"{_coordinates.x},{_coordinates.y}";

//		UpdateObjectName();
//	}

//	void UpdateObjectName()
//	{
//		transform.parent.name = _coordinates.ToString();
//	}
//}
