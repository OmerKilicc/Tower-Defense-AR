using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	//TODO: Deploy this to Game Manager
    [SerializeField]
    Vector2Int _boardSize = new Vector2Int(11,11);

    [SerializeField]
    GameBoard _board = default;

	private void Awake()
	{
        _board.Initialize(_boardSize);
	}


	//Check if board is in logical size to spawn
	private void OnValidate()
	{
		if(_boardSize.x < 2)
			_boardSize.x = 2;

		if(_boardSize.y < 2)
			_boardSize.y = 2;
	}
}
