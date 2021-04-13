using UnityEngine;

public class CrystalShaft : MineShaft
{
    public override void ConstructBuilding(Model model)
    {
        type = 1;

        base.ConstructBuilding(model);

        this.gameObject.name = "CS" + CSStaticData.crystalShaft_counter;
        CSStaticData.crystalShaft_counter++;


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[1];
        info.mapPoints[0] = model.BTileZero.transform;
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        ResourceManager.Instance.crystalShaftList.Add(this);
    }

    public void ConstructShaftFromFile()
    {
        ResourceManager.Instance.crystalShaftList.Add(this);
    }

    public override void DestroyBuilding()
    {
        base.DestroyBuilding();

        ResourceManager.Instance.crystalShaftList.Remove(this);

        Destroy(gameObject);
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }
}