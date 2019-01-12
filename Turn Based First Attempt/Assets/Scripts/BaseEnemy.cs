﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEnemy
{

    public string name;

    //Next one for Enemy Types
    public enum Type
    {
        Grass,
        Fire,
        Water,
        Electric
    }

    public Type EnemyType;

    public float baseHP;
    public float curHP;
    public float baseMP;
    public float curMP;
    public float baseATK;
    public float cutATK;
    public float baseDEF;
    public float curDEF;


}
