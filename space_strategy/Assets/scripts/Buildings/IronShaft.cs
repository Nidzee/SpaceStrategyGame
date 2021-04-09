using UnityEngine;

public class IronShaft : MineShaft
{
    public override void ConstructBuilding(Model model)
    {
        type = 2;

        base.ConstructBuilding(model);

        ISStaticData.ironShaft_counter++;
        this.gameObject.name = "IS" + ISStaticData.ironShaft_counter;


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[1];
        info.mapPoints[0] = model.BTileZero.transform;
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




        ResourceManager.Instance.ironShaftList.Add(this);
    }

    public void ConstructShaftFromFile(MineShaftSavingData mineShaftSavingData)
    {
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