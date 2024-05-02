using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;

public class GameTile : MonoBehaviour
{
	[SerializeField]
	Transform _arrow = default;

	//Directions of the tiles and next tile
	GameTile north, east, south, west, nextOnPath;

	static Quaternion
		northRotation = Quaternion.Euler(90f, 0f, 0f),
		eastRotation = Quaternion.Euler(90f, 90f, 0f),
		southRotation = Quaternion.Euler(90f, 180f, 0f),
		westRotation = Quaternion.Euler(90f, 270f, 0f);

	// distance to finish
	int distance;

	// Let's have 2 squares, let's call the left one the first and the right one the second.
	// If the first square is to the east of the second, it becomes the second, and if the second square is to the west of the first, it becomes the first
	// So they become east-west neighbors, and we're handling this here.
	//We are going to establish theese relationships in GameBoard

	public static void MakeEastWestNeighbors(GameTile east, GameTile west)
	{
		Debug.Assert(west.east == null && east.west == null, "Redefined neighbors!");

		west.east = east;
		east.west = west;
	}

	public static void MakeNorthSouthNeighbors(GameTile north, GameTile south)
	{
		Debug.Assert(south.north == null && north.south == null, "Redefined neighbors!");

		south.north = north;
		north.south = south;
	}


	public void ClearPath()
	{
		distance = int.MaxValue;
		nextOnPath = null;
	}

	public void BecomeDestination()
	{
		distance = 0;
		nextOnPath = null;
	}

	public bool HasPath => distance != int.MaxValue;

	// Starts from the destination goes to 4 neighbors if available
	// for that 4 neighbors aborts if there are paths in them and goes to
	// others without path and +1 their distance and make them point to itself
	// will return the new tile for path
	GameTile GrowPathTo(GameTile neighbor)
	{
		Debug.Assert(HasPath, "No path!");

		if (neighbor == null || neighbor.HasPath)
			return null;

		neighbor.distance = distance + 1;
		neighbor.nextOnPath = this;
		return neighbor;
	}

	public GameTile GrowPathNorth() => GrowPathTo(north);
	public GameTile GrowPathEast() => GrowPathTo(east);
	public GameTile GrowPathSouth() => GrowPathTo(south);
	public GameTile GrowPathWest() => GrowPathTo(west);

	public void ShowPath()
	{
		if(distance == 0)
		{
			_arrow.gameObject.SetActive(false);
			return;
		}

		_arrow.gameObject.SetActive(true);
		_arrow.localRotation =
			nextOnPath == north ? northRotation :
			nextOnPath == east ? eastRotation :
			nextOnPath == south ? southRotation :
			westRotation;
			
	}

	public bool IsAlternative { get; set; }

}

