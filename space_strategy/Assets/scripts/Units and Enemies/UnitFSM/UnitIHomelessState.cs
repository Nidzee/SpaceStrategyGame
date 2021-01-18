using UnityEngine;

public class UnitIHomelessState : IUnitState
{
    private bool isChangerColor = false;
    private Color color;

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);


        if (unit.home)
        {
            // if (unit.isApproachHome) // if he is at home already
            // {
            //     return unit.unitIdleState;
            // }
            if (unit.resource) // if he became homeless while carrying resource or (gathering not sure)
            {
                isChangerColor = false;
                unit.GetComponent<SpriteRenderer>().color = Color.green;
                unit.destination = unit.storage.dispenserPosition;
                return unit.unitIGoToState;
            }
            else // if he became homeless while going on job
            {
                isChangerColor = false;
                unit.GetComponent<SpriteRenderer>().color = Color.green;
                unit.destination = unit.home.angarPosition;
                return unit.unitIGoToState;
            }
        }

        else 
            return unit.unitIHomelessState;
    }

    private void DoMyState(Unit unit)
    {
        // Logic
        if (!isChangerColor)
        {
            color = unit.GetComponent<SpriteRenderer>().color;
            unit.GetComponent<SpriteRenderer>().color = Color.gray;
        }
    }
}

