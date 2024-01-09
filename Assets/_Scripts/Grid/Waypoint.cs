using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
	[SerializeField] bool isPlacable;
	public bool IsPlacable
	{
		get { return isPlacable; }
		set { isPlacable = value; }
	}

	private void OnMouseDown()
	{
		if (isPlacable)
		{
			Debug.Log($"Placable at {transform.name}");
			//TODO: Place tower if money is enough for that tower and make isplacable false
		}
	}
}
