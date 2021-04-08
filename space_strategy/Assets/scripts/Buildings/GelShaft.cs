using UnityEngine;

public class GelShaft : MineShaft
{
    public override void ConstructBuilding(Model model)
    {
        base.ConstructBuilding(model);

        mineShaftData.type = 3;
        mineShaftData.PlaceBuilding(model);

        GSStaticData.gelShaft_counter++;
        this.gameObject.name = "GS" + GSStaticData.gelShaft_counter;
        // myName = this.name;



        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[2];
        info.mapPoints[0] = model.BTileZero.transform;
        info.mapPoints[1] = model.BTileOne.transform;



        OnShaftDestroyed += GameViewMenu.Instance.unitManageMenuReference.RemoveGelScrollItem;
        OnUnitManipulated += GameViewMenu.Instance.unitManageMenuReference.ReloadGelSlider;
        OnShaftDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;




        ResourceManager.Instance.gelShaftList.Add(this);


        // Helper object reinitialization
        // base.mineShaftData.HelperObjectInit(this);
        gameObject.transform.GetChild(0).transform.position = mineShaftData._tileOccupied1.transform.position + OffsetConstants.buildingOffset;
        mineShaftData.dispenser.transform.position = gameObject.transform.GetChild(0).transform.position;
    }

    public void ConstructShaftFromFile(MineShaftSavingData mineShaftSavingData)
    {
        OnShaftDestroyed += GameViewMenu.Instance.unitManageMenuReference.RemoveGelScrollItem;
        OnUnitManipulated += GameViewMenu.Instance.unitManageMenuReference.ReloadGelSlider;
        OnShaftDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;

        ResourceManager.Instance.gelShaftList.Add(this);

        gameObject.transform.GetChild(0).transform.position = mineShaftData._tileOccupied1.transform.position + OffsetConstants.buildingOffset;
        mineShaftData.dispenser.transform.position = gameObject.transform.GetChild(0).transform.position;
    }

    public override void Invoke() 
    {
        base.Invoke();

        MineShaftStaticData.shaftMenuReference.ReloadPanel(this);
    }

    public override void DestroyBuilding()
    {
        base.DestroyBuilding();

        ResourceManager.Instance.gelShaftList.Remove(this);

        ReloadUnitManageMenuInfo();
        // ReloadBuildingsManageMenuInfo();
        
        Destroy(gameObject);
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }

    private void ReloadUnitManageMenuInfo()
    {
        GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterShaftDestroying(this);
    }

}