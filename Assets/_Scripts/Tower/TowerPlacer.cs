using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
	[SerializeField] int cost = 75;

	public bool CreateTower(GameObject tower, Vector3 position)
	{
		MoneyHandler bank = FindObjectOfType<MoneyHandler>();

		if (bank == null)
		{
			return false;
		}

		if (bank.CurrentBalance >= cost)
		{
			Instantiate(tower, position, Quaternion.identity);
			bank.Withdraw(cost);
			return true;
		}

		return false;
	}

}
