using UnityEngine;

public class UnitIHomelessState : IUnitState
{
    private bool isChangerColor = false;
    private Color color;
    private bool isIndicatorInitialized = false;

    private void StateReset(Unit unit)
    {
        isChangerColor = false;
        isIndicatorInitialized = false;

        unit.powerOffIndicator.SetActive(false);
        unit.GetComponent<SpriteRenderer>().color = Color.green;
    }

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);

        if (unit.Home)
        {
            // if he is at home already
            if (unit.isApproachHome) 
            {
                // BUILD NEW PATH TO NEW GARAGE HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                StateReset(unit);

                return unit.unitIdleState;
            }

            // if he became homeless while carrying resource or (gathering not sure)
            if (unit.resource)
            {
                StateReset(unit);
                unit.ChangeDestination((int)UnitDestinationID.Storage);
                unit.RebuildPath();
                
                return unit.unitIGoToState;
            }

            // if he became homeless while going on job
            else 
            {
                StateReset(unit);
                unit.ChangeDestination((int)UnitDestinationID.Home);
                unit.RebuildPath();
                
                return unit.unitIGoToState;
            }
        }

        else 
            return unit.unitIHomelessState;
    }

    private void DoMyState(Unit unit)
    {
        if (!isIndicatorInitialized)
        {
            isIndicatorInitialized = true;
            unit.powerOffIndicator.SetActive(true);
        }

        if (!isChangerColor)
        {
            color = unit.GetComponent<SpriteRenderer>().color;
            unit.GetComponent<SpriteRenderer>().color = Color.gray;
        }
    }
}

