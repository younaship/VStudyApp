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

public abstract class Weapon : Item
{
    float power;

    public Weapon(string name, float power, float price, float amoPrice) : base(name)
    {
        this.power = power;
        this.Price = price;
        this.AmoPrice = amoPrice;
    }

    public override void ActiveEffect(People people)
    {
        people.Atk += power;
    }

    public override void DisposeEffect(People people)
    {
        people.Atk -= power;
    }
}

public abstract class Armor : Item
{
    public Armor(string name) : base(name) { }
}