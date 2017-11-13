using DG.Tweening;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

	public PlayerController Controller;

	private void Start()
	{
		PlayerController.OnDamageTaken += UpdateHealth;
	}

	private void OnDestroy()
	{
		PlayerController.OnDamageTaken -= UpdateHealth;
	}

	private void UpdateHealth()
	{
		transform.DOScaleX((float) Controller.HealthPoints / Controller.MaxHealth, 1f);
	}
}
