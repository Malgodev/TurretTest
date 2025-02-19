using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] private Transform turret;

    [Header("Config")]
    protected float fireRate = 0.1f;
    private float maxFireRange = 500f;

    private int fireTime = 0;

    private WaitForSecondsRealtime existAmmoTime;

    private void Awake()
    {
        existAmmoTime = new WaitForSecondsRealtime(fireRate * 0.9f);
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
        RaycastHit hit;

        Physics.Raycast(turret.position + turret.forward.normalized * 3f, turret.forward, out hit, maxFireRange);

        GameObject ammo = AmmoPool.SharedInstance.GetPooledObject();

        if (ammo != null)
        {
            Vector3 direction = hit.point - turret.position;

            ammo.transform.position = turret.position + turret.forward * direction.magnitude / 2;
            ammo.transform.rotation = turret.transform.rotation;
            ammo.transform.localScale = new Vector3(0.1f, 0.1f, direction.magnitude);

            ammo.SetActive(true);
            yield return existAmmoTime;

            ammo.SetActive(false);
        }
    }
}
