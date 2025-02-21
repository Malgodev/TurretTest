using ProjectilePooling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class PlayerController : TurretController
{
    [Header("LayerMask")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Config")]
    private float lastFireTime;

    public event Action<int> OnPlayerHealthChange;
    public event Action OnPlayerDead;

    private void Start()
    {
                
    }

    void Update()
    {
        TurnTurret(GetMouseWorldPosition());

        lastFireTime += Time.deltaTime;

        if (Input.GetMouseButton(0) && lastFireTime > fireRate)
        {
            ProjectileController bullet = ProjectilePoolManager.Instance.SpawnProjectile(
                ProjectilePoolManager.ProjectileType.PlayerBullet,
                transform.position,
                transform.rotation,
                gameObject
            );
        }


        //if (Input.GetMouseButton(0) && lastFireTime > fireRate)
        //{
        //    lastFireTime = 0;

        //    // StartCoroutine(BeginShoot());

        //    Fire();
        //}
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPosition = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            worldPosition = hit.point;
        }

        return worldPosition;
    }

    protected override void Fire()
    {
        TurretUpgradeController.Instance.CurUpgrade.TryGetValue(EUpgradeName.BulletNumber, out int bulletNum);

        for (int i = 0; i <= bulletNum; i++)
        {
            base.Fire();
        }
    }

    public override void GettingDamage(int damage)
    {
        base.GettingDamage(damage);

        OnPlayerHealthChange?.Invoke(health);

        if (health <= 0)
        {
            OnPlayerDead?.Invoke();
        }
    }
}
