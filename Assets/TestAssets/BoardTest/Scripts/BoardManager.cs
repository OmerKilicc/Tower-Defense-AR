using System;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	//TODO: Deploy this to Game Manager
	[SerializeField]
	Vector2Int _boardSize = new Vector2Int(11, 11);

	[SerializeField]
	EnemyFactory _enemyFactory = default;

	[SerializeField, Range(0.1f, 10f)]
	float _spawnSpeed = 1f;

	[SerializeField]
	GameBoard _board = default;

	[SerializeField]
	GameTileContentFactory _tileContentFactory = default;

	Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

	float _spawnProgress;

    GameBehaviorCollection _enemies = new GameBehaviorCollection();
    GameBehaviorCollection _nonEnemies = new GameBehaviorCollection();

    TowerType selectedTowerType;

    [SerializeField]
    WarFactory warFactory = default;

    static BoardManager instance;

    public static Shell SpawnShell()
    {
        Shell shell = instance.warFactory.Shell;
        instance._nonEnemies.Add(shell);
        return shell;
    }

    void OnEnable()
    {
        instance = this;
    }

    private void Awake()
	{
		_board.Initialize(_boardSize, _tileContentFactory);
		_board.ShowGrid = true;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			HandleTouch();
		}
		else if (Input.GetMouseButtonDown(1))
		{
			HandleAlternativeTouch();
		}

		if (Input.GetKeyDown(KeyCode.V))
		{
			_board.ShowPaths = !_board.ShowPaths;
		}

		if (Input.GetKeyDown(KeyCode.G))
		{
			_board.ShowGrid = !_board.ShowGrid;
		}

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedTowerType = TowerType.Laser;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedTowerType = TowerType.Mortar;
        }


        _spawnProgress += _spawnSpeed * Time.deltaTime;
		while (_spawnProgress >= 1f)
		{
			_spawnProgress -= 1f;
			SpawnEnemy();
		}

		_enemies.GameUpdate();
        Physics.SyncTransforms();
        _board.GameUpdate();
        _nonEnemies.GameUpdate();
    }

	private void SpawnEnemy()
	{
		GameTile spawnPoint = _board.GetSpawnPoint(UnityEngine.Random.Range(0, _board.SpawnPointCount));
		NewEnemy enemy = _enemyFactory.Get();
		enemy.SpawnOn(spawnPoint);
		_enemies.Add(enemy);
	}

	private void HandleTouch()
	{
		GameTile tile = _board.GetTile(TouchRay);
		if (tile != null)
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				_board.ToggleTower(tile, selectedTowerType);
			}
			else
			{
				_board.ToggleWall(tile);
			}
		}
	}

	private void HandleAlternativeTouch()
	{
		// Get a tile from the board, and if we got one
		// setting its content to destination, by getting one from the factory
		// it is now possible to turn any tile into a destination with a primary cursor click
		GameTile tile = _board.GetTile(TouchRay);
		if (tile != null)
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				_board.ToggleDestination(tile);
			}
			else
			{
				_board.ToggleSpawnPoint(tile);
			}
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

    public static Explosion SpawnExplosion()
    {
        Explosion explosion = instance.warFactory.Explosion;
        instance._nonEnemies.Add(explosion);
        return explosion;
    }
}
