﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private Enemy enemy;
    private float patrolTimer;
    private float patrolDuration = 10;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;   
    }

    public void Execute()
    {
        Debug.Log("patroling");
        Patrol();
        enemy.Move();

        if(enemy.Target != null)
        {
            enemy.ChangeState(new RangeState());
        }
       
    }

    public void Exit()
    {
      
    }

    public void OnTriggerEnter(Collider2D other)
    {
        if(other.tag == "Edge")
        {
            enemy.ChangeDirection();
        }
    }
    private void Patrol()
    {
        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }
}
