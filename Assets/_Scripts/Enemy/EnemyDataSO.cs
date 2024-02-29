using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
	public int MaxHealth = 100;
	public int CurrentHealth = 100;

	public int Damage = 10;

	public int Reward = 25;
	public int Penalty = -25;
}
