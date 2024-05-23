using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyFactory : GameObjectFactory
{
	[SerializeField, FloatRangeSlider(0.5f, 2f)]
	FloatRange _scale = new FloatRange(1f);

	[SerializeField, FloatRangeSlider(-0.4f, 0.4f)]
	FloatRange _pathOffset = new FloatRange(0f);

	[SerializeField, FloatRangeSlider(0.2f, 5f)]
	FloatRange _speed = new FloatRange(1f);

	[SerializeField]
	NewEnemy _prefab = default;

	public NewEnemy Get()
	{
		NewEnemy instance = CreateGameObjectInstance(_prefab);
		instance.OriginFactory = this;
		instance.Initialize(_scale.RandomValueInRange, _speed.RandomValueInRange, _pathOffset.RandomValueInRange);
		return instance;
	}
	public void Reclaim(NewEnemy enemy)
	{
		Debug.Assert(enemy.OriginFactory == this, "Wrong factory reclaimed!");
		Destroy(enemy.gameObject);
	}
}
