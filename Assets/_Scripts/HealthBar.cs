using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

	public PlayerController Controller;

	private void OnEnable()
	{
		PlayerController.OnDamageTaken += UpdateHealth;
	}

	public void UpdateHealth()
	{
		transform.DOScaleX((float) Controller.HealthPoints / Controller.MaxHealth, 1f);
	}
}
