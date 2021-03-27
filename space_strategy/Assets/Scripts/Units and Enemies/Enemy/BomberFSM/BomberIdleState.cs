using UnityEngine;

public class BomberIdleState : IBomberState
{
    public IBomberState DoState(EnemyBomber bomber)
    {
        return bomber.bomberIdleState;
    }

    private void DoMyState()
    {
        
    }
}