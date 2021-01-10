using UnityEngine;

public class UnitGatherState : IUnitState
{
    public IUnitState DoState(Unit unit)
    {
        return unit.unitIdleState;
    }
}
