using UnityEngine;

public class Model
{
    public BuildingType buildingType = BuildingType.Clear; // Useless

    public GameObject BTileZero = null; // Reference for Hexes on map, so when building is created they are assigned
    public GameObject BTileOne = null; // Reference for Hexes on map, so when building is created they are assigned
    public GameObject BTileTwo = null; // Reference for Hexes on map, so when building is created they are assigned

    public GameObject modelSprite; // FIX!

    public int rotation = 1;
    public int buildingID = 0; // Not existing building ID
    public int BSelectedTileIndex = 0;

    //TileType on which can be placed
    public Tile_Type PlacingTile;
    public Tile_Type PlacingTile_Optional; // Only need for Gel Shaft

    public bool isModelPlacable = false; // For activating UI Button

    public GameObject tempGo = null; // for deleting object from hierarchy


    public void InitModel(int buildingID) // Initialize model with static fields from each building script
    {
        this.buildingID = buildingID;
        switch (buildingID)
        {
            case (int)IDconstants.IDturette: // Turette
            {
                modelSprite = Turette.buildingSprite;
                buildingType = Turette.buildingType;
                PlacingTile = Turette.PlacingTileType;
            }
            break;

            case (int)IDconstants.IDgarage: // Garage
            {
                modelSprite = Garage.buildingSprite;
                buildingType = Garage.buildingType;
                PlacingTile = Garage.PlacingTileType;
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

            case (int)IDconstants.IDshieldGenerator: // Shield Generator
            {

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

        // Cashing Selected Hex info
        int q = (GameHendler.Instance.SelectedHex.GetComponent<Hex>().Q);
        int r = (GameHendler.Instance.SelectedHex.GetComponent<Hex>().R);
        int s = (GameHendler.Instance.SelectedHex.GetComponent<Hex>().S);

        // Findind positions for Tiles aquoting to Building Type
        switch (buildingType)
        {
            case BuildingType.SingleTileBuilding:
                BTileZero = GameHendler.Instance.SelectedHex;
                BTileOne = null;
                BTileTwo = null;
            break;
            
            case BuildingType.DoubleTileBuilding:
                BTileZero = GameHendler.Instance.SelectedHex;
                BTileOne = GameObject.Find(q + "." + (r+1) + "." + (s-1));
                BTileTwo = null;
            break;
            
            case BuildingType.TripleTileBuilding:
                BTileZero = GameHendler.Instance.SelectedHex;
                BTileOne = GameObject.Find(q + "." + (r+1) + "." + (s-1));
                BTileTwo = GameObject.Find(q + "." + (r-1) + "." + (s+1));
            break;

            // If i want to add Another Shape of TripleTile Building
            // Then Add *NewTripleTileBuilding* to BuildingType.cs
            // And add another case with cubic coords offset (див. с.Вулик)
        }
        
        // i Dont understand it (Aske SergeyJJJJ)
        tempGo = GameObject.Instantiate (modelSprite, BTileZero.transform.position, Quaternion.Euler(0, 0, rotation*60));
        modelSprite = tempGo; // Add sprite

        //ResetBuildingSpritePositions(); // Debug
        OffsetModelPosition();
        ChechForCorrectPlacement();
    }

    public void ResetModel() // Delete all info about current model and set all fields to default
    {
        buildingType = BuildingType.Clear;

        BTileZero = null; // Reference for Hexes on map, so when building is created they are assigned
        BTileOne = null; // Reference for Hexes on map, so when building is created they are assigned
        BTileTwo = null; // Reference for Hexes on map, so when building is created they are assigned

        modelSprite = null;
        GameObject.Destroy(tempGo);

        rotation = 1;
        buildingID = 0;
        BSelectedTileIndex = 0;

        PlacingTile = Tile_Type.FreeTile;
        PlacingTile_Optional = Tile_Type.FreeTile;

        //GameHendler.Instance.ResetDebugTilesPosition(); // Reset debugging tiles
    }

    public void RotateModel() // Rotate model 
    {
        rotation++;
        if (rotation>6)
            rotation = 1;

        int q = BTileZero.GetComponent<Hex>().Q;
        int r = BTileZero.GetComponent<Hex>().R;
        int s = BTileZero.GetComponent<Hex>().S;

        switch (buildingType)
        {
            // No case for SingleTypeBuilding because it is stupid to rotate 1-Tile building

            case BuildingType.DoubleTileBuilding :
            {
                // no need for check for existance of an Hex Objects because Border is 2 Hexes away from SelectedHex
                switch (rotation)
                {
                    case 1:
                        BTileOne = GameObject.Find(q + "." + (r+1) + "." + (s-1));
                    break;
                    
                    case 2:
                        BTileOne = GameObject.Find((q-1) + "." + (r+1) + "." + s);
                    break;
                    
                    case 3:
                        BTileOne = GameObject.Find((q-1) + "." + r + "." + (s+1));
                    break;
                    
                    case 4:
                        BTileOne = GameObject.Find(q + "." + (r-1) + "." + (s+1));
                    break;
                    
                    case 5:
                        BTileOne = GameObject.Find((q+1) + "." + (r-1) + "." + s);
                    break;
                    
                    case 6:
                        BTileOne = GameObject.Find((q+1) + "." + r + "." + (s-1));
                    break;
                }
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
            }
            break;
        }

        //GameHendler.Instance.ResetBuildingSpritePositions(); // DEBUG
        OffsetModelPosition();
        ResetModelRotation();
        ChechForCorrectPlacement();
    }

    public void MoveModel() // Movement in Building Mode
    {
        GameObject tempCurrentHex = GameHendler.Instance.CurrentHex;
        GameHendler.Instance.selectTileState.setCurrentHex();
        
        if (tempCurrentHex == GameHendler.Instance.CurrentHex) // We didnt move model from previous position // NOT IDEAL
            return;

        switch (buildingType)
        {
            case BuildingType.SingleTileBuilding:
                SingleTileModelMovement();
            break;

            case BuildingType.DoubleTileBuilding:
                DoubleTileModelMovement();
            break;

            case BuildingType.TripleTileBuilding:
                TripleTileModelMovement();
            break;

            default : 
                Debug.Log("ERROR!"); // Impossible
            break;

            // If it is necessary add another Building type to "BuildingType.cs" and add functionality here
            // Also create  ...ModelMovement() function
            // Change info in Init and so on
        }
        //GameHendler.Instance.ResetBuildingSpritePositions(); // DEBUG
        
        OffsetModelPosition();
        ChechForCorrectPlacement();
    }

    public void OffsetModelPosition()
    {
        modelSprite.transform.position = BTileZero.transform.position + new Vector3 (0,0,-0.2f);
    }

    private void ResetModelRotation()
    {
        modelSprite.transform.rotation = Quaternion.Euler(0f, 0f, (rotation*60));
    }

    public void ResetBTiles(GameObject zeroHex, GameObject oneHex = null, GameObject twoHex = null)
    {
        BTileZero = zeroHex;
        BTileOne = oneHex;
        BTileTwo = twoHex;
    }

    private bool IsMapEdgePositioning(GameObject hex) // Check for Map Edge
    {
        return (hex.GetComponent<Hex>().tile_Type == Tile_Type.MapEdge);
    }

    public void ChechForCorrectPlacement() // Adding Shader for right or bad placing on map
    {
        switch (buildingType)
        {
            case BuildingType.SingleTileBuilding:
            {
                if (BTileZero.GetComponent<Hex>().tile_Type == PlacingTile)
                {
                    // Add shader GREEN and set bool to true
                    isModelPlacable = true;
                    Debug.Log("GREEN SHADER");
                }
                else
                {
                    // Add shader RED and set bool to false
                    isModelPlacable = false;
                    Debug.Log("RED SHADER");
                }
            }
            break;
            
            case BuildingType.DoubleTileBuilding:
            {
                if (buildingID == (int)IDconstants.IDgelShaft)
                {
                    if (BTileZero.GetComponent<Hex>().tile_Type == PlacingTile &&
                        BTileOne.GetComponent<Hex>().tile_Type == PlacingTile_Optional)
                    {
                        // Add shader GREEN and set bool to true
                        isModelPlacable = true;
                        Debug.Log("GREEN SHADER");
                    }
                    else
                    {
                        // Add shader RED and set bool to false
                        isModelPlacable = false;
                        Debug.Log("RED SHADER");
                    }
                }
                else if (BTileZero.GetComponent<Hex>().tile_Type == PlacingTile &&
                        BTileOne.GetComponent<Hex>().tile_Type == PlacingTile)
                {
                    // Add shader GREEN and set bool to true
                    isModelPlacable = true;
                    Debug.Log("GREEN SHADER");
                }
                else
                {
                    // Add shader RED and set bool to false
                    isModelPlacable = false;
                    Debug.Log("RED SHADER");
                }
            }
            break;
            
            case BuildingType.TripleTileBuilding:
            {
                if (BTileZero.GetComponent<Hex>().tile_Type == PlacingTile &&
                    BTileOne.GetComponent<Hex>().tile_Type == PlacingTile && 
                    BTileTwo.GetComponent<Hex>().tile_Type == PlacingTile)
                {
                    // Add shader GREEN and set bool to true
                    isModelPlacable = true;
                    Debug.Log("GREEN SHADER");
                }
                else
                {
                    // Add shader RED and set bool to false
                    isModelPlacable = false;
                    Debug.Log("RED SHADER");
                }
            }
            break;
        }
    }


    #region Model moving functions
    private void SingleTileModelMovement()
    {
        if (IsMapEdgePositioning(GameHendler.Instance.CurrentHex))  // BTileZoro always must stay on map!!!!!!!!!!!!!!!!!!!
             return;

        ResetBTiles(GameHendler.Instance.CurrentHex); // We pass BtileZero
    }

    private void DoubleTileModelMovement()
    {
        GameObject temp = null; // Helper for calculating
        int q = (GameHendler.Instance.CurrentHex.GetComponent<Hex>().Q);
        int r = (GameHendler.Instance.CurrentHex.GetComponent<Hex>().R);
        int s = (GameHendler.Instance.CurrentHex.GetComponent<Hex>().S);

        // Which Tile we hit INDEX
        switch (BSelectedTileIndex)
        {
            case 0:
            {
                if (IsMapEdgePositioning(GameHendler.Instance.CurrentHex))  // BTileZoro always must stay on map!!!!!!!!!!!!!!!!!!!
                    return;
                    
                // Finding Hex *temp* on map
                switch (rotation)
                    {
                        case 1:
                            temp = GameObject.Find(q + "." + (r+1) + "." + (s-1));
                        break;

                        case 2:
                            temp = GameObject.Find((q-1) + "." + (r+1) + "." + s);
                        break; 
                        
                        case 3:
                            temp = GameObject.Find((q-1) + "." + r + "." + (s+1));
                        break;
                        
                        case 4:
                            temp = GameObject.Find(q + "." + (r-1) + "." + (s+1));
                        break;
                        
                        case 5:
                            temp = GameObject.Find((q+1) + "." + (r-1) + "." + s);
                        break;
                        
                        case 6:
                            temp = GameObject.Find((q+1) + "." + r + "." + (s-1));
                        break;
                    }

                if (!temp)
                    return;

                ResetBTiles(GameHendler.Instance.CurrentHex, temp);
            }
            break;

            case 1:
            {
                switch (rotation)
                {
                    case 1:
                        temp = GameObject.Find(q + "." + (r-1) + "." + (s+1));
                    break;

                    case 2:
                        temp = GameObject.Find((q+1) + "." + (r-1) + "." + s);
                    break; 
                    
                    case 3:
                        temp = GameObject.Find((q+1) + "." + r + "." + (s-1));
                    break;
                    
                    case 4:
                        temp = GameObject.Find(q + "." + (r+1) + "." + (s-1));
                    break;
                    
                    case 5:
                        temp = GameObject.Find((q-1) + "." + (r+1) + "." + s);
                    break;
                    
                    case 6:
                        temp = GameObject.Find((q-1) + "." + r + "." + (s+1));
                    break;
                }

                if (!temp)
                    return;
                
                if (IsMapEdgePositioning(temp))  // BTileZoro always must stay on map!!!!!!!!!!!!!!!!!!!
                    return;
                
                ResetBTiles(temp, GameHendler.Instance.CurrentHex);
            }
            break;
        }
    }

    private void TripleTileModelMovement()
    {
        GameObject temp = null;
        GameObject temp1 = null;

        int q = (GameHendler.Instance.CurrentHex.GetComponent<Hex>().Q);
        int r = (GameHendler.Instance.CurrentHex.GetComponent<Hex>().R);
        int s = (GameHendler.Instance.CurrentHex.GetComponent<Hex>().S);

        switch(BSelectedTileIndex)
        {
            case 0:
            {
                if (IsMapEdgePositioning(GameHendler.Instance.CurrentHex))  // BTileZoro always must stay on map!!!!!!!!!!!!!!!!!!!
                    return;
                    
                switch (rotation)
                    {
                        case 1:
                            temp = GameObject.Find(q + "." + (r+1) + "." + (s-1));
                            temp1 = GameObject.Find(q + "." + (r-1) + "." + (s+1));
                        break;

                        case 2:
                            temp = GameObject.Find((q-1) + "." + (r+1) + "." + s);
                            temp1 = GameObject.Find((q+1) + "." + (r-1) + "." + s);
                        break; 
                        
                        case 3:
                            temp = GameObject.Find((q-1) + "." + r + "." + (s+1));
                            temp1 = GameObject.Find((q+1) + "." + r + "." + (s-1));
                        break;
                        
                        case 4:
                            temp = GameObject.Find(q + "." + (r-1) + "." + (s+1));
                            temp1 = GameObject.Find(q + "." + (r+1) + "." + (s-1));
                        break;
                        
                        case 5:
                            temp = GameObject.Find((q+1) + "." + (r-1) + "." + s);
                            temp1 = GameObject.Find((q-1) + "." + (r+1) + "." + s);
                        break;
                        
                        case 6:
                            temp = GameObject.Find((q+1) + "." + r + "." + (s-1));
                            temp1 = GameObject.Find((q-1) + "." + r + "." + (s+1));
                        break;
                    }

                if(!temp || !temp1)
                    return;

                ResetBTiles(GameHendler.Instance.CurrentHex, temp, temp1);
            }
            break;
            
            case 1:
            {
                switch (rotation)
                {
                    case 1:
                        temp1 = GameObject.Find(q + "." + (r-2) + "." + (s+2));
                        temp = GameObject.Find(q + "." + (r-1) + "." + (s+1));
                    break;

                    case 2:
                        temp1 = GameObject.Find((q+2) + "." + (r-2) + "." + s);
                        temp = GameObject.Find((q+1) + "." + (r-1) + "." + s);
                    break; 
                    
                    case 3:
                        temp1 = GameObject.Find((q+2) + "." + r + "." + (s-2));
                        temp = GameObject.Find((q+1) + "." + r + "." + (s-1));
                    break;
                    
                    case 4:
                        temp1 = GameObject.Find(q + "." + (r+2) + "." + (s-2));
                        temp = GameObject.Find(q + "." + (r+1) + "." + (s-1));
                    break;
                    
                    case 5:
                        temp1 = GameObject.Find((q-2) + "." + (r+2) + "." + s);
                        temp = GameObject.Find((q-1) + "." + (r+1) + "." + s);
                    break;
                    
                    case 6:
                        temp1 = GameObject.Find((q-2) + "." + r + "." + (s+2));
                        temp = GameObject.Find((q-1) + "." + r + "." + (s+1));
                    break;
                }

                if (!temp || !temp1)
                    return;

                if (IsMapEdgePositioning(temp)) // BTileZoro always must stay on map!!!!!!!!!!!!!!!!!!!
                    return;

                ResetBTiles(temp, GameHendler.Instance.CurrentHex, temp1);
            }
            break;
            
            case 2:
            {
                switch (rotation)
                {
                    case 1:
                        temp = GameObject.Find(q + "." + (r+1) + "." + (s-1));
                        temp1 = GameObject.Find(q + "." + (r+2) + "." + (s-2));
                    break;

                    case 2:
                        temp = GameObject.Find((q-1) + "." + (r+1) + "." + s);
                        temp1 = GameObject.Find((q-2) + "." + (r+2) + "." + s);
                    break; 
                    
                    case 3:
                        temp = GameObject.Find((q-1) + "." + r + "." + (s+1));
                        temp1 = GameObject.Find((q-2) + "." + r + "." + (s+2));
                    break;
                    
                    case 4:
                        temp = GameObject.Find(q + "." + (r-1) + "." + (s+1));
                        temp1 = GameObject.Find(q + "." + (r-2) + "." + (s+2));
                    break;
                    
                    case 5:
                        temp = GameObject.Find((q+1) + "." + (r-1) + "." + s);
                        temp1 = GameObject.Find((q+2) + "." + (r-2) + "." + s);
                    break;
                    
                    case 6:
                        temp = GameObject.Find((q+1) + "." + r + "." + (s-1));
                        temp1 = GameObject.Find((q+2) + "." + r + "." + (s-2));
                    break;
                }

                if (!temp || !temp1)
                    return;

                if (IsMapEdgePositioning(temp)) // BTileZoro always must stay on map!!!!!!!!!!!!!!!!!!!
                    return;

                ResetBTiles(temp, temp1, GameHendler.Instance.CurrentHex);
            }
            break;
        }
    }
    #endregion

    // TODO
    public void CreateBuildingFromModel()
    {
        // Instantiate Building Object
        // Set all fields
        // Destroy model

        switch (buildingID)
        {
            case (int)IDconstants.IDturette: // Turette
            {
                GameObject go = GameObject.Instantiate(BuildingManager.Instance.turettePrefab, BTileZero.transform.position, Quaternion.identity);
                go.GetComponent<Turette>().Creation(this);
            }
            break;

            case (int)IDconstants.IDgarage: // Garage
            {
                GameObject go = GameObject.Instantiate(BuildingManager.Instance.garagePrefab, BTileZero.transform.position, Quaternion.identity);
                go.GetComponent<Garage>().Creation(this);
                go.name = "Garage" + Garage.garage_counter;
            }
            break;

            case (int)IDconstants.IDshieldGenerator: // Shield Generator
            {

            }
            break;

            case (int)IDconstants.IDgelShaft: // Gel Shaft
            {

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
        ResetModel(); // Delete model
    }
}
