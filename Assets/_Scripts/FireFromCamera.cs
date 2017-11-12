using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFromCamera : MonoBehaviour
{
	private Camera _camera;
	public GameObject BulletPrefab;

	private void Start()
	{
		_camera = GetComponent<Camera>();
	}

	private void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			StartCoroutine(Fire());
		}
	}

	private IEnumerator Fire()
	{
		var bullet = Instantiate(BulletPrefab);
		bullet.transform.position = transform.position;
		bullet.transform.rotation = Quaternion.LookRotation(_camera.transform.forward);
		bullet.GetComponent<Bullet>().Direction = _camera.transform.forward;
		bullet.GetComponent<Bullet>().Speed = 0.05f;
		yield return new WaitForSeconds(1f/30);
	} 
}
