using System.Collections.Generic;

[System.Serializable]
public class EnemyCollection
{
	List<NewEnemy> _enemies = new List<NewEnemy>();

	public void Add(NewEnemy enemy)
	{
		_enemies.Add(enemy);
	}

	public void GameUpdate()
	{
		for (int i = 0; i < _enemies.Count; i++)
		{
			// If the choosen enemy is dead
			if (!_enemies[i].GameUpdate())
			{
				// make it the last index of the enemy on the list 
				// then remove it
				int lastIndex = _enemies.Count - 1;
				_enemies[i] = _enemies[lastIndex];
				_enemies.RemoveAt(lastIndex);
				i -= 1;
			}
		}
	}
}