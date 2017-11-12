using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Camera _camera;
    public GameObject BulletPrefab;
    public GameObject HitFlash;

    public int RateOfFire = 3;
    public int MaxHealth = 100;
    public int HealthPoints = 100;

    public delegate void EventHandler();

    public static event EventHandler OnDamageTaken;

    private bool _shooting;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !_shooting)
        {
            StartCoroutine(Fire());
        }
    }

    private IEnumerator Fire()
    {
        _shooting = true;
        var bullet = Instantiate(BulletPrefab);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.LookRotation(_camera.transform.forward);
        bullet.GetComponent<Bullet>().Direction = _camera.transform.forward;
        bullet.GetComponent<Bullet>().Speed = 0.5f;
        Destroy(bullet, 3f);
        yield return new WaitForSeconds(1f / RateOfFire);
        _shooting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            Hit(other.gameObject);
        }
    }

    private void Hit(GameObject bullet)
    {
        Debug.Log("Ouch");
        TakeDamage(5);
        var image = HitFlash.GetComponent<Image>();
        var hitColor = image.color;
        hitColor.a = 0.3f;
        image.color = hitColor;
        image.DOFade(0f, 1f);
        Destroy(bullet);
    }

    private void TakeDamage(int ammount)
    {
        HealthPoints = Mathf.Max(0, HealthPoints - ammount);
        OnDamageTaken?.Invoke();
    }
}