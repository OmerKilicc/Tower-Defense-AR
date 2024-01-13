using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{

	[SerializeField] private bool isPlacable;
	public bool IsPlacable
	{
		get { return isPlacable; }
		set { isPlacable = value; }
	}

}
