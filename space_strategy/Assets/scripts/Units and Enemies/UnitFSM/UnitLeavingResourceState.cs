using UnityEngine;

public class UnitLeavingResourceState : IUnitState
{
    public IUnitState DoState(Unit unit)
    {
        DoMyState();

        return unit.unitIdleState;
    }

    private void DoMyState()
    {
        
    }
}
