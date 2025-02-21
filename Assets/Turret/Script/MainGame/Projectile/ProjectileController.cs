using ProjectilePooling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProjectilePooling.ProjectilePoolManager;

public class ProjectileController : MonoBehaviour, IDamageDealer
{
    [SerializeField] private ProjectileData projectileData;
    [SerializeField] private ProjectileType projectileType;
    private List<IProjectileEffect> effects = new List<IProjectileEffect>();
    private GameObject owner;
    private Rigidbody rb;
    private float currentDamage;
    private float lifeTimer;

    private int pierceCount = 0;
    private int ricochetCount = 0;

    public void Initialize(GameObject owner, ProjectileType projectileType, float damageMultiplier = 1f)
    {
        this.owner = owner;
        this.currentDamage = projectileData.BaseDamage * damageMultiplier;
        this.projectileType = projectileType;
        rb = GetComponent<Rigidbody>();
        effects.Clear();
        lifeTimer = projectileData.LifeTime;
    }

    private void Update()
    {
        // Move projectile
        if (rb.velocity.magnitude < projectileData.Speed)
        {
            rb.velocity = transform.forward * projectileData.Speed;
        }

        // Projectile effect
        foreach (IProjectileEffect effect in effects)
        {
            effect.OnMove(this);
        }

        // Disable after a certain time
        lifeTimer -= Time.deltaTime;

        if (lifeTimer < 0)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null) {
            damageable.TakeDamage(GetDamage(), GetDamageType(), GetOwner());

            foreach (IProjectileEffect effect in effects)
            {
                effect.OnHit(other.gameObject, this);

                if (effect.ShouldDestroyOnHit())
                {
                    ReturnToPool();
                    return;
                }
            }
        }
    }

    public float GetDamage()
    {
        return currentDamage;
    }

    public DamageType GetDamageType()
    {
        return projectileData.DamageType;
    }

    public GameObject GetOwner()
    {
        return owner;
    }

    private void ReturnToPool()
    {
        ProjectilePoolManager.Instance.ReturnProjectile(projectileType, this);
    }
}
