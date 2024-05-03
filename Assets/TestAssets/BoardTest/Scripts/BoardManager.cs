using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	//TODO: Deploy this to Game Manager
	[SerializeField]
	Vector2Int _boardSize = new Vector2Int(11, 11);

	[SerializeField]
	GameBoard _board = default;

	[SerializeField]
	GameTileContentFactory _tileContentFactory = default;

	Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

	private void Awake()
	{
		_board.Initialize(_boardSize, _tileContentFactory);
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			HandleTouch();
		}
	}

	private void HandleTouch()
	{
		// Get a tile from the board, and if we got one
		// setting its content to destination, by getting one from the factory
		// it is now possible to turn any tile into a destination with a primary cursor click
		GameTile tile = _board.GetTile(TouchRay);
		if (tile != null)
		{
			_board.ToggleDestinations(tile);
		}
	}

	//Check if board is in logical size to spawn
	private void OnValidate()
	{
		if (_boardSize.x < 2)
			_boardSize.x = 2;

		if (_boardSize.y < 2)
			_boardSize.y = 2;
	}
}
