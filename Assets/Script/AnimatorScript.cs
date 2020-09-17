﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatorScript : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    GameObject player, enemy;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.name.Equals("PlayerImage"))
        {
            enemy=GameObject.Find("EnemyImage");
            enemy.GetComponent<Animator>().SetBool("enemyWait", true);
        }
        else
        {
            player=GameObject.Find("PlayerImage");
            player.GetComponent<Animator>().SetBool("playerWait", true);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.name.Equals("PlayerImage"))
        {
            enemy = GameObject.Find("EnemyImage");
            enemy.GetComponent<Animator>().SetBool("enemyWait", false);
        }
        else
        {
            player = GameObject.Find("PlayerImage");
            player.GetComponent<Animator>().SetBool("playerWait", false);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
    
}
