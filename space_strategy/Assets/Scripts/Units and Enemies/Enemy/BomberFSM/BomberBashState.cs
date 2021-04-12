using UnityEngine;

public class BomberBashState : IBomberState
{
    private float bashTimer = 5f;
    private bool isTimerOver = false;
    private bool flag = false;

    private bool isIndicatorInitialized = false;

    public IBomberState DoState(EnemyBomber bomber)
    {
        if (!flag)
        {
            flag = true;
            bomber.bashAdditionalDamage = 5;
        }

        if (!isIndicatorInitialized)
        {
            isIndicatorInitialized = true;
            bomber.powerOffIndicator.SetActive(true);
        }


        DoMyState(bomber);

        if (isTimerOver)
        {
            isTimerOver = false;
            bashTimer = 5f;
            flag = false;
            bomber.bashAdditionalDamage = 0;

            bomber._path = null;
            bomber.RebuildCurrentPath();

            
            isIndicatorInitialized = false;
            bomber.powerOffIndicator.SetActive(false);

            return bomber.bomberGoToState;
        }


        return bomber.bomberBashState;
    }

    private void DoMyState(EnemyBomber bomber) // sleeping
    {
        Debug.Log("Bash state!");

        // Roll animation
        CoolDownLogic();
    }

    private void CoolDownLogic()
    {
        bashTimer -= Time.deltaTime;
        if (bashTimer <= 0)
        {
            bashTimer = 5f;
            isTimerOver = true;
        }
    }
}