using UnityEngine;

public class UnitIHomelessState : IUnitState
{
    private bool isChangerColor = false;
    private Color color;

    private void StateReset(Unit unit)
    {
        isChangerColor = false;
        unit.GetComponent<SpriteRenderer>().color = Color.green;
    }

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);

        if (unit.Home)
        {
            // if he is at home already
            if (unit.unitData.isApproachHome) 
            {
                StateReset(unit);
                return unit.unitData.unitIdleState;
            }

            // if he became homeless while carrying resource or (gathering not sure)
            if (unit.unitData.resource)
            {
                StateReset(unit);
                unit.ChangeDestination((int)UnitDestinationID.Storage);// unit.GetComponent<AIDestinationSetter>().target = unit.storage.GetUnitDestination();// unit.destination = unit.storage.GetUnitDestination().position;
                return unit.unitData.unitIGoToState;
            }

            // if he became homeless while going on job
            else 
            {
                StateReset(unit);
                unit.ChangeDestination((int)UnitDestinationID.Home);// unit.GetComponent<AIDestinationSetter>().target = unit.home.GetUnitDestination();// unit.destination = unit.home.GetUnitDestination().position;
                return unit.unitData.unitIGoToState;
            }
        }

        else 
            return unit.unitData.unitIHomelessState;
    }

    private void DoMyState(Unit unit)
    {
        // Logic
        
        if (!isChangerColor)
        {
            color = unit.GetComponent<SpriteRenderer>().color;
            unit.GetComponent<SpriteRenderer>().color = Color.gray;
        }
    }
}

