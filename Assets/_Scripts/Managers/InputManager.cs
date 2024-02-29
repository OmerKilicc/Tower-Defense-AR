using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	#region Singleton

	public static InputManager Instance;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(Instance);
		}
		else
		{
			Instance = this;
		}
	}
	#endregion

	public Action<Vector2,RaycastHit?> OnTouchedScreen; //Subscribe to this Action to instantiate an object with touching on screen etc.

	// Update is called once per frame
	void Update()
	{
		if (!TryGetTouch())
			return;
	}

	bool TryGetTouch()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			// Ekran koordinatýndan ray oluþtur
			Ray ray = Camera.main.ScreenPointToRay(touch.position);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit)) 
			{
				OnTouchedScreen?.Invoke(touch.position, hit);
			}
			else
			{
				OnTouchedScreen?.Invoke(touch.position,null);
			}
			return true;
		}
		else { return false; }
	}

}
