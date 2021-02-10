using UnityEngine;

public class UnitIdleState : IUnitState
{
    private int targetRelaxPoint = 0;


    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);


        if (!unit.home)
        {
            targetRelaxPoint = 1;
            return unit.unitIHomelessState;
        }

        if (unit.workPlace)
        {
            targetRelaxPoint = 1;
            unit.isApproachHome = false;
            unit.destination = unit.workPlace.dispenserPosition; // child object Radius
            return unit.unitIGoToState;
        }

        else 
            return unit.unitIdleState;
    }

    private void DoMyState(Unit unit)
    {
        if (unit.home)
        {
            if (targetRelaxPoint == 0)
            {
                unit.transform.position = Vector3.MoveTowards(unit.transform.position, 
                                    unit.home.relaxPointCENTER.transform.position, Unit.moveSpeed*Time.deltaTime);

                if (unit.transform.position == unit.home.relaxPointCENTER.transform.position)
                {
                    targetRelaxPoint++;
                }
            }
            // Logic
            if (targetRelaxPoint == 1)
            {
                unit.transform.position = Vector3.MoveTowards(unit.transform.position, 
                                    unit.home.relaxPoint1.transform.position, Unit.moveSpeed*Time.deltaTime);

                if (unit.transform.position == unit.home.relaxPoint1.transform.position)
                {
                    targetRelaxPoint++;
                }
            }

            if (targetRelaxPoint == 2)
            {
                unit.transform.position = Vector3.MoveTowards(unit.transform.position, 
                                    unit.home.relaxPoint2.transform.position, Unit.moveSpeed*Time.deltaTime);

                if (unit.transform.position == unit.home.relaxPoint2.transform.position)
                {
                    targetRelaxPoint++;
                }
            }

            if (targetRelaxPoint == 3)
            {
                unit.transform.position = Vector3.MoveTowards(unit.transform.position, 
                                    unit.home.relaxPoint3.transform.position, Unit.moveSpeed*Time.deltaTime);

                if (unit.transform.position == unit.home.relaxPoint3.transform.position)
                {
                    targetRelaxPoint++;
                }
            }

            if (targetRelaxPoint == 4)
            {
                unit.transform.position = Vector3.MoveTowards(unit.transform.position, 
                                    unit.home.relaxPoint4.transform.position, Unit.moveSpeed*Time.deltaTime);

                if (unit.transform.position == unit.home.relaxPoint4.transform.position)
                {
                    targetRelaxPoint = 1;
                }
            }
        }
    }
}
