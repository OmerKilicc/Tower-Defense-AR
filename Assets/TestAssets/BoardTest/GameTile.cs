using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;

public class GameTile : MonoBehaviour
{
	[SerializeField]
	Transform _arrow = default;

	//Directions of the tiles
	GameTile north, east, south, west;

	// Let's have 2 squares, let's call the left one the first and the right one the second.
	// If the first square is to the east of the second, it becomes the second, and if the second square is to the west of the first, it becomes the first
	// So they become east-west neighbors, and we're handling this here.
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

	//We are going to establish theese relationships in GameBoard


