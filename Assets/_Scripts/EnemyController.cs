using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {
	
	public Transform PlayerCamera;
	public float FireRate = 1f;
	public GameObject ProjectilePrefab;
	public int MaxHealth = 25;
	public int Health = 25;

	public delegate void EventHandler();

	public event EventHandler OnDead;

	private GameObject _canvas;
	private RectTransform _canvasTransform;
	private RectTransform _hpBar;
	private Material _material;
	private Color _originalColor;
	private Coroutine _shootingCoroutine;

	private void Start () {
		_material = GetComponentInChildren<MeshRenderer>().material;
		_originalColor = _material.color;
		_canvas = transform.Find("Canvas").gameObject;
		_hpBar = _canvas.transform.Find("Foreground").GetComponent<RectTransform>();
		_canvasTransform = _canvas.GetComponent<RectTransform>();
		if (_shootingCoroutine == null) _shootingCoroutine = StartCoroutine(Fire());
	}

	private void OnEnable()
	{
		if (_shootingCoroutine == null) _shootingCoroutine = StartCoroutine(Fire());
	}

	private void OnDisable()
	{
		if (_shootingCoroutine == null) return;
		StopCoroutine(_shootingCoroutine);
		_shootingCoroutine = null;
	}

	// Update is called once per frame
	private void Update () {
		var dir = PlayerCamera.transform.position - transform.position;
		dir.y = 0;
		transform.rotation = Quaternion.LookRotation(dir);
		// Point HP bar to camera
		var canvasDir = PlayerCamera.transform.position - transform.position;
		_canvasTransform.rotation = Quaternion.LookRotation(canvasDir);
	}
	
	private IEnumerator Fire()
	{
		while (PlayerCamera != null)
		{
			var bullet = Instantiate(ProjectilePrefab);
			var direction = (PlayerCamera.position - transform.position).normalized;
			bullet.transform.position = transform.position;
			bullet.transform.rotation = Quaternion.LookRotation(direction);
			bullet.GetComponent<Bullet>().Direction = direction;
			bullet.GetComponent<Bullet>().Speed = 0.25f;
			Destroy(bullet, 5f);
			yield return new WaitForSeconds(1f / FireRate);
		}
	}

	private void Hit(GameObject bullet)
	{
		Health = Mathf.Max(0, Health - bullet.GetComponent<Bullet>().Damage);
		_material.color = Color.red;
		_material.DOColor(_originalColor, 0.5f);
		_hpBar.DOScaleX((float) Health / MaxHealth, 0.5f);
		Destroy(bullet);
		if (Health <= 0)
		{
			OnDead?.Invoke();
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Bullet"))
		{
			Hit(other.gameObject);
		}
	}
}
