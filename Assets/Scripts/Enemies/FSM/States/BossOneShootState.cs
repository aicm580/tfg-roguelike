﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOneShootState : State
{
    public BossOneShootState(GameObject enemy, StateType state) : base(enemy, state) { }

    private CharacterShooting characterShooting;
    private float timer;
    private float delay = 1f;

    public override void OnStateEnter()
    {
        characterShooting = enemy.GetComponent<CharacterShooting>();
        timer = 0;
    }

    public override void UpdateState()
    {
        if (timer < delay)
        {
            timer += Time.deltaTime;
        }
        else
        {
            characterShooting.Shoot(enemy.transform.position, Vector2.zero, Quaternion.identity, DamageOrigin.Boss);
            enemy.GetComponent<FiniteStateMachine>().EnterNextState();
        }
    }
}
