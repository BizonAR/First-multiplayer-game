using UnityEngine;

public class HealthPresenter : MonoBehaviour
{
	private Damagable _damagable;
	private RectTransform _healthBar;
	private float _maxSize;

	public void Connect(Damagable damagable)
	{
		_healthBar = transform as RectTransform;
		damagable = _damagable;
		_maxSize = _healthBar.sizeDelta.x;
	}

	private void Update()
	{
		if (_damagable != null) 
		{
			_healthBar.sizeDelta = new Vector2((_maxSize * _damagable.Health) / 100f, _healthBar.sizeDelta.y);
			Debug.Log("Изменить здоровье");
		}
	}
}
