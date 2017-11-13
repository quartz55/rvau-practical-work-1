using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SpawnPoint : MonoBehaviour, ITrackableEventHandler
{
	public ScoreManager Score;
	public GameObject EnemyPrefab;
	public int MaxEnemies = 10;
	public float SpawnDelay = 3f;

	private HashSet<GameObject> _enemies;
	private float _delayTimer;
	private bool _engaged;
	private TrackableBehaviour _trackable;

	private void Start () {
		_enemies = new HashSet<GameObject>();
		_trackable = GetComponent<TrackableBehaviour>();
		if (_trackable) _trackable.RegisterTrackableEventHandler(this);
	}

	private void Update()
	{
		if (_engaged && _delayTimer <= 0 && _enemies.Count < MaxEnemies)
		{
			var enemy = Instantiate(EnemyPrefab);
			enemy.transform.SetParent(transform);
			// Place on top of plane
			enemy.transform.position = transform.position + new Vector3(0, enemy.transform.position.y, 0);
			// Randomly on the plane
			var x = Random.Range(-0.45f, 0.45f);
			var z = Random.Range(-0.45f, 0.45f);
			enemy.transform.localPosition = new Vector3(x, enemy.transform.localPosition.y, z);
			
			var controller = enemy.GetComponent<EnemyController>();
			controller.PlayerCamera = Camera.main.transform;
			controller.OnDead += () =>
			{
				Score.CurrentScore += 1;
				if (_enemies.Count == MaxEnemies)
				{
                    _delayTimer = Random.Range(SpawnDelay - 1f, SpawnDelay + 1f);
				}
				_enemies.Remove(enemy);
			};
			
			_enemies.Add(enemy);
			_delayTimer = Random.Range(SpawnDelay - 1f, SpawnDelay + 1f);
		}

		_delayTimer = Mathf.Max(0f, _delayTimer - Time.deltaTime);
	}

	public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
		    newStatus == TrackableBehaviour.Status.TRACKED ||
		    newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			UberDebug.LogChannel("spawn", "Enemies engaged");
			OnEngage();
		}
		else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
		         newStatus == TrackableBehaviour.Status.NOT_FOUND)
		{
			UberDebug.LogChannel("spawn", "Enemies disengaged");
			OnDisengage();
		}
		else
		{
			OnDisengage();
		}
	}

	private void OnEngage()
	{
		_engaged = true;
		
		var renderers = GetComponentsInChildren<Renderer>(true);
		var colliders = GetComponentsInChildren<Collider>(true);
		var canvases = GetComponentsInChildren<Canvas>(true);
		var controllers = GetComponentsInChildren<EnemyController>(true);

		foreach (var renderer in renderers)
		{
			renderer.enabled = true;
		}
		
		foreach (var collider in colliders)
		{
			collider.enabled = true;
		}
		
		foreach (var canvas in canvases)
		{
			canvas.enabled = true;
		}
		
		foreach (var controller in controllers)
		{
			controller.enabled = true;
		}
	}
	
	private void OnDisengage()
	{
		_engaged = false;
		
		var renderers = GetComponentsInChildren<Renderer>(true);
		var colliders = GetComponentsInChildren<Collider>(true);
		var canvases = GetComponentsInChildren<Canvas>(true);
		var controllers = GetComponentsInChildren<EnemyController>(true);

		foreach (var renderer in renderers)
		{
			renderer.enabled = false;
		}
		
		foreach (var collider in colliders)
		{
			collider.enabled = false;
		}
		
		foreach (var canvas in canvases)
		{
			canvas.enabled = false;
		}
		
		foreach (var controller in controllers)
		{
			controller.enabled = false;
		}
	}
}
