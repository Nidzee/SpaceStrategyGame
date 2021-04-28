public class BomberIdleState : IBomberState
{
    private bool startPathCreation = false;

    public IBomberState DoState(EnemyBomber bomber)
    {
        DoMyState(bomber);

        if (bomber.isBashIntersects)
        {
            bomber.isBashIntersects = false;
            return bomber.bashState;
        }

        if (bomber.path != null)
        {
            return bomber.movingState;
        }

        return bomber.idleState;
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