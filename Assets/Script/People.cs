using System;
using UnityEngine;

[Serializable]
public class People
{
    public float Hp;
    public float MaxHp;

    public float Atk;

    public Sprite Normal, Atack;

    public People Target; // 攻撃対象

    public People()
    {
        this.Hp = this.MaxHp;
    }

    public AttackAction AttackToTarget(float damage)
    {
        return Target.AttackToMe(damage);
    }

    public AttackAction AttackToMe(float damage)
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