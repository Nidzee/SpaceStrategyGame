using UnityEngine;

public class UnitIGatherState : IUnitState
{
    private float gatheringSpeed = 1f;

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);

        if (!unit.home) // means that we dont have job and home
        {
            if(unit.isGatheringComplete)
            {
                unit.isGatheringComplete = false;
                unit.destination = unit.home.gameObject.transform.GetChild(0).position;
                return unit.unitIHomelessState;
            }
            else
            {
                GameObject.Destroy(unit.resource);
                return unit.unitIHomelessState;
            }
        }

        else if (!unit.workPlace) // if we lost job - destroy resource and go home at any time
        {
            // Destroy resource
            GameObject.Destroy(unit.resource);

            unit.isGatheringComplete = false; 

            unit.destination = unit.home.gameObject.transform.GetChild(0).position;
            return unit.unitIGoToState;
        }

        else if (unit.isGatheringComplete)
        {
            unit.isGatheringComplete = false;  

            unit.destination = unit.sklad.gameObject.transform.GetChild(0).position;
            return unit.unitIGoToState;
        }

        else 
            return unit.unitIGatherState;
    }

    private void DoMyState(Unit unit)
    {
        if (!unit.resource) // Creates unit-resource object inside shaft
        {
            InitUnitResourcePrefab(unit);// Stinky code
            
            unit.resource = GameObject.Instantiate(
                            unit.resourcePrefab, 
                            unit.workPlace.gameObject.transform.GetChild(0).position, 
                            Quaternion.identity);
            //isResourceCreated = true;            
        }

        if (!unit.isGatheringComplete) // move resource object towards unit
        {
            unit.resource.transform.position = Vector3.MoveTowards(unit.resource.transform.position, 
                                                unit.transform.position, gatheringSpeed*Time.deltaTime);
        }
    }

    private void InitUnitResourcePrefab(Unit unit)
    {
        if (unit.workPlace.GetComponent<CrystalShaft>())
        {
            unit.resourcePrefab = CrystalShaft.crystalShaftResourcePrefab;
        }
        else if (unit.workPlace.GetComponent<GelShaft>())
        {
            unit.resourcePrefab = unit.workPlace.gameObject; // FIX
        }
        else if (unit.workPlace.GetComponent<IronShaft>())
        {
            unit.resourcePrefab = unit.workPlace.gameObject; // FIX
        }
    }
}

