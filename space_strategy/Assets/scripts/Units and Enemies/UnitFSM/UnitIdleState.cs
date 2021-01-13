using UnityEngine;

public class UnitIdleState : IUnitState
{
    public IUnitState DoState(Unit unit)
    {
        if(unit.workPlace)
        {
            return unit.unitApproachState;
        }
        
        else
        {
            return unit.unitIdleState;
        }
    }
}
