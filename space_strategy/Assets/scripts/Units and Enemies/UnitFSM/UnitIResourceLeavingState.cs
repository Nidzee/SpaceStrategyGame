using UnityEngine;

public class UnitResourceLeavingState : IUnitState
{
    private bool isStateEnd = false;
    private float timer = 2f;

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);


        // if(!unit.home)
        // {
        //     // TODO
        //     return unit.unitIHomelessState;
        // }

        if (isStateEnd)
        {
            if(unit.workPlace) // we still have job - go to work
            {
                unit.destination = unit.workPlace.gameObject.transform.GetChild(0).position;
                isStateEnd = false;
                return unit.unitIGoToState;
            }
            else // we dont have job - go home
            {
                unit.destination = unit.home.gameObject.transform.GetChild(0).position;
                isStateEnd = false;
                return unit.unitIGoToState;
            }
        }

        else 
            return unit.unitResourceLeavingState;
    }

    private void DoMyState(Unit unit) // sleeping
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 2f;
            isStateEnd = true;
        }
    }
}
