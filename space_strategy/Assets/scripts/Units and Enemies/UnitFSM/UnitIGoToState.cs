using UnityEngine;
using Pathfinding;


public class UnitIGoToState : IUnitState
{
    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);


        if (!unit.home)
        {
            return unit.unitIHomelessState;
        }


        if (!unit.workPlace && unit.destination != unit.home.angar.transform.position && unit.destination != unit.storage.storageConsumer.transform.position) // if we lost job on way
        {
            unit.GetComponent<AIDestinationSetter>().target = unit.home.angar.transform;




            unit.destination = unit.home.angar.transform.position;
            return unit.unitIGoToState;
        }


        if (unit.destination == unit.home.angar.transform.position && unit.workPlace) // if we get job on way to home
        {

            unit.GetComponent<AIDestinationSetter>().target = unit.workPlace.dispenser.transform;
            


            unit.destination = unit.workPlace.dispenser.transform.position;
            return unit.unitIGoToState;
        }


        if (unit.isApproachShaft)
        {
            unit.isApproachShaft = false;
            return unit.unitIGatherState;
        }
        else if (unit.isApproachStorage)
        {   
            unit.isApproachStorage = false;
            return unit.unitResourceLeavingState;
        }
        else if (unit.isApproachHome)
        {
            // unit.isApproachHome = false;
            return unit.unitIdleState;
        }
        

        else 
            return unit.unitIGoToState;
    }

    private void DoMyState(Unit unit)
    {
        // TODO moving logic

        // unit.transform.position = Vector3.MoveTowards(unit.transform.position, 
        //                         unit.destination, Unit.moveSpeed*Time.deltaTime);

    }
}

