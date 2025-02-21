using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyController : MonoBehaviour, IDamageable
{
    public void TakeDamage(float damage, DamageType type, GameObject source)
    {
        Debug.Log($"{this.name}, {damage}, {type.ToString()}, {source.name}");
    }
}
