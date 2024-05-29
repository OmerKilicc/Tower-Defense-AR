using Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

	#region Singleton

	public static UIManager Instance;

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


	[SerializeField] TextMeshProUGUI _textMesh;
	[SerializeField] private GameObject _pauseCanvas;
	[SerializeField] private GameObject _gameOverCanvas;
	public TextMeshProUGUI DebugText;

	private void OnEnable()
	{
		BasicEnemy.OnMoneyChanged += HandleMoneyText;
	}
	private void OnDisable()
	{
		BasicEnemy.OnMoneyChanged -= HandleMoneyText;
	}

	private void HandleMoneyText(int currentMoney)
	{
		_textMesh.text = "Money: " + currentMoney.ToString();
	}

	public void DebugAText(string text)
	{
		DebugText.text = text;
	}

	public void HideMainMenu(GameObject canvasMainMenu)
	{
		canvasMainMenu.SetActive(false);
	}

	public void TogglePauseMenu(bool isOpen)
	{
		_pauseCanvas.SetActive(isOpen);
	}

	public void HandleGameOver()
	{
		_gameOverCanvas.SetActive(true);
	}

	public void TryAgainButton()
	{
		GameManager.Instance.RestartGame();
	}
}
