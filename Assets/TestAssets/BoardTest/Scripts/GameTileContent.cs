using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SelectionBase]
public class GameTileContent : MonoBehaviour
{
	// Tracks the type of its content
	[SerializeField]
	GameTileContentType _type = default;
	public GameTileContentType Type => _type;
    public bool BlocksPath =>
        Type == GameTileContentType.Wall || Type == GameTileContentType.Tower;


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

    public virtual void GameUpdate() { }
}
