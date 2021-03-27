using UnityEngine;

public class BomberAttackState : IBomberState
{
    public IBomberState DoState(EnemyBomber bomber)
    {
        return bomber.bomberIdleState;
    }

    private void DoMyState()
    {
        
    }
}