public class GelShaft : MineShaft
{
    public override void ConstructBuilding(Model model)
    {
        base.ConstructBuilding(model);

        mineShaftData.type = 3;
        mineShaftData.PlaceBuilding(model);

        GSStaticData.gelShaft_counter++;
        this.gameObject.name = "GS" + GSStaticData.gelShaft_counter;
        gameUnit.name = this.name;


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
        AstarPath.active.Scan();
    }

    private void ReloadUnitManageMenuInfo()
    {
        GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterShaftDestroying(this);
    }

}