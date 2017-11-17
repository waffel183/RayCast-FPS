using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

    [SerializeField] private float health = 100;

    public void ApplyDamage(float damage)
    {
        health -= damage;
        Debug.Log(health);

        //when player is dead
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
