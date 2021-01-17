using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{
    MyConnection.Player player, tmp;
    [SerializeField] Text text;
    [SerializeField] Image weapon, armor;

    string id = "Unknown";
    

    public void Set(MyConnection.Player player)
    {
        this.player = player;
    }

    private void Update()
    {
        if (player != tmp)
        {
            tmp = player;
            Sync();
        }
    }

    void Sync()
    {
        this.text.text = player.Name;
        try
        {
            if (player.Config != null)
            {
                var config = JsonUtility.FromJson<GameConfig>(player.Config);
                if (config != null)
                {
                    if(config.Player.Armor.Sprite != null) armor.sprite = config.Player.Armor.Sprite;
                    if(config.Player.Weapon.Sprite != null) weapon.sprite = config.Player.Weapon.Sprite;
                }
            }
        }
        catch (Exception e)
        {

        }
    }

    public void Remove()
    {
        Destroy(this.gameObject);
    }
}
