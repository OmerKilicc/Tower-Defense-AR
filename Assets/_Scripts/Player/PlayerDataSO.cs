using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerDataSO")]

public class PlayerDataSO : ScriptableObject
{
	public int MaxHealth = 100;
	public int CurrentHealth = 100;

	public int StartingMoney = 250;
	public int CurrentMoney = 250;

	public int CurrentLevel = 0;
	
}
