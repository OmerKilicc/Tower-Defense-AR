using UnityEngine;

public class EnemyData : MonoBehaviour
{
	[SerializeField] private int health = 100;
	[SerializeField] private int reward = 50;

	// Health property
	public int Health
	{
		get { return health; }
		set
		{
			health = Mathf.Max(0, value); // Saðlýk deðerini sýfýrýn altýna düþürmemek için
		}
	}

	// Reward property
	public int Reward
	{
		get { return reward; }
		set
		{
			reward = Mathf.Max(0, value); // Ödül deðerini sýfýrýn altýna düþürmemek için
		}
	}

}
