using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField] Image cover;
    [SerializeField] Image player, enemy;
    [SerializeField] Image bgImage;

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

    public void Awake()
    {
        this.player_anim = this.player.GetComponent<Animator>();
        this.enemy_anim = this.enemy.GetComponent<Animator>();
    }

    /// <summary>
    /// 現在の戦闘ステージを用意します。
    /// </summary>
    public void SetBattleStage(GameSystem gameSystem)
    {
        this.player_def = gameSystem.Player.Normal;
        this.player_atk = gameSystem.Player.Atack;
        this.enemy_def = gameSystem.GetBattleRound().Enemy.Normal;
        this.enemy_atk = gameSystem.GetBattleRound().Enemy.Atack;

        this.bgImage.sprite = gameSystem.GetRound().BackImage;

        SetPlayer(this.player_def);
        SetEnemy(this.enemy_def);
    }

    public void SetShopStage(GameSystem gameSystem)
    {
        this.player_def = gameSystem.Player.Normal;
        this.player_atk = gameSystem.Player.Atack;

        this.bgImage.sprite = gameSystem.GetRound().BackImage;

        var sr = gameSystem.GetRound() as ShopRound;

        SetPlayer(this.player_def);
        SetEnemy(sr.GetShopImage());
    }

    /// <summary>
    /// 死亡後、開始する前(コンテニュー)のアニメーションを行います。
    /// </summary>
    public IEnumerator PlayContinue()
    {
        yield return new WaitForSeconds(1);
    }

    /// <summary>
    /// ラウンド開始時のアニメーションを行います。
    /// </summary>
    public IEnumerator PlayRoundStart(UIController ui, GameSystem gameSystem)
    {
        SetDark();
        StartCoroutine(PlayPlayerJoin()); // join

        for(var i = 0; i <= 30; i++) // 1s . 16s
        {
            cover.color = new Color(0, 0, 0, 1 - i / 30f);
            yield return null;
        }

        var stage = gameSystem.GetRound();
        if (stage.Type == StageType.shop) yield return ui.PlayCenterText("ショップを発見");
        else yield return ui.PlayCenterText($"Round {gameSystem.GameConfig.NowRoundIndex}");
    }

    /// <summary>
    /// ラウンド終了時(Clear)のアニメーションを行います。
    /// </summary>
    public IEnumerator PlayRoundClear(UIController ui, float waitTime = 1f)
    {
        StartCoroutine(PlayPlayerNext()); // out

        yield return new WaitForSeconds(waitTime);

        for (var i = 30; i >= 0; i--) // 1s . 16s
        {
            cover.color = new Color(0, 0, 0, 1 - i / 30f);
            yield return null;
        }

        yield return new WaitForSeconds(1);
    }


    /// <summary>
    /// 敵が攻撃するアニメーション効果を再生します。
    /// </summary>
    public void PlayAtackEnemy()
    {
        enemy_anim.SetTrigger("enemyAttack");

    }

    /// <summary>
    /// プレイヤーが攻撃するアニメーション効果を再生します。
    /// </summary>
    public void PlayAtackPlayer()
    {
        player_anim.SetTrigger("playerAttack");
    }

    /// <summary>
    /// プレイヤーがステージに入ってくるアニメーションを行います。
    /// </summary>
    public IEnumerator PlayPlayerJoin()
    {
        player_anim.SetTrigger("playerEnter");
        yield return null;
    }


    /// <summary>
    /// プレイヤーが次ステージに進むアニメーションを行います。
    /// </summary>
    public IEnumerator PlayPlayerNext()
    {
        player_anim.SetTrigger("playerExit");
        yield return null;
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

    public IEnumerator PlayEnemyDie()
    {
        yield return new WaitForSeconds(.3f);
        this.enemy.gameObject.GetComponent<DeathPR>().Play();
    }
    
}
