using UnityEngine;

public class BomberIdleState : IBomberState
{
    private bool startPathCreation = false;

    public IBomberState DoState(EnemyBomber bomber)
    {
        DoMyState(bomber);

        if (bomber._path != null)
        {
            return bomber.bomberGoToState;
        }

        return bomber.bomberIdleState;
    }

    private void DoMyState(EnemyBomber bomber)
    {
        if (!startPathCreation)
        {
            startPathCreation = true;
            bomber.CreateStartPath();
        }
    }
}