using UnityEngine;

public class UnitIHomelessState : IUnitState
{
    //private bool isGatheringComplete = false;

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);


        if(unit.home)
        {
            // Check if it is on garage already
            // return idle if it is already on garage
            // return goto if it is somwhere else in world
            return unit.unitIHomelessState;
        }

        else 
            return unit.unitIHomelessState;
    }

    private void DoMyState(Unit unit)
    {
        // Logic
    }
}

