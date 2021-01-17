using System;
using UnityEngine;

[Serializable]
public abstract class Item
{
    public string Name;// { private set; get; }
    public int Price;// { protected set; get; }
    public float AmoPrice;// { protected set; get; }

    public Item(string name)
    {
        this.Name = name;
    }

    /// <summary>
    /// 効果内容 
    /// </summary>
    public abstract void ActiveEffect(People people);

    public abstract void DisposeEffect(People people);


}

public class Money : Item
{
    
    public Money(int value, string name = "Money") : base(name)
    {
        this.Name = name;
        this.Price = value;
    }

    public override void ActiveEffect(People people)
    {
        throw new NotImplementedException();
    }

    public override void DisposeEffect(People people)
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public class Weapon : Item
{
    public float Power; //{ private set; get; }

    public Weapon(string name, int price, float amoPrice, float power) : base(name)
    {
        this.Power = power;
        this.Price = price;
        this.AmoPrice = amoPrice;
    }

    public override void ActiveEffect(People people)
    {
        Debug.Log("Active"+Name);
        people.Atk += Power;
    }

    public override void DisposeEffect(People people)
    {
        Debug.Log("DiActive" + Name);
        people.Atk -= Power;
    }
}

[Serializable]
public class Armor : Item
{
    public float Defence; //{ private set; get; }

    public Armor(string name, int price, float amoPrice, float defence) : base(name)
    {
        this.Defence = defence;
        this.Price = price;
        this.AmoPrice = amoPrice;
    }

    public override void ActiveEffect(People people)
    {
        people.Atk += Defence;
    }

    public override void DisposeEffect(People people)
    {
        people.Atk -= Defence;
    }
}