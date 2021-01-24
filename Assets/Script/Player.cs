using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player : People
{
    [SerializeField] private Armor armor;
    public Armor Armor { get { return armor; } private set { this.armor = value; } }
    [SerializeField] private Weapon weapon;
    public Weapon Weapon { get { return weapon; } private set { this.weapon = value; } }

    public void SetItem(Item item)
    {
        if (item is Armor)
        {
            this.Armor?.DisposeEffect(this);
            this.Armor = item as Armor;
            this.Armor.ActiveEffect(this);
        }
        else if (item is Weapon)
        {
            this.Weapon?.DisposeEffect(this);
            this.Weapon = item as Weapon;
            this.Weapon.ActiveEffect(this);
        }
        else Debug.LogWarning("Not Match Type."+item.GetType());
    }

    public static Player Default
    {
        get
        {
            return new Player()
            {
                Hp = 100,
                MaxHp = 1000,
                Atk = 100,
            };
        }
    }
}
