using UnityEngine;

public class UnitIdleState : IUnitState
{
    private int targetIdlePoint = 0;
    private Transform[] idlePoints;
    private bool flag = false;

    private void StateReset()
    {
        flag = false;
        idlePoints = null;
        targetIdlePoint = 1;
    }

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);

        if (!unit.Home)
        {
            unit.ChangeDestination((int)UnitDestinationID.Null);
            StateReset();
            return unit.unitIHomelessState;
        }

        if (unit.WorkPlace)
        {
            StateReset();

            unit.isApproachHome = false;
            unit.ChangeDestination((int)UnitDestinationID.WorkPlace);

            unit.RebuildPath();

            return unit.unitIGoToState;
        }

        else 
            return unit.unitIdleState;
    }

    private void DoMyState(Unit unit)
    {
        if (unit.isHomeChanged == true)
        {
            unit.isHomeChanged = false;
            flag = false;
        }

        if (!flag)
        {
            idlePoints = new Transform[] {
            unit.Home.relaxPointCENTER.transform,  
            unit.Home.relaxPoint1.transform, 
            unit.Home.relaxPoint2.transform, 
            unit.Home.relaxPoint3.transform, 
            unit.Home.relaxPoint4.transform};
            flag = true;
        }

        if (unit.Home)
        {
            if (idlePoints[targetIdlePoint])
            {
                unit.transform.position = Vector3.MoveTowards(
                unit.transform.position, 
                idlePoints[targetIdlePoint].position, 
                Time.deltaTime);

                if (unit.transform.position == idlePoints[targetIdlePoint].position)
                {
                    targetIdlePoint++;
                    if (targetIdlePoint == 5)
                    {
                        targetIdlePoint = 1;
                    }
                }
            }
        }
    }
}
