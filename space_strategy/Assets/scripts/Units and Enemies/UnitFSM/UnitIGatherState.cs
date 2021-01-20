using UnityEngine;

public class UnitIGatherState : IUnitState
{
    private float gatheringSpeed = 1f;

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);

        if (!unit.home) // means that we dont have job and home
        {
            if (unit.isGatheringComplete)
            {
                unit.isGatheringComplete = false;
                unit.destination = unit.home.angarPosition;
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

            unit.destination = unit.home.angarPosition;
            return unit.unitIGoToState;
        }

        else if (unit.isGatheringComplete)
        {
            unit.isGatheringComplete = false;  

            unit.destination = unit.storage.dispenserPosition;
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
                            unit.workPlace.dispenserPosition, 
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
            unit.resourcePrefab = GelShaft.gelShaftResourcePrefab; // FIX
        }
        else if (unit.workPlace.GetComponent<IronShaft>())
        {
            unit.resourcePrefab = IronShaft.ironShaftResourcePrefab; // FIX
        }
    }
}

