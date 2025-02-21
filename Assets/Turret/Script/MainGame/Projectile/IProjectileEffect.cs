using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileEffect
{
    void OnHit(GameObject target, ProjectileController projectile);
    void OnMove(ProjectileController projectile);
    bool ShouldDestroyOnHit();
}
