
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

	Queue<GameTile> _searchFrontier = new Queue<GameTile>();
	List<GameTile> _spawnPoints = new List<GameTile>();

	GameTileContentFactory _contentFactory;

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
		_ground.localScale = new Vector3(size.x, size.y, 1f);

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

				//if not in 0 can create neighbor bonds
				if (x > 0) { GameTile.MakeEastWestNeighbors(tile, _tiles[i - 1]); }
				if (y > 0) { GameTile.MakeNorthSouthNeighbors(tile, _tiles[i - size.x]); }

				tile.IsAlternative = (x & 1) == 0;
				if ((y & 1) == 0)
					tile.IsAlternative = !tile.IsAlternative;

				// Give all tiles an empty content instance.
				tile.Content = _contentFactory.Get(GameTileContentType.Empty);

			}
		}

		ToggleDestination(_tiles[_tiles.Length / 2]);
		ToggleSpawnPoint(_tiles[0]);
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

		if (Physics.Raycast(ray, out RaycastHit hit))
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
				FindPaths();
			}
		}
		else if (tile.Content.Type == GameTileContentType.Empty)
		{
			tile.Content = _contentFactory.Get(GameTileContentType.Destination);
			FindPaths();
		}
	}

	//Toggle between wall and empty
	public void ToggleWall(GameTile tile)
	{
		if (tile.Content.Type == GameTileContentType.Wall)
		{
			tile.Content = _contentFactory.Get(GameTileContentType.Empty);
			FindPaths();
		}
		else if (tile.Content.Type == GameTileContentType.Empty)
		{
			tile.Content = _contentFactory.Get(GameTileContentType.Wall);
			if (!FindPaths())
			{
				tile.Content = _contentFactory.Get(GameTileContentType.Empty);
				FindPaths();

			}
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
			}
		}
		else if (tile.Content.Type == GameTileContentType.Empty)
		{
			tile.Content = _contentFactory.Get(GameTileContentType.SpawnPoint);
			_spawnPoints.Add(tile);
		}
	}

	public GameTile GetSpawnPoint(int index)
	{
		return _spawnPoints[index];
	}

}

