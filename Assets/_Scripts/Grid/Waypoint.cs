using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
	[SerializeField] GameObject towerPrefab;
	[SerializeField] bool isPlacable;
	public bool IsPlacable
	{
		get { return isPlacable; }
		set { isPlacable = value; }
	}

	private void OnMouseDown()
	{
		if (isPlacable)
		{
			TowerPlacer towerPlacer = towerPrefab.GetComponent<TowerPlacer>();
			bool isPlaced = towerPlacer.CreateTower(towerPrefab, transform.position);
			IsPlacable = !isPlaced;
		}
	}
}
