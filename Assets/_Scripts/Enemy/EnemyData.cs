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
			health = Mathf.Max(0, value); // Sa�l�k de�erini s�f�r�n alt�na d���rmemek i�in
		}
	}

	// Reward property
	public int Reward
	{
		get { return reward; }
		set
		{
			reward = Mathf.Max(0, value); // �d�l de�erini s�f�r�n alt�na d���rmemek i�in
		}
	}

}
