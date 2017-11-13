using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{
    public string Name;
    public GameObject Prefab;
    public GameObject BulletPrefab;
    public float RateOfFire;
    public int Damage;
}