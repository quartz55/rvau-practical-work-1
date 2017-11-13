using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb_collision : MonoBehaviour {
    //private Transform explosionPrefab;
    private Rigidbody m_rigidbody;
	// Use this for initialization
	void Start () {
        m_rigidbody = GetComponent<Rigidbody>();
	}

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        // Rotate the object so that the y-axis faces along the normal of the surface
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        //Instantiate(explosionPrefab, pos, rot);
        Destroy(gameObject);
    }

        // Update is called once per frame
        void Update () {
        
		
	}
}
