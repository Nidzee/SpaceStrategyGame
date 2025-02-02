﻿using UnityEngine;

public class BomberAttackState : IBomberState
{
    private float bomberAttackTimer = 2f;
    private bool isTimerOver = false;


    public IBomberState DoState(EnemyBomber bomber)
    {
        DoMyState(bomber);

        return bomber.attackState;
    }

    private void DoMyState(EnemyBomber bomber)
    {
        CoolDownLogic();
        
        if (isTimerOver)
        {
            bomber.Attack();
        }
    }

    private void CoolDownLogic()
    {
        bomberAttackTimer -= Time.deltaTime;

        if (bomberAttackTimer <= 0)
        {
            bomberAttackTimer = 2f;
            isTimerOver = true;
        }
    }
}