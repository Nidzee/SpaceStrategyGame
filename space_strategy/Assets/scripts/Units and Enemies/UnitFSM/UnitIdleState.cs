using UnityEngine;

public class UnitIdleState : IUnitState
{
    public IUnitState DoState(Unit unit)
    {
        if(unit.WorkPlace)
        {
            return unit.unitApproachState;
        }
        
        else
        {
            return unit.unitIdleState;
        }
    }
}
