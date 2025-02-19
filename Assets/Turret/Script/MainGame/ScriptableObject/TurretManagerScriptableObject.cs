using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TankManagerScriptableObject", order = 1)]
public class TurretManagerScriptableObject : ScriptableObject
{
    public int damageMin;
    public int damageMax;
    public int health;
}
