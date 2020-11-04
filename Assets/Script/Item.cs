using UnityEngine;

public abstract class Item
{
    public string Name { private set; get; }
    public float Price { protected set; get; }
    public float AmoPrice { protected set; get; }

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

public class Weapon : Item
{
    public float Power { private set; get; }

    public Weapon(string name, float price, float amoPrice, float power) : base(name)
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

public class Armor : Item
{
    public float Defence { private set; get; }

    public Armor(string name, float price, float amoPrice, float defence) : base(name)
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