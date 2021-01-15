using UnityEngine;

public class UnitIGatherState : IUnitState
{
    private float gatheringSpeed = 1f;
    private bool isResourceCreated = false;

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);

        // if(!unit.home)
        // {
        //     return unit.unitIHomelessState;
        // }

        // if (!unit.workPlace) // if we lost job
        // {
        //     // Destroy resource
        //     unit.destination = unit.home.gameObject.transform.GetChild(0).position;
        //     return unit.unitIGoToState;
        // }

        if (unit.isGatheringComplete)
        {
            unit.destination = unit.sklad.gameObject.transform.GetChild(0).position;
            isResourceCreated = false; 
            unit.isGatheringComplete = false;  
            return unit.unitIGoToState;
        }

        else 
            return unit.unitIGatherState;
    }

    private void DoMyState(Unit unit)
    {
        if (!isResourceCreated) // Creates resource object inside shaft
        {
            unit.resource = GameObject.Instantiate(
                            unit.resourcePrefab, 
                            unit.workPlace.gameObject.transform.GetChild(0).position, 
                            Quaternion.identity);
            isResourceCreated = true;            
        }

        if (!unit.isGatheringComplete) // move resource object towards unit
        {
            unit.resource.transform.position = Vector3.MoveTowards(unit.resource.transform.position, 
                                                unit.transform.position, gatheringSpeed*Time.deltaTime);
        }
    }
}

