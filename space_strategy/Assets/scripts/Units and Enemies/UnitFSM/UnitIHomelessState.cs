using UnityEngine;
using Pathfinding;


public class UnitIHomelessState : IUnitState
{
    private bool isChangerColor = false;
    private Color color;

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);


        if (unit.home)
        {
            if (unit.isApproachHome) // if he is at home already
            {
                isChangerColor = false;
                unit.GetComponent<SpriteRenderer>().color = Color.green;
                
                return unit.unitIdleState;
            }

            if (unit.resource) // if he became homeless while carrying resource or (gathering not sure)
            {
                isChangerColor = false;
                unit.GetComponent<SpriteRenderer>().color = Color.green;


                unit.GetComponent<AIDestinationSetter>().target = unit.storage.storageConsumer.transform;


                unit.destination = unit.storage.storageConsumer.transform.position;
                return unit.unitIGoToState;
            }

            else // if he became homeless while going on job
            {
                isChangerColor = false;
                unit.GetComponent<SpriteRenderer>().color = Color.green;


                unit.GetComponent<AIDestinationSetter>().target = unit.home.angar.transform;


                
                unit.destination = unit.home.angar.transform.position;
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

