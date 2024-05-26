using UnityEngine;

[CreateAssetMenu]
public class GameTileContentFactory : GameObjectFactory
{
	[SerializeField]
	GameTileContent _spawnPointPrefab;

	[SerializeField]
	GameTileContent _destinationPrefab;

	[SerializeField]
	GameTileContent _emptyPrefab;

	[SerializeField]
	GameTileContent _wallPrefab;

	[SerializeField]
	Tower[] towerPrefabs = default;

	public void Reclaim(GameTileContent content)
	{
		Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed!");
		Destroy(content.gameObject);
	}


	// kendisine verilen prefab türünde contenti spawn eder kendini origin olarak verir
	// factory sahnesine taşır ve return eder
	GameTileContent Get(GameTileContent prefab)
	{
		GameTileContent instance = CreateGameObjectInstance(prefab);
		instance.OriginFactory = this;
		return instance;
	}

	public GameTileContent Get(GameTileContentType type)
	{
		switch (type)
		{
			case GameTileContentType.Destination: return Get(_destinationPrefab);
			case GameTileContentType.Empty: return Get(_emptyPrefab);
			case GameTileContentType.Wall: return Get(_wallPrefab);
			case GameTileContentType.SpawnPoint: return Get(_spawnPointPrefab);
		}

		Debug.Assert(false, "Unsupported type: " + type);
		return null;
	}

	public Tower Get(TowerType type)
	{
		Debug.Assert((int)type < towerPrefabs.Length, "Unsupported tower type!");
		Tower prefab = towerPrefabs[(int)type];
		Debug.Assert(type == prefab.TowerType, "Tower prefab at wrong index!");
		return Get(prefab);
	}

	T Get<T>(T prefab) where T : GameTileContent
	{
		T instance = CreateGameObjectInstance(prefab);
		instance.OriginFactory = this;
		return instance;
	}
}
