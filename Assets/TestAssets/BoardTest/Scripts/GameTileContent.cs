using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTileContent : MonoBehaviour
{
	// Tracks the type of its content
	[SerializeField]
	GameTileContentType _type = default;
	public GameTileContentType Type => _type;

	// Tracks the factory it came from
	GameTileContentFactory _originFactory;
	public GameTileContentFactory OriginFactory
	{
		get => _originFactory;
		set
		{
			Debug.Assert(_originFactory == null, "Redefined origin factory!");
			_originFactory = value;
		}
	}

	public void Recycle()
	{
		_originFactory.Reclaim(this);
	}
}
