using UnityEngine;

public class UnitApproachState : IUnitState
{
    public IUnitState DoState(Unit unit)
    {
        return unit.unitIdleState;
    }
}

