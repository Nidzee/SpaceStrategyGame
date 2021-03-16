using UnityEngine;

public class PowerPlantData
{
    public GameObject _tileOccupied;
    public bool isMenuOpened;

    // public PowerPlant _myPowerPlant;


    // public void Invoke()
    // {
    //     UIPannelManager.Instance.ResetPanels("PowerPlantMenu");
        
    //     PowerPlantStaticData.powerPlantMenuReference.ReloadPanel(_myPowerPlant);
    // }

    public PowerPlantData()
    {
        isMenuOpened = false;

        _tileOccupied = null;
    }

    public void ConstructBuilding(Model model)
    {
        _tileOccupied = model.BTileZero;
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
    }

    public void DestroyBuilding()
    {
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;

        if (isMenuOpened)
        {
            PowerPlantStaticData.powerPlantMenuReference.ExitMenu();
        }
    }
}