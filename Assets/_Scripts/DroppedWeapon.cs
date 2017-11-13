using System.Collections.Generic;
using UnityEngine;


public class DroppedWeapon : MonoBehaviour
{
    private struct Mat
    {
        public readonly Color OriginalColor;
        public readonly Material Material;

        public Mat(Material m)
        {
            OriginalColor = m.color;
            Material = m;
        }
    }

    private List<Mat> _mats;
    public Weapon WeaponStats;

    // Use this for initialization
    private void Start()
    {
        _mats = new List<Mat>();
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            _mats.Add(new Mat(r.material));
        }
    }

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 50, Color.green);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50, 1 << 12)
            && hit.collider.gameObject == gameObject)
        {
            foreach (var mat in _mats)
            {
                mat.Material.color = Color.green;
            }
        }
        else
        {
            foreach (var mat in _mats)
            {
                mat.Material.color = mat.OriginalColor;
            }
        }
    }
}