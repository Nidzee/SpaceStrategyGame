using UnityEngine;

public class Model
{
    public BuildingType buildingType = BuildingType.Clear;

    public GameObject BTileZero = null; // Reference for Hexes on map, so when building is created they are assigned
    public GameObject BTileOne = null; // Reference for Hexes on map, so when building is created they are assigned
    public GameObject BTileTwo = null; // Reference for Hexes on map, so when building is created they are assigned
    [SerializeField]public GameObject modelSprite; // FIX!
    public int rotation = 1;
    public int buildingID = 0; // Not existing building ID
    public int BSelectedTileIndex = 0;

    //TileType on which can be placed
    public Tile_Type PlacingTile;
    public Tile_Type PlacingTile_Optional; // Only need for Gel Shaft


    public bool isCanBePlaced = false; // For activating UI Button

    public void ResetModel()
    {
        buildingType = BuildingType.Clear;

        BTileZero.transform.position = BTileOne.transform.position = BTileOne.transform.position = Vector3.zero;

        BTileZero = null; // Reference for Hexes on map, so when building is created they are assigned
        BTileOne = null; // Reference for Hexes on map, so when building is created they are assigned
        BTileTwo = null; // Reference for Hexes on map, so when building is created they are assigned
        modelSprite = null;
        rotation = 1;
        buildingID = 0;
        BSelectedTileIndex = 0;

        PlacingTile = Tile_Type.FreeTile;
        PlacingTile_Optional = Tile_Type.FreeTile;
    }

    public void InitModel(int buildingID)
    {
        this.buildingID = buildingID;
        switch(buildingID)
        {
            case (int)IDconstants.IDturette: // Turette
            {
                //modelSprite = GameObject.Find("Square"); // Add sprite

                buildingType = Turette.buildingType;
                PlacingTile = Turette.PlacingTileType;
            }
            break;

            case (int)IDconstants.IDgarage: // Garage
            {
                modelSprite = null; // Add sprite

                buildingType = Garage.buildingType;
                PlacingTile = Garage.PlacingTileType;
            }
            break;

            case (int)IDconstants.IDshieldGenerator: // Shield Generator
            {
                modelSprite = null; // Add sprite

                buildingType = BuildingType.TripleTileBuilding;
                PlacingTile = Garage.PlacingTileType; // FIX
            }
            break;

            case (int)IDconstants.IDcrystalShaft: // Crystal Shaft
            {

            }
            break;

            case (int)IDconstants.IDironShaft: // Iron Shaft
            {

            }
            break;

            case (int)IDconstants.IDgelShaft: // Gel Shaft
            {
                modelSprite = null; // Add sprite

                buildingType = GelShaft.buildingType;
                PlacingTile = GelShaft.PlacingTileType;
                PlacingTile_Optional = GelShaft.PlacingTile_Optional;
            }
            break;

            case (int)IDconstants.IDantenne: // Antenne
            {

            }
            break;

            case (int)IDconstants.IDstrategicCenter: // Strategic Center
            {

            } 
            break;

            case (int)IDconstants.IDpowerPlant: // Power plant
            {

            }
            break;
        }
    }

    public void RotateModel()
    {
        rotation++;
        if(rotation>6)
            rotation = 1;

        int q = BTileZero.GetComponent<Hex>().Q;
        int r = BTileZero.GetComponent<Hex>().R;
        int s = BTileZero.GetComponent<Hex>().S;

        switch(buildingType)
        {
            case BuildingType.DoubleTileBuilding :
            {
                switch(rotation)
                {
                    case 1:
                        BTileOne = GameObject.Find(q + "." + (r+1) + "." + (s-1));
                        // no need for check for exstance as Border is 2 Hexes away from SelectedHex
                    break;
                    
                    case 2:
                        BTileOne = GameObject.Find((q-1) + "." + (r+1) + "." + s);
                        // no need for check for exstance as Border is 2 Hexes away from SelectedHex
                    break;
                    
                    case 3:
                        BTileOne = GameObject.Find((q-1) + "." + r + "." + (s+1));
                        // no need for check for exstance as Border is 2 Hexes away from SelectedHex
                    break;
                    
                    case 4:
                        BTileOne = GameObject.Find(q + "." + (r-1) + "." + (s+1));
                        // no need for check for exstance as Border is 2 Hexes away from SelectedHex
                    break;
                    
                    case 5:
                        BTileOne = GameObject.Find((q+1) + "." + (r-1) + "." + s);
                        // no need for check for exstance as Border is 2 Hexes away from SelectedHex
                    break;
                    
                    case 6:
                        BTileOne = GameObject.Find((q+1) + "." + r + "." + (s-1));
                        // no need for check for exstance as Border is 2 Hexes away from SelectedHex
                    break;
                }
                // reset positions here
            }
            break;

            case BuildingType.TripleTileBuilding:
            {
                switch(rotation)
                {
                     case 1:
                        BTileOne = GameObject.Find(q + "." + (r+1) + "." + (s-1));
                        BTileTwo = GameObject.Find(q + "." + (r-1) + "." + (s+1));
                    break;

                    case 2:
                        BTileOne = GameObject.Find((q-1) + "." + (r+1) + "." + s);
                        BTileTwo = GameObject.Find((q+1) + "." + (r-1) + "." + s);
                    break; 
                    
                    case 3:
                        BTileOne = GameObject.Find((q-1) + "." + r + "." + (s+1));
                        BTileTwo = GameObject.Find((q+1) + "." + r + "." + (s-1));
                    break;
                    
                    case 4:
                        BTileOne = GameObject.Find(q + "." + (r-1) + "." + (s+1));
                        BTileTwo = GameObject.Find(q + "." + (r+1) + "." + (s-1));
                    break;
                    
                    case 5:
                        BTileOne = GameObject.Find((q+1) + "." + (r-1) + "." + s);
                        BTileTwo = GameObject.Find((q-1) + "." + (r+1) + "." + s);
                    break;
                    
                    case 6:
                        BTileOne = GameObject.Find((q+1) + "." + r + "." + (s-1));
                        BTileTwo = GameObject.Find((q-1) + "." + r + "." + (s+1));
                    break;
                }
                // reset positions here

            }
            break;
        }

        // reset rotation for Building Sprite here
        
    }

    public void ResetBTiles(GameObject zeroHex, GameObject oneHex = null, GameObject twoHex = null)
    {
        BTileZero = zeroHex;
        BTileOne = oneHex;
        BTileTwo = twoHex;
    }
}
