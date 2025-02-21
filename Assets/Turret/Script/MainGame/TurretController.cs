using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TurretController : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] protected Transform turret;

    [Header("Config")]
    protected float fireRate = 0.1f;
    private float maxFireRange = 500f;

    [field: Header("Stat")]
    [field: SerializeField] public int health { get; protected set; } = 100;

    private WaitForSeconds delayBetweenAmmo;

    private void Awake()
    {
        delayBetweenAmmo = new WaitForSeconds(0.05f);
    }

    protected void TurnTurret(Vector3 targetPosition)
    {
        if (targetPosition == Vector3.zero)
        {
            return;
        }

        Vector3 direction = targetPosition - transform.position;

        direction.y = 0;

        turret.rotation = Quaternion.LookRotation(direction);
    }

    protected IEnumerator Fire()
    {
        //RaycastHit hit;

        //Physics.Raycast(turret.position + turret.forward.normalized * 3f, turret.forward, out hit, maxFireRange);

        GameObject ammo = AmmoPool.SharedInstance.GetPooledObject();

        //TurretController turretHited = hit.collider.GetComponentInParent<TurretController>();

        //if (turretHited != null)
        //{
        //    turretHited.GettingDamage(UnityEngine.Random.Range(minDamage, maxDamage));
        //}

        if (ammo != null)
        {
            AmmoController ammoController = ammo.GetComponent<AmmoController>();

            Vector3 targetPosition = turret.position + turret.forward;
            targetPosition.y = 1.5f;


            ammoController.SetBulletInfo(GameManager.ParseTag(this.tag), targetPosition, turret.transform.rotation);

            ammo.SetActive(true);

            yield return null;
        }
    }

    public virtual void GettingDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            HandleDestruction();
        }
    }

    protected virtual void HandleDestruction()
    {

    }
}
