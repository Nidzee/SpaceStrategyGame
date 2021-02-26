using UnityEngine;
using Pathfinding;

public class UnitResourceLeavingState : IUnitState
{
    private bool isCoolDownOver = false;
    // private float unitCoolDownTimer = 2f;

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);


        if (!unit.home) // means that we dont have job and home
        {
            // we dont have resource, job, home
            return unit.unitIHomelessState;
        }

        if (isCoolDownOver)
        {
            isCoolDownOver = false;

            if (unit.workPlace) // we still have job - go to work
            {
                unit.GetComponent<AIDestinationSetter>().target = unit.workPlace.dispenser.transform;

                unit.destination = unit.workPlace.dispenser.transform.position;
                return unit.unitIGoToState;
            }
            else // we dont have job - go home
            {
                unit.GetComponent<AIDestinationSetter>().target = unit.home.angar.transform;

                unit.destination = unit.home.angar.transform.position;
                return unit.unitIGoToState;
            }
        }

        else 
            return unit.unitResourceLeavingState;
    }

    private void DoMyState(Unit unit) // sleeping
    {
        if (unit.resource) // Resource destruction
        {
            GameObject.Destroy(unit.resource);
        }

        // I can increment resource count here for easy saving data if file

        isCoolDownOver = true;

        // CoolDownLogic();
    }

    // private void CoolDownLogic() // change to coroutine!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    // {
    //     unitCoolDownTimer -= Time.deltaTime;
    //     if (unitCoolDownTimer <= 0)
    //     {
    //         unitCoolDownTimer = 2f;
    //         isCoolDownOver = true;
    //     }
    // }
}
