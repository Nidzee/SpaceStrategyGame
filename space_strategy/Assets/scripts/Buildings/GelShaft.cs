using UnityEngine;

public class GelShaft : MineShaft
{
    public override void ConstructBuilding(Model model)
    {
        type = 3;

        base.ConstructBuilding(model);

        this.gameObject.name = "GS" + GSStaticData.gelShaft_counter;
        GSStaticData.gelShaft_counter++;


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[2];
        info.mapPoints[0] = model.BTileZero.transform;
        info.mapPoints[1] = model.BTileOne.transform;
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        gameObject.transform.GetChild(0).transform.position = _tileOccupied1.transform.position + OffsetConstants.buildingOffset;
        dispenser.transform.position = gameObject.transform.GetChild(0).transform.position;
        

        ResourceManager.Instance.gelShaftList.Add(this);
    }

    public void ConstructShaftFromFile()
    {
        ResourceManager.Instance.gelShaftList.Add(this);

        gameObject.transform.GetChild(0).transform.position = _tileOccupied1.transform.position + OffsetConstants.buildingOffset;
        dispenser.transform.position = gameObject.transform.GetChild(0).transform.position;
    }

    public override void DestroyBuilding()
    {
        base.DestroyBuilding();

        ResourceManager.Instance.gelShaftList.Remove(this);

        Destroy(gameObject);
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }
}