using UnityEngine;

public class CrystalShaft : MineShaft
{
    public override void ConstructBuilding(Model model)
    {
        base.ConstructBuilding(model);

        mineShaftData.type = 1;
        mineShaftData.PlaceBuilding(model);

        CSStaticData.crystalShaft_counter++;
        this.gameObject.name = "CS" + CSStaticData.crystalShaft_counter;  
        // myName = this.name;

        
        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[1];
        info.mapPoints[0] = model.BTileZero.transform;



        // Can be extracted to MineShaft.cs
        OnShaftDestroyed += GameViewMenu.Instance.unitManageMenuReference.RemoveCrystalScrollItem;
        OnShaftDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;
        OnUnitManipulated += GameViewMenu.Instance.unitManageMenuReference.ReloadCrystalSlider;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;



        ResourceManager.Instance.crystalShaftList.Add(this);
    }

    public void ConstructShaftFromFile(MineShaftSavingData mineShaftSavingData)
    {
        mineShaftData.type = 1;
        
        OnShaftDestroyed += GameViewMenu.Instance.unitManageMenuReference.RemoveCrystalScrollItem;
        OnShaftDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;
        OnUnitManipulated += GameViewMenu.Instance.unitManageMenuReference.ReloadCrystalSlider;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;

        ResourceManager.Instance.crystalShaftList.Add(this);
    }

    public override void Invoke() 
    {
        base.Invoke();

        MineShaftStaticData.shaftMenuReference.ReloadPanel(this);
    }

    public override void DestroyBuilding()
    {
        base.DestroyBuilding();

        ResourceManager.Instance.crystalShaftList.Remove(this);

        ReloadUnitManageMenuInfo();

        Destroy(gameObject);
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }

    private void ReloadUnitManageMenuInfo()
    {
        GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterShaftDestroying(this);
    }
}