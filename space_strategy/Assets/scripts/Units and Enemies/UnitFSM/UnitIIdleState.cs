using UnityEngine;

public class UnitIdleState : IUnitState
{
    private int targetRelaxPoint = 0;

    private void StateReset()
    {
        targetRelaxPoint = 1;
    }

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);

        if (!unit.Home)
        {
            unit.ChangeDestination((int)UnitDestinationID.Null);
            StateReset();
            return unit.unitData.unitIHomelessState;
        }

        if (unit.WorkPlace)
        {
            StateReset();

            unit.unitData.isApproachHome = false;
            unit.ChangeDestination((int)UnitDestinationID.WorkPlace);// unit.GetComponent<AIDestinationSetter>().target = unit.workPlace.GetUnitDestination();// unit.destination = (unit.workPlace.GetUnitDestination()).position;

            return unit.unitData.unitIGoToState;
        }

        else 
            return unit.unitData.unitIdleState;
    }

    private void DoMyState(Unit unit)
    {
        if (unit.Home)
        {
            if (targetRelaxPoint == 0)
            {
                unit.transform.position = Vector3.MoveTowards(unit.transform.position, 
                                    unit.unitData.home.garageData.relaxPointCENTER.transform.position, UnitStaticData.moveSpeed*Time.deltaTime);

                if (unit.transform.position == unit.unitData.home.garageData.relaxPointCENTER.transform.position)
                {
                    targetRelaxPoint++;
                }
            }
            // Logic
            if (targetRelaxPoint == 1)
            {
                unit.transform.position = Vector3.MoveTowards(unit.transform.position, 
                                    unit.unitData.home.garageData.relaxPoint1.transform.position, UnitStaticData.moveSpeed*Time.deltaTime);

                if (unit.transform.position == unit.unitData.home.garageData.relaxPoint1.transform.position)
                {
                    targetRelaxPoint++;
                }
            }

            if (targetRelaxPoint == 2)
            {
                unit.transform.position = Vector3.MoveTowards(unit.transform.position, 
                                    unit.unitData.home.garageData.relaxPoint2.transform.position, UnitStaticData.moveSpeed*Time.deltaTime);

                if (unit.transform.position == unit.unitData.home.garageData.relaxPoint2.transform.position)
                {
                    targetRelaxPoint++;
                }
            }

            if (targetRelaxPoint == 3)
            {
                unit.transform.position = Vector3.MoveTowards(unit.transform.position, 
                                    unit.unitData.home.garageData.relaxPoint3.transform.position, UnitStaticData.moveSpeed*Time.deltaTime);

                if (unit.transform.position == unit.unitData.home.garageData.relaxPoint3.transform.position)
                {
                    targetRelaxPoint++;
                }
            }

            if (targetRelaxPoint == 4)
            {
                unit.transform.position = Vector3.MoveTowards(unit.transform.position, 
                                    unit.unitData.home.garageData.relaxPoint4.transform.position, UnitStaticData.moveSpeed*Time.deltaTime);

                if (unit.transform.position == unit.unitData.home.garageData.relaxPoint4.transform.position)
                {
                    targetRelaxPoint = 1;
                }
            }
        }
    }
}
