using UnityEngine;

public class IronShaft : MineShaft
{
    public override void ConstructBuilding(Model model)
    {
        type = 2;

        base.ConstructBuilding(model);

        this.gameObject.name = "IS" + ISStaticData.ironShaft_counter;
        ISStaticData.ironShaft_counter++;


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[1];
        info.mapPoints[0] = model.BTileZero.transform;
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        ResourceManager.Instance.ironShaftList.Add(this);
    }

    public void ConstructShaftFromFile()
    {
        ResourceManager.Instance.ironShaftList.Add(this);
    }

    public override void DestroyBuilding()
    {
        base.DestroyBuilding();

        ResourceManager.Instance.ironShaftList.Remove(this);
        
        Destroy(gameObject);
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }
}