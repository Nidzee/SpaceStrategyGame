public class IronShaft : MineShaft
{
    public override void ConstructBuilding(Model model)
    {
        base.ConstructBuilding(model);

        mineShaftData.type = 2;
        mineShaftData.PlaceBuilding(model);

        ISStaticData.ironShaft_counter++;
        this.gameObject.name = "IS" + ISStaticData.ironShaft_counter;
        gameUnit.name = this.name;


        OnShaftDestroyed += GameViewMenu.Instance.unitManageMenuReference.RemoveIronScrollItem;
        OnUnitManipulated += GameViewMenu.Instance.unitManageMenuReference.ReloadIronSlider;
        OnShaftDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;





        ResourceManager.Instance.ironShaftList.Add(this);
    }

    public override void Invoke() 
    {
        base.Invoke();

        MineShaftStaticData.shaftMenuReference.ReloadPanel(this);
    }

    public override void DestroyBuilding()
    {
        base.DestroyBuilding();

        ResourceManager.Instance.ironShaftList.Remove(this);

        // On Shaft destroyed
        ReloadUnitManageMenuInfo();
        
        Destroy(gameObject);
        AstarPath.active.Scan();
    }

    private void ReloadUnitManageMenuInfo()
    {
        GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterShaftDestroying(this);
    }
}