using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BulletManagerScriptableObject", order = 2)]
public class BulletManagerScriptableObject : ScriptableObject
{
    public int minDamage;
    public int maxDamage;
    public int bulletNum;
    public float bulletSize;
    public int ricochet;
    public bool isPiercing;
    public bool isVampirism;
}
