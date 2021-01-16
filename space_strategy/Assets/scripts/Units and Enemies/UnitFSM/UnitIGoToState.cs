using UnityEngine;

public class UnitIGoToState : IUnitState
{
    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);


        if (!unit.home)
        {
            return unit.unitIHomelessState;
        }

        if (!unit.workPlace && unit.destination != unit.home.transform.GetChild(0).position && unit.destination != unit.sklad.transform.GetChild(0).position) // if we lost job on way
        {
            unit.destination = unit.home.transform.GetChild(0).position;
            return unit.unitIGoToState;
        }

        if (unit.destination == unit.home.transform.GetChild(0).position && unit.workPlace) // if we get job on way to home
        {
            unit.destination = unit.workPlace.transform.GetChild(0).position;
            return unit.unitIGoToState;
        }

        if (unit.isApproachShaft)
        {
            unit.isApproachShaft = false;
            return unit.unitIGatherState;
        }

        else if (unit.isApproachSklad)
        {   
            unit.isApproachSklad = false;
            return unit.unitResourceLeavingState;
        }

        else if (unit.isApproachHome)
        {
            unit.isApproachHome = false;
            return unit.unitIdleState;
        }

        else 
            return unit.unitIGoToState;
    }

    private void DoMyState(Unit unit)
    {
        // TODO moving logic

        unit.transform.position = Vector3.MoveTowards(unit.transform.position, 
                                unit.destination, Unit._moveSpeed*Time.deltaTime);

    }

    //private void GoHomeIfWeLostJob(Unit unit) // OPTIONAL
    // {
    //     if (!unit.workPlace && !helper)
    //     {
    //         if(unit.resource)
    //         {
    //             GameObject.Destroy(unit.resource);
    //             unit.resource = null;
    //         }
    //         unit.destination = unit.home.gameObject.transform.GetChild(0).position;
    //         helper = true;
    //     }
    // }
}

