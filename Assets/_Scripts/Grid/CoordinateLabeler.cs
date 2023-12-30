using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class CoordinateLabeler : MonoBehaviour
{
    TextMeshPro _gridLabel;
    Vector2Int _coordinates = new Vector2Int();

    void Awake()
    {
        _gridLabel = GetComponent<TextMeshPro>();
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
        }
    }

    void DisplayCoordinates() 
    {
        var snapSettingsX = UnityEditor.EditorSnapSettings.move.x;
        var snapSettingsY = UnityEditor.EditorSnapSettings.move.z;

		_coordinates.x = Mathf.RoundToInt(transform.parent.position.x / snapSettingsX);
        _coordinates.y = Mathf.RoundToInt(transform.parent.position.z / snapSettingsY);

        _gridLabel.text = $"{_coordinates.x},{_coordinates.y}";

        UpdateObjectName();
    }

    void UpdateObjectName()
    {
        transform.parent.name = _coordinates.ToString();
    }
}
