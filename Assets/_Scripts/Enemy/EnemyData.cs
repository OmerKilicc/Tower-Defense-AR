using UnityEngine;

public class EnemyData : MonoBehaviour
{
	MoneyHandler bank;

	// Health property
	[SerializeField] int health = 100;
	public int Health
	{
		get { return health; }
		set { health = Mathf.Max(0, value); }
	}

	// Reward property
	[SerializeField] int goldReward = 25;
	public int GoldReward
	{
		get { return goldReward; }
		set { goldReward = Mathf.Max(0, value); }
	}

	[SerializeField] int goldPenalty = 25;
	public int GoldPenalty
	{
		get { return goldPenalty; }
		set { goldPenalty = Mathf.Max(0, value); }
	}

	private void Start()
	{
		bank = FindObjectOfType<MoneyHandler>();
	}

	public void RewardGold()
	{
		if (bank == null) return;

		bank.Deposit(goldReward);
	}

	public void StealGold()
	{
		if (bank == null) return;

		bank.Withdraw(goldPenalty);
	}
}
