using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField] private SpawnableObject _spawnableObject;
	[SerializeField] private float _pushDelay = 0.3f;

	private Queue<SpawnableObject> _spawnQueue = new Queue<SpawnableObject>();
	private Transform _transform;

	private void Awake()
	{
		_transform = transform;
	}

	public void Pull(Vector3 start, Vector3 end)
	{
		if (_spawnQueue.Count == 0)
		{
			Instantiate(_spawnableObject, Vector3.zero, Quaternion.identity).Initialize(this).Push();
		}

		var spawned = _spawnQueue.Dequeue();
		spawned.Pull(start, end).PushDelayed(_pushDelay);
	}

	internal void Push(SpawnableObject spawnableObject) 
	{
		spawnableObject.SetActive(false);
		_spawnQueue.Enqueue(spawnableObject);
	}
}
