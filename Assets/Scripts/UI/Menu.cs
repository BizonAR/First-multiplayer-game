using UnityEngine;

public class Menu : MonoBehaviour
{
	[SerializeField] private GameObject[] _menus;

	public void SetScreen(Screens screen)
	{
		for (int i = 0; i < _menus.Length; i++) 
		{
			_menus[i].SetActive(i == (int)screen);
		}
	}

	public enum Screens
	{
		Connect = 0,
		Wait,
		Rooms
	}
}
