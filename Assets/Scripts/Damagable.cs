using UnityEngine;

public abstract class Damagable : MonoBehaviour
{
	internal float _health;

	public float Health => _health;

	public void TakeDamage(float damage)
	{
		_health -= damage;

		if (_health <= 0)
		{
			_health = 0;
			OnDie();
		}
	}

	public abstract void OnDie();
}
