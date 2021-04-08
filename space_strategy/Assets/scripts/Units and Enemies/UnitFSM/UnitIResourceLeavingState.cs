using UnityEngine;

public class UnitResourceLeavingState : IUnitState
{
    private bool isCoolDownOver = false;

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);


        if (!unit.Home) // means that we dont have job and home
        {
            unit.ChangeDestination((int)UnitDestinationID.Null);
            isCoolDownOver = false;
            // we dont have resource, job, home
            return unit.unitData.unitIHomelessState;
        }

        if (isCoolDownOver)
        {
            isCoolDownOver = false;

            if (unit.WorkPlace) // we still have job - go to work
            {
                unit.ChangeDestination((int)UnitDestinationID.WorkPlace);// unit.GetComponent<AIDestinationSetter>().target = unit.workPlace.GetUnitDestination();// unit.destination = unit.workPlace.GetUnitDestination().position;
            }
            else // we dont have job - go home
            {
                unit.ChangeDestination((int)UnitDestinationID.Home);// unit.GetComponent<AIDestinationSetter>().target = unit.home.GetUnitDestination();// unit.destination = unit.home.GetUnitDestination().position;
            }

            unit.RebuildPath();


            return unit.unitData.unitIGoToState;
        }

        else 
            return unit.unitData.unitResourceLeavingState;
    }

    private void DoMyState(Unit unit) // sleeping
    {
        if (unit.unitData.resource) // Resource destruction
        {
            GameObject.Destroy(unit.unitData.resource);
        }

        // I can increment resource count here for easy saving data if file

        isCoolDownOver = true;
    }
}
