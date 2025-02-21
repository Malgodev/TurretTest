using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AmmoController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float ammoSpeed = 50f;

    [Header("Stat")]
    [SerializeField] protected int minDamage = 1;
    [SerializeField] protected int maxDamage = 6;
    [SerializeField] protected int bulletNum = 1;
    [SerializeField] protected float bulletSize = 0.1f;
    [SerializeField] protected int ricochet = 10;
    [SerializeField] protected bool isPiercing = false;
    [SerializeField] private ETag shooter;

    private int ricochetCount = 0;
    private float existTime = 0;

    private void OnEnable()
    {
        ricochetCount = 0;
    }

    private void Update()
    {
        existTime += Time.deltaTime;

        if (existTime >= 60)
        {
            rb.velocity = Vector3.zero;
            this.gameObject.SetActive(false);
        }

        if (rb.velocity.magnitude < ammoSpeed)
        {
            rb.velocity = transform.forward * ammoSpeed;
        }
    }

    public void SetBulletInfo(ETag shooter, Vector3 position, Quaternion rotation)
    {
        this.shooter = shooter;

        this.transform.position = position;
        this.transform.rotation = rotation;
        this.transform.localScale = new Vector3(bulletSize, bulletSize, 1);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider == null || collider.tag == ETag.Player.ToString())
        {
            return;
        }

        if (collider.tag == ETag.Enemy.ToString())
        {
            DealDamage(collider);

            if (isPiercing)
            {
                return;
            }
        }

        if (ricochetCount++ >= ricochet)
        {
            rb.velocity = Vector3.zero;
            this.gameObject.SetActive(false);
            return;
        }

        Bounce(collider);

        ricochetCount++;
    }

    public void Bounce(Collider collider)
    {
        Vector3 normal;
        float distance;
        if (Physics.ComputePenetration(
            GetComponent<Collider>(), transform.position, transform.rotation,
            collider, collider.transform.position, collider.transform.rotation,
            out normal, out distance))
        {
            normal = normal.normalized;

            rb.velocity = Vector3.Reflect(rb.velocity, normal).normalized * ammoSpeed;
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    public void DealDamage(Collider collider)
    {
        if (collider.tag != shooter.ToString())
        {
            TurretController turretController = collider.GetComponent<TurretController>();

            if (turretController != null)
            {
                turretController.GettingDamage(Random.Range(minDamage, maxDamage));
            }
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
        // GameManager.Instance.OnEnemyKilled += HealthBack;
    }
}
