using UnityEngine;

public class BomberBashState : IBomberState
{
    public IBomberState DoState(EnemyBomber bomber)
    {
        return bomber.bomberIdleState;
    }

    private void DoMyState()
    {
        
    }
}