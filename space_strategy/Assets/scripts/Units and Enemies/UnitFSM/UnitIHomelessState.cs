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
            if (unit.isApproachHome) 
            {
                StateReset(unit);
                return unit.unitIdleState;
            }

            // if he became homeless while carrying resource or (gathering not sure)
            if (unit.resource)
            {
                StateReset(unit);
                unit.ChangeDestination((int)UnitDestinationID.Storage);// unit.GetComponent<AIDestinationSetter>().target = unit.storage.GetUnitDestination();// unit.destination = unit.storage.GetUnitDestination().position;
                unit.RebuildPath();
                
                return unit.unitIGoToState;
            }

            // if he became homeless while going on job
            else 
            {
                StateReset(unit);
                unit.ChangeDestination((int)UnitDestinationID.Home);// unit.GetComponent<AIDestinationSetter>().target = unit.home.GetUnitDestination();// unit.destination = unit.home.GetUnitDestination().position;
                unit.RebuildPath();
                
                return unit.unitIGoToState;
            }
        }

        else 
            return unit.unitIHomelessState;
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

