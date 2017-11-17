using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeTrower : MonoBehaviour {

    public float throwForce = 40f;
    public GameObject grenadePrefab;
    
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            ThrowGrenade();
        }
	}

    //creates new grenade and throws if forwards
    void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody> ();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
    }
}
