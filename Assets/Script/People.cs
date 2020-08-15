using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People
{
    public int Hp;
    public int MaxHp;

    public int Atk;

    public People Target; // 攻撃対象

    public AttackAction AttackToTarget(int damage)
    {
        return Target.AttackToMe(damage);
    }

    public AttackAction AttackToMe(int damage)
    {
        Hp = Hp - damage;
        if (Hp < 0)
        {
            Hp = 0;
            return AttackAction.Kill;
        }
        return AttackAction.Success;
    }
}

public enum AttackAction
{
    Success, Miss, Kill
}