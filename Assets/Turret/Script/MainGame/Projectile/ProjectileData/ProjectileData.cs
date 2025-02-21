using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Game/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    public float BaseDamage = 10f;
    public float Speed = 20f;
    public DamageType DamageType = DamageType.Normal;
    public bool CanPierce = false;
    public int MaxPierceCount = 1;
    public bool CanRicochet = false;
    public int MaxRicochetCount = 1;
    public float LifeTime = 10f;
}
