using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player : People
{
    public Armor Armor { get; private set; }
    public Weapon Weapon { get; private set; }

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
}
