using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

	public float Speed = 0.1f;
	public Vector3 Direction;

	private Rigidbody _rigidbody;

	private void Start () {
		_rigidbody = GetComponent<Rigidbody>();
	}
	
	private void FixedUpdate()
	{
		_rigidbody.MovePosition(_rigidbody.position + Direction * Speed);
	}
}
