using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SpawnPoint : MonoBehaviour, ITrackableEventHandler
{

	public GameObject EnemyPrefab;
	public int MaxEnemies = 5;
	public float SpawnDelay = 5f;

	private HashSet<GameObject> _enemies;
	private float _delayTimer = 0f;

	private void Start () {
		_enemies = new HashSet<GameObject>();
	}

	private void Update()
	{
		if (_delayTimer <= 0 && _enemies.Count < MaxEnemies)
		{
			var enemy = Instantiate(EnemyPrefab);
			var x = Random.Range(-2.5f, 2.5f);
			var y = enemy.transform.position.y;
			var z = Random.Range(-2f, 2f);
			enemy.transform.position = new Vector3(x, y, z);
			enemy.transform.SetParent(transform);
			_enemies.Add(enemy);
			var controller = enemy.GetComponent<EnemyController>();
			controller.PlayerCamera = Camera.main.transform;
			controller.OnDead += () =>
			{
				if (_enemies.Count == MaxEnemies)
				{
                    _delayTimer = Random.Range(SpawnDelay - 1f, SpawnDelay + 1f);
				}
				_enemies.Remove(enemy);
			};
			_delayTimer = Random.Range(SpawnDelay - 1f, SpawnDelay + 1f);
		}

		_delayTimer = Mathf.Max(0f, _delayTimer - Time.deltaTime);
	}

	public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
	{
	}
}
