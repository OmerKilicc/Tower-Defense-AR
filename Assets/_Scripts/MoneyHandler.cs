using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoneyHandler : MonoBehaviour
{
	[SerializeField] int startingBalance = 150;
	[SerializeField] int currentBalance;

	[SerializeField] TextMeshProUGUI displayBalance;

	public int CurrentBalance { get { return currentBalance; } }

	void Awake()
	{
		currentBalance = startingBalance;
		UpdateDisplay();
	}

	public void Deposit(int amount)
	{
		currentBalance += Mathf.Abs(amount);
		UpdateDisplay();
	}

	public void Withdraw(int amount)
	{
		currentBalance -= Mathf.Abs(amount);
		UpdateDisplay();

		if (currentBalance < 0)
		{
			//Lose the game;
			ReloadScene();
		}

	}
	void UpdateDisplay()
	{
		displayBalance.text = "Gold: " + currentBalance;
	}

	void ReloadScene()
	{
		Scene currentScene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(currentScene.buildIndex);
	}


}
