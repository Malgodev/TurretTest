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

    void Update()
    {
        TurnTurret(GetMouseWorldPosition());

        lastFireTime += Time.deltaTime;

        if (Input.GetMouseButton(0) && lastFireTime > fireRate)
        {
            lastFireTime = 0;

            for (int i = 0; i < bulletNum; i++)
            {
                StartCoroutine(Fire());
            }
        }
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

    protected override void GettingDamage(int damage)
    {
        base.GettingDamage(damage);

        OnPlayerHealthChange?.Invoke(health);

        if (health <= 0)
        {
            OnPlayerDead?.Invoke();
        }
    }

    public void IncreaseDamage()
    {
        minDamage++;
        maxDamage++;
    }

    public void IncreaseBulletNumber()
    {
        bulletNum++;
    }

    public void IncreaseBulletSize()
    {
        bulletSize += 0.1f;
    }

    public void IncreaseRicochet()
    {
        ricochet++;
    }

    public void SetPiercing()
    {
        isPiercing = true;
    }

    public void SetVapirism()
    {
        GameManager.Instance.OnEnemyKilled += HealthBack;
    }

    public void HealthBack()
    {
        health += 1;
        OnPlayerHealthChange?.Invoke(health);
    }
}
