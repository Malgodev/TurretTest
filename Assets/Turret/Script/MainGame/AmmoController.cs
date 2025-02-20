using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float ammoSpeed = 50f;

    [SerializeField] protected int minDamage = 1;
    [SerializeField] protected int maxDamage = 6;
    [SerializeField] protected int bulletNum = 1;
    [SerializeField] protected float bulletSize = 0.1f;
    [SerializeField] protected int ricochet = 1;
    [SerializeField] protected bool isPiercing = false;


    private void Update()
    {
        rb.velocity = transform.forward * ammoSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if (ricochet count =)
        Destroy(this.gameObject);
    }

    private void SetBulletInfo()
    {
        
    }
}
