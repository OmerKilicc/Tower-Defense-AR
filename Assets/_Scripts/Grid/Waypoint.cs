using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
	[SerializeField] GameObject towerPrefab;
	[SerializeField] bool isPlacable;

	private void OnEnable()
	{
		InputManager.Instance.OnTouchedScreen += HandleTowerPlacement;
	}

	private void OnDisable()
	{
		InputManager.Instance.OnTouchedScreen -= HandleTowerPlacement;
	}

	public bool IsPlacable
	{
		get { return isPlacable; }
		set { isPlacable = value; }
	}

	private void HandleTowerPlacement(Vector2 vector)
	{
		if (isPlacable)
		{
			TowerPlacer towerPlacer = towerPrefab.GetComponent<TowerPlacer>();
			bool isPlaced = towerPlacer.CreateTower(towerPrefab, transform.position);
			IsPlacable = !isPlaced;
		}
	}
}
