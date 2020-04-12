﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAttackBehaviour : StateMachineBehaviour
{
    private CharacterShooting characterShooting;
    private Vector2 direction;
    private Vector3 bulletOrigin = new Vector3();
    private Transform target;
    private float attackRange;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterShooting = animator.GetComponentInParent<CharacterShooting>();
        target = GameManager.instance.player.transform;
        attackRange = animator.GetComponentInParent<Enemy>().attackRange;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(animator.transform.position, target.position) <= attackRange)
        {
            direction = animator.GetComponentInParent<Enemy>().GetDirectionToPlayer();
            bulletOrigin = animator.transform.position + (Vector3)(direction * 0.75f);
            characterShooting.Shoot(bulletOrigin, direction, Quaternion.identity, DamageOrigin.NormalEnemy);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
