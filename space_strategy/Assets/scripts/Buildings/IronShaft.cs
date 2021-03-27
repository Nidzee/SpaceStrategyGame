using UnityEngine;

public class IronShaft : MineShaft
{
    public override void ConstructBuilding(Model model)
    {
        base.ConstructBuilding(model);

        mineShaftData.type = 2;
        mineShaftData.PlaceBuilding(model);

        ISStaticData.ironShaft_counter++;
        this.gameObject.name = "IS" + ISStaticData.ironShaft_counter;
        // myName = this.name;



        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[1];
        info.mapPoints[0] = model.BTileZero.transform;



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
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }

    private void ReloadUnitManageMenuInfo()
    {
        GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterShaftDestroying(this);
    }
}