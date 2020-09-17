using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public Image cover;
    public Image player, enemy;

    Sprite player_def, player_atk, enemy_def, enemy_atk;
    Animator player_anim, enemy_anim;

    void SetPlayer(Sprite sprite)
    {
        player.sprite = sprite;
    }

    void SetEnemy(Sprite sprite)
    {
        enemy.sprite = sprite;
    }

    void SetDark()
    {
        cover.color = new Color(0, 0, 0, 1);
    }

    public void SetStage(GameSystem gameSystem)
    {
        this.player_def = gameSystem.Player.Normal;
        this.player_atk = gameSystem.Player.Atack;
        this.enemy_def = gameSystem.GetStageInfo().Enemy.Normal;
        this.enemy_atk = gameSystem.GetStageInfo().Enemy.Atack;

        this.player_anim = this.player.GetComponent<Animator>();
        this.enemy_anim = this.enemy.GetComponent<Animator>();

        SetPlayer(this.player_def);
        SetEnemy(this.enemy_def);
    }

    /// <summary>
    /// 死亡後開始する前のアニメーションを行います。
    /// </summary>
    public IEnumerator PlayContinue()
    {
        yield return new WaitForSeconds(1);
    }

    /// <summary>
    /// ラウンド開始時のアニメーションを行います。
    /// </summary>
    public IEnumerator PlayRoundStart(UIController ui, int round)
    {
        SetDark();
        for(var i = 0; i <= 30; i++) // 1s . 16s
        {
            cover.color = new Color(0, 0, 0, 1 - i / 30f);
            yield return null;
        }
        yield return ui.PlayCenterText($"Round {round}");
    }

    /// <summary>
    /// 敵が攻撃するアニメーション効果を再生します。
    /// </summary>
    public void PlayAtackEnemy()
    {
        enemy_anim.SetTrigger("attack");
        Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaa");

    }

    /// <summary>
    /// 敵が攻撃するアニメーション効果を再生します。
    /// </summary>
    public void PlayAtackPlayer()
    {
        player_anim.SetTrigger("attack");
    }

    /// <summary>
    /// 攻撃するアニメーションを再生します。
    /// </summary>
    public void PlayAtackPeople()
    {
        
    }

    /// <summary>
    /// プレイやー死亡のアニメーションをします。
    /// </summary>
    public IEnumerator PlayDie()
    {
        var r = player.transform.rotation;
        for(var i = 0; i < 60; i++)
        {
            player.transform.Rotate(Vector3.forward, 5);
            cover.color = new Color(0, 0, 0, i / 60f);
            yield return null;
        }
        player.transform.rotation = r;
    }

    
}
