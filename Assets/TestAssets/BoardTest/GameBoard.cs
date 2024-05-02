
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
	[SerializeField]
	Transform _ground = default;

	[SerializeField]
	GameTile _tilePrefab = default;

	Vector2Int _size;

	Queue<GameTile> _searchFrontier = new Queue<GameTile>();

	// keep track of the tiles that have been initilized
	GameTile[] _tiles;

	public void Initialize(Vector2Int size)
	{
		//Size given by player will be applied to this object with localScale func
		this._size = size;
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

			}
		}

		FindPaths();
	}

	private void FindPaths()
	{
		// Reset all the paths found
		foreach (GameTile tile in _tiles)
		{
			tile.ClearPath();
		}

		// Start adding to queue with destination tile
		_tiles[_tiles.Length / 2].BecomeDestination();
		_searchFrontier.Enqueue(_tiles[_tiles.Length / 2]);

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
			tile.ShowPath();
		}
	}
}
