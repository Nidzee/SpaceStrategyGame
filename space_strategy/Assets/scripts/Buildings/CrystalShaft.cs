using UnityEngine;
using UnityEngine.UI;

public class CrystalShaft : MineShaft
{
    public RectTransform shaftPanelReference; // for panel interaction

    public static int crystalShaft_counter = 0; // For Debug and iterations (OPTIONAL)
    public static GameObject crystalShaftResourcePrefab; // resource prefab - got from PrefabManager
    public static Tile_Type placingTileType;
    public static BuildingType buildingType;
    public static GameObject buildingPrefab;
    public GameObject tileOccupied = null; // Tile on which building is set - before setruction - set to TileFree!

    public static void InitStaticFields() // Do not touch!
    {
        placingTileType = Tile_Type.RS1_crystal;
        buildingType = BuildingType.SingleTileBuilding;
        buildingPrefab = PrefabManager.Instance.crystalShaftPrefab;
        crystalShaftResourcePrefab = PrefabManager.Instance.crystalResourcePrefab;
    }

    public void Creation(Model model)
    {
        HealthPoints = 100;
        ShieldPoints = 100;

        tileOccupied = model.BTileZero; // grab reference to hex on which model is currently set
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile; // make this tile unwalkable for units and buildings

        crystalShaft_counter++;
        this.gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
        this.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = LayerConstants.buildingLayer;
        this.gameObject.tag = TagConstants.buildingTag;
        this.gameObject.name = "CrystalShaft" + CrystalShaft.crystalShaft_counter;
    }


    public override void Invoke() 
    {
        Debug.Log("Selected CrystalShaft - go menu now");
        //shaftPanelReference.GetComponent<ShaftPanel>().shaftRef = this;
        // UIPannelManager.Instance.ResetPanels((int)InitPannelIndex.shaftPanel);
        // shaftPanelReference.GetComponent<ShaftPanel>().ReloadPanel(this);
    }
}
