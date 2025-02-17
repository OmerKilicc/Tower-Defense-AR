
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
	[SerializeField]
	Texture2D _gridTexture;

	[SerializeField]
	Transform _ground = default;

	[SerializeField]
	GameTile _tilePrefab = default;

	GameTile[] _tiles;

	int[,] Map = {
	{0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0},
	{0, 2, 2, 2, 2, 0, 0, 2, 2, 2, 0},
	{0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0},
	{0, 2, 0, 2, 2, 2, 2, 2, 0, 2, 2},
	{0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0},
	{0, 2, 2, 2, 2, 2, 0, 2, 2, 2, 0},
	{0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0},
	{0, 2, 0, 2, 2, 2, 2, 2, 0, 2, 2},
	{0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0},
	{0, 2, 2, 2, 2, 2, 0, 2, 2, 2, 0},
	{0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0}
	};

	Queue<GameTile> _searchFrontier = new Queue<GameTile>();
	List<GameTile> _spawnPoints = new List<GameTile>();

	GameTileContentFactory _contentFactory;

	List<GameTileContent> updatingContent = new List<GameTileContent>();

	Vector2Int _size;
	public int SpawnPointCount => _spawnPoints.Count;

	bool _showPaths;
	public bool ShowPaths
	{
		get => _showPaths;
		set
		{
			_showPaths = value;
			if (_showPaths)
			{
				foreach (GameTile tile in _tiles)
				{
					tile.ShowPath();
				}
			}
			else
			{
				foreach (GameTile tile in _tiles)
				{
					tile.HidePath();
				}
			}
		}
	}

	bool _showGrid;
	public bool ShowGrid
	{
		get => _showGrid;
		set
		{
			_showGrid = value;
			Material m = _ground.GetComponent<MeshRenderer>().material;
			if (_showGrid)
			{
				m.mainTexture = _gridTexture;
				m.SetTextureScale("_MainTex", _size);
			}
			else
			{
				m.mainTexture = null;
			}
		}
	}
	public void Initialize(Vector2Int size, GameTileContentFactory contentFactory)
	{
		//Size given by player will be applied to this object with localScale func
		this._size = size;
		this._contentFactory = contentFactory;
		//_ground.localScale = new Vector3(size.x, size.y, 1f);

		/*
		 * We will instantiate tiles with a double loop over two dimensions of the grid.
		 * We place the tiles in the XZ plane, like the board.
		 */

		// As the board is centered on the origin, we have to subtract,
		// relevant size minus one, divided by two, from the tile positon's components.
		Vector2 offset = new Vector2(
			(size.x - 1) * 0.5f, (size.y - 1) * 0.5f
		);

		_tiles = new GameTile[size.x * size.y];
		for (int i = 0, y = 0; y < _size.y; y++)
		{
			for (int x = 0; x < _size.x; x++, i++)
			{
				GameTile tile = _tiles[i] = Instantiate(_tilePrefab);
				tile.transform.SetParent(transform, false);
				tile.transform.localPosition = new Vector3(x - offset.x, 0f, y - offset.y);
				//Tile Scale 
				//if not in 0 can create neighbor bonds
				if (x > 0) { GameTile.MakeEastWestNeighbors(tile, _tiles[i - 1]); }
				if (y > 0) { GameTile.MakeNorthSouthNeighbors(tile, _tiles[i - size.x]); }

				tile.IsAlternative = (x & 1) == 0;
				if ((y & 1) == 0)
					tile.IsAlternative = !tile.IsAlternative;

				// Give all tiles an empty content instance.
				tile.Content = _contentFactory.Get(GetTileType(Map[size.y - y - 1,x]));
				tile.Content.transform.SetParent(tile.transform);
				tile.Content.transform.localPosition = Vector3.zero;
			}
		}
		transform.localScale = new Vector3(.1f, .1f, .1f);

		ToggleSpawnPoint(_tiles[0]);
		ToggleSpawnPoint(_tiles[116]);



	

		ToggleDestination(_tiles[_tiles.Length - 1]);
	}


	private GameTileContentType GetTileType(int idx) 
	{
		GameTileContentType type;
		switch (idx)
		{
			case 0:
				return GameTileContentType.Empty;
			case 1:
				return GameTileContentType.Destination;
			case 2:
				return GameTileContentType.Wall;
			case 3:
				return GameTileContentType.SpawnPoint;
			case 4:
				return GameTileContentType.Tower;

		}
		return GameTileContentType.Empty;
	}

	private bool FindPaths()
	{
		// Reset all the paths found
		foreach (GameTile tile in _tiles)
		{
			if (tile.Content.Type == GameTileContentType.Destination)
			{
				tile.BecomeDestination();
				_searchFrontier.Enqueue(tile);
			}
			else
			{
				tile.ClearPath();
			}
		}

		if (_searchFrontier.Count == 0)
		{
			return false;
		}


		// For all the frontier tiles, search neighbors for path, make them frontiers.
		while (_searchFrontier.Count > 0)
		{
			GameTile tile = _searchFrontier.Dequeue();
			if (tile != null)
			{
				if (tile.IsAlternative)
				{
					_searchFrontier.Enqueue(tile.GrowPathNorth());
					_searchFrontier.Enqueue(tile.GrowPathSouth());

					_searchFrontier.Enqueue(tile.GrowPathEast());
					_searchFrontier.Enqueue(tile.GrowPathWest());
				}
				else
				{
					_searchFrontier.Enqueue(tile.GrowPathWest());
					_searchFrontier.Enqueue(tile.GrowPathEast());

					_searchFrontier.Enqueue(tile.GrowPathSouth());
					_searchFrontier.Enqueue(tile.GrowPathNorth());
				}

			}
		}

		foreach (GameTile tile in _tiles)
		{
			if (!tile.HasPath)
			{
				return false;
			}
		}

		if (_showPaths)
		{
			foreach (GameTile tile in _tiles)
			{
				tile.ShowPath();
			}
		}

		return true;
	}

	// Casts a ray gets the info checks if the tile is in bounds of
	// board, if so returns it
	public GameTile GetTile(Ray ray)
	{

		if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, 1))
		{
			int x = (int)(hit.point.x + _size.x * 0.5f);
			int y = (int)(hit.point.z + _size.y * 0.5f);
			if (x >= 0 && x < _size.x && y >= 0 && y < _size.y)
			{
				return _tiles[x + y * _size.x];
			}
		}

		return null;
	}
	public GameTile GetTile(UnityEngine.XR.ARFoundation.ARRaycastHit hit)
	{

		
		
			int x = (int)(hit.pose.position.x + _size.x * 0.5f);
			int y = (int)(hit.pose.position.z + _size.y * 0.5f);
			if (x >= 0 && x < _size.x && y >= 0 && y < _size.y)
			{
				return _tiles[x + y * _size.x];
			}
		

		return null;
	}

	// Make it destination if its empty, make it empty if its destination
	// FindPaths again in both occasions
	public void ToggleDestination(GameTile tile)
	{
		if (tile.Content.Type == GameTileContentType.Destination)
		{
			tile.Content = _contentFactory.Get(GameTileContentType.Empty);

			// If its the only destination than make it untoggleable
			if (!FindPaths())
			{
				tile.Content = _contentFactory.Get(GameTileContentType.Destination);
				tile.Content.transform.SetParent(tile.transform);
				tile.Content.transform.localPosition = Vector3.zero;
				FindPaths();
			}
		}
		else if (tile.Content.Type == GameTileContentType.Empty)
		{
			tile.Content = _contentFactory.Get(GameTileContentType.Destination);
			tile.Content.transform.SetParent(tile.transform);
			tile.Content.transform.localPosition = Vector3.zero;
			FindPaths();
		}
	}

	//Toggle between wall and empty
	//public void ToggleWall(GameTile tile)
	//{
	//	if (tile.Content.Type == GameTileContentType.Wall)
	//	{
	//		tile.Content = _contentFactory.Get(GameTileContentType.Empty);
	//		FindPaths();
	//	}
	//	else if (tile.Content.Type == GameTileContentType.Empty)
	//	{
	//		tile.Content = _contentFactory.Get(GameTileContentType.Wall);
	//		if (!FindPaths())
	//		{
	//			tile.Content = _contentFactory.Get(GameTileContentType.Empty);
	//			FindPaths();

	//		}
	//	}
	//}

	public void ToggleTower(GameTile tile, TowerType towerType)
	{
		if (tile.Content.Type == GameTileContentType.Tower)
		{
			updatingContent.Remove(tile.Content);
			if (((Tower)tile.Content).TowerType == towerType)
			{
				tile.Content = _contentFactory.Get(GameTileContentType.Empty);
				tile.Content.transform.SetParent(tile.transform); tile.Content.transform.localPosition = Vector3.zero;
			}
			else
			{
				tile.Content = _contentFactory.Get(towerType);
				tile.Content.transform.SetParent(tile.transform); tile.Content.transform.localPosition = Vector3.zero;

				updatingContent.Add(tile.Content);
			}
			FindPaths();
		}
		else if (tile.Content.Type == GameTileContentType.Empty)
		{
			tile.Content = _contentFactory.Get(towerType);
			if (FindPaths())
			{
				updatingContent.Add(tile.Content);
			}
			else
			{
				tile.Content = _contentFactory.Get(GameTileContentType.Empty);
				FindPaths();
			}
		}
		else if (tile.Content.Type == GameTileContentType.Wall)
		{
			tile.Content = _contentFactory.Get(towerType);
			updatingContent.Add(tile.Content);
		}
	}

	// Does not affect the pathfinding so we do not need to find paths again
	public void ToggleSpawnPoint(GameTile tile)
	{
		if (tile.Content.Type == GameTileContentType.SpawnPoint)
		{
			if (_spawnPoints.Count > 1)
			{
				_spawnPoints.Remove(tile);
				tile.Content = _contentFactory.Get(GameTileContentType.Empty);
				tile.Content.transform.SetParent(tile.transform);
				tile.Content.transform.localPosition = Vector3.zero;
			}
		}
		else if (tile.Content.Type == GameTileContentType.Empty)
		{
			tile.Content = _contentFactory.Get(GameTileContentType.SpawnPoint);
			tile.Content.transform.SetParent(tile.transform);
			tile.Content.transform.localPosition = Vector3.zero;
			_spawnPoints.Add(tile);
			
		}
	}

	public GameTile GetSpawnPoint(int index)
	{
		return _spawnPoints[index];
	}

	public void GameUpdate()
	{
		for (int i = 0; i < updatingContent.Count; i++)
		{
			updatingContent[i].GameUpdate();
		}
	}
}

