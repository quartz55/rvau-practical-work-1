using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToTarget : MonoBehaviour
{

	public Transform Target;
	public bool YEnabled;

	public void Update ()
	{
		var dir = Target.transform.position - transform.position;
		dir.y = YEnabled ? 1 : 0;
		transform.rotation = Quaternion.LookRotation(dir);
	}
}
