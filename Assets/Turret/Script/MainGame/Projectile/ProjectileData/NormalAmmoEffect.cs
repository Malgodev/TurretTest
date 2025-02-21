using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class NormalAmmo : IProjectileEffect
{
    public void OnHit(GameObject target, ProjectileController projectile)
    {
        Debug.Log(target.name);
    }

    public void OnMove(ProjectileController projectile)
    {
        
    }

    public bool ShouldDestroyOnHit() => true;
}
