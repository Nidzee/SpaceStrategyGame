using UnityEngine;

public class BomberIdleState : IBomberState
{
    private bool startPathCreation = false;

    public IBomberState DoState(EnemyBomber bomber)
    {
        DoMyState(bomber);

        if (bomber._path != null)
        {
            Debug.Log("Path is ready: GoToState!");

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