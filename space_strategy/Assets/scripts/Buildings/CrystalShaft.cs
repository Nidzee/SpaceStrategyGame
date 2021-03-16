public class CrystalShaft : MineShaft
{
    public override void ConstructBuilding(Model model)
    {
        base.ConstructBuilding(model);

        mineShaftData.type = 1;
        mineShaftData.PlaceBuilding(model);

        CSStaticData.crystalShaft_counter++;
        this.gameObject.name = "CS" + CSStaticData.crystalShaft_counter;  
        gameUnit.name = this.name;


        // Can be extracted to MineShaft.cs

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
        AstarPath.active.Scan();
    }

    private void ReloadUnitManageMenuInfo()
    {
        GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterShaftDestroying(this);
    }
}