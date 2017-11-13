using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public ScoreManager Score;
    public GameObject HitFlash;
    public int MaxHealth = 100;
    public int HealthPoints = 100;
    public Weapon CurrentWeapon;
    public Transform InventoryTransform;

    private Camera _camera;
    private bool _shooting;
    
    public delegate void EventHandler();
    public static event EventHandler OnDamageTaken;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        Score.CurrentScore = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // If selecting a weapon
            if (Physics.Raycast(ray, out hit, 50, 1 << 12))
            {
                var newWeapon = hit.collider.gameObject.GetComponent<DroppedWeapon>().WeaponStats;
                Destroy(hit.collider.gameObject);

                var droppedWeapon = Instantiate(CurrentWeapon.Prefab);
                droppedWeapon.transform.SetParent(InventoryTransform);
                droppedWeapon.transform.localPosition = new Vector3(Random.Range(-0.3f, 0.3f), 0.25f, Random.Range(-0.15f, 0.15f));
                droppedWeapon.transform.eulerAngles = new Vector3(Random.value * 360, Random.value * 360, Random.value * 360);

                CurrentWeapon = newWeapon;
            }
            else if (!_shooting)
            {
                StartCoroutine(Fire());
            }
        }
    }

    private IEnumerator Fire()
    {
        _shooting = true;
        var bullet = Instantiate(CurrentWeapon.BulletPrefab);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.LookRotation(_camera.transform.forward);

        var bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Direction = _camera.transform.forward;
        bulletScript.Speed = 1f;
        bulletScript.Damage = CurrentWeapon.Damage;
        Destroy(bullet, 3f);
        yield return new WaitForSeconds(1f / CurrentWeapon.RateOfFire);
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
        if (HealthPoints <= 0)
        {
            if (Score.CurrentScore > Score.HighScore)
            {
                Score.HighScore = Score.CurrentScore;
            }
            SceneManager.LoadScene("EndGame");
        }
    }
}