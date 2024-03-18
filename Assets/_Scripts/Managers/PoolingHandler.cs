using Enemy;
using UnityEngine;

// Example of usage in Unity
public class PoolingHandler : MonoBehaviour
{
	[SerializeField] TowerMainScript bulletTowerPrefab; // Reference to the bullet tower prefab
	[SerializeField] BasicEnemy basicEnemyPrefab; // Reference to the basic enemy prefab
	[SerializeField] Bullet bulletPrefab; // Reference to the bullet prefab

	private void Start()
	{
		// Register pools for BulletTower, BasicEnemy, and Bullet
		GameObjectFactory.RegisterPool<TowerMainScript>(10, bulletTowerPrefab);
		GameObjectFactory.RegisterPool<BasicEnemy>(10, basicEnemyPrefab);
		GameObjectFactory.RegisterPool<Bullet>(20, bulletPrefab);

		/*Examples
		 * 
		// Spawn some objects
		BulletTower tower1 = GameObjectFactory.CreateObject<BulletTower>();
		BasicEnemy enemy1 = GameObjectFactory.CreateObject<BasicEnemy>();
		Bullet bullet1 = GameObjectFactory.CreateObject<Bullet>();

		// Return objects to the pool when done
		GameObjectFactory.ReturnObject(tower1);
		GameObjectFactory.ReturnObject(enemy1);
		GameObjectFactory.ReturnObject(bullet1);
		*/
	}
}
