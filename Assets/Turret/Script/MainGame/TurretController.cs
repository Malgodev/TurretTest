using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TurretController : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] protected Transform turret;

    [Header("Config")]
    protected float fireRate = 0.1f;

    [field: Header("Stat")]
    [field: SerializeField] public int health { get; protected set; } = 100;

    private void Awake()
    {
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

    protected void TurnTurret(Quaternion targetRotation)
    {
        turret.rotation = targetRotation;
    }

    protected virtual void Fire()
    {
        Vector3 targetPosition = turret.position + turret.forward;
        targetPosition.y = 1.5f;

        GameObject ammo = AmmoPool.SharedInstance.GetPooledObject();

        if (ammo != null)
        {
            AmmoController ammoController = ammo.GetComponent<AmmoController>();


            ammoController.SetBulletInfo(GameManager.ParseTag(this.tag), targetPosition, AngleOffset());

            ammo.SetActive(true);
        }
    }

    public Quaternion AngleOffset(int delta = 1)
    {
        float randomAngle = UnityEngine.Random.Range(-5f * delta, 5f * delta); 
        Quaternion offsetRotation = Quaternion.Euler(0, randomAngle, 0);
        Quaternion newRotation = turret.transform.rotation * offsetRotation;

        return newRotation;
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
