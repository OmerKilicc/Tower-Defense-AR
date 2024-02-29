using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerPlacer : MonoBehaviour
{
	[SerializeField] GameObject towerPrefab;
	Waypoint waypoint;

	[SerializeField] PlayerDataSO _playerData;
	[SerializeField] int cost = 75;
	bool isGamePlaying = false;

	private void OnEnable()
	{
		GameManager.OnGameStateChanged += IsGamePlaying;
		InputManager.Instance.OnTouchedScreen += CheckCreateTower;
	}

	private void OnDisable()
	{
		GameManager.OnGameStateChanged -= IsGamePlaying;
		InputManager.Instance.OnTouchedScreen -= CheckCreateTower;
	}

	private void Start()
	{
		waypoint = GetComponent<Waypoint>();
	}

	private void IsGamePlaying(GameManager.GameState obj)
	{
		if (obj == GameManager.GameState.Playing) { isGamePlaying = true; }
		else { isGamePlaying = false; }
	}

	private bool DoesHaveEnoughMoney()
	{
		if (_playerData.CurrentMoney >= cost)
			return true;
		else
			return false;
	}

	void CheckCreateTower(Vector2 TouchPosition,RaycastHit? hit)
	{
		GameObject touchedObject = hit?.transform.gameObject;

		if (!isGamePlaying || touchedObject != gameObject || !waypoint.IsPlacable || !DoesHaveEnoughMoney())
			return;

		Instantiate(towerPrefab, transform.position, Quaternion.identity);
		_playerData.CurrentMoney -= cost;
		waypoint.IsPlacable = false;
		
	}

}
