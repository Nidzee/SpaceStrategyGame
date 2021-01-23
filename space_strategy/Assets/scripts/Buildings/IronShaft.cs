using UnityEngine;

public class IronShaft : MineShaft
{
    public static int ironShaft_counter = 0; // For Debug and iterations (OPTIONAL)
    public static GameObject ironShaftResourcePrefab; // resource prefab - got from PrefabManager
    public static Tile_Type placingTileType;
    public static BuildingType buildingType;
    public static GameObject buildingPrefab;
    public GameObject tileOccupied = null; // Tile on which building is set - before setruction - set to TileFree!

    public static void InitStaticFields() // Do not touch!
    {
        placingTileType = Tile_Type.RS2_iron;
        buildingType = BuildingType.SingleTileBuilding;
        buildingPrefab = PrefabManager.Instance.ironShaftPrefab;
        ironShaftResourcePrefab = PrefabManager.Instance.ironResourcePrefab;
    }

    public void Creation(Model model)
    {
        tileOccupied = model.BTileZero; // grab reference to hex on which model is currently set
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile; // make this tile unwalkable for units and buildings

        ironShaft_counter++;
        this.gameObject.tag = TagConstants.buildingTag;
        this.gameObject.name = "IronShaft" + IronShaft.ironShaft_counter;
    }


    public override void Invoke() 
    {
        Debug.Log("Selected IronShaft - go menu now");
        //UIPannelManager.Instance.ResetPanels((int)InitPannelIndex.shaftPanel);
    }
}
