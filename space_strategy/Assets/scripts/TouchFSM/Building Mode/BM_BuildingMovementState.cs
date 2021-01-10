using UnityEngine;

public class BM_BuildingMovementState : ITouchState
{
    public ITouchState DoState()
    {
        DomyState();

        if (GameHendler.Instance.isZooming)
        {
            GameHendler.Instance.resetInfo();
            return GameHendler.Instance.BM_zoomState;
        }

        else if (!GameHendler.Instance.isBuildingSelected)
            return GameHendler.Instance.BM_idleState;
        
        else
            return GameHendler.Instance.BM_buildingMovementState;
    }

    private void DomyState()
    {
        if (Input.touchCount == 2)
        {
            GameHendler.Instance.isZooming = true;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            BuildingMovement();
            ChechForCorrectPlacement();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StateReset();
        }
    }

    private void BuildingMovement() // Movement in Building Mode
    {
        GameHendler.Instance.selectTileState.setCurrentHex();

        switch (GameHendler.Instance.buildingModel.buildingType)
        {
            case BuildingType.SingleTileBuilding:
                SingleTileBuildingMovement();
            break;

            case BuildingType.DoubleTileBuilding:
                DoubleTileBuilding();
            break;

            case BuildingType.TripleTileBuilding:
                TripleTileBuilding();
            break;

            default : 
                Debug.Log("ERROR!");
            break;
        }

        GameHendler.Instance.ResetBuildingSpritePositions(); // DEBUG
    }

    private void SingleTileBuildingMovement()
    {
        if (IsMapEdgePositioning(GameHendler.Instance.CurrentHex))  // BTileZoro always must stay on map!!!!!!!!!!!!!!!!!!!
             return;

        GameHendler.Instance.buildingModel.ResetBTiles(GameHendler.Instance.CurrentHex); // We pass BtileZero
    }

    private void DoubleTileBuilding()
    {
        GameObject temp = null; // Helper for calculating
        int q = (GameHendler.Instance.CurrentHex.GetComponent<Hex>().Q);
        int r = (GameHendler.Instance.CurrentHex.GetComponent<Hex>().R);
        int s = (GameHendler.Instance.CurrentHex.GetComponent<Hex>().S);

        // Which Tile we hit INDEX
        switch (GameHendler.Instance.buildingModel.BSelectedTileIndex)
        {
            case 0:
            {
                if (IsMapEdgePositioning(GameHendler.Instance.CurrentHex))  // BTileZoro always must stay on map!!!!!!!!!!!!!!!!!!!
                    return;
                    
                // Finding Hex *temp* on map
                switch (GameHendler.Instance.buildingModel.rotation)
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

                GameHendler.Instance.buildingModel.ResetBTiles(GameHendler.Instance.CurrentHex, temp);
            }
            break;

            case 1:
            {
                switch (GameHendler.Instance.buildingModel.rotation)
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
                
                GameHendler.Instance.buildingModel.ResetBTiles(temp, GameHendler.Instance.CurrentHex);
            }
            break;
        }
    }

    private void TripleTileBuilding()
    {
        GameObject temp = null;
        GameObject temp1 = null;

        int q = (GameHendler.Instance.CurrentHex.GetComponent<Hex>().Q);
        int r = (GameHendler.Instance.CurrentHex.GetComponent<Hex>().R);
        int s = (GameHendler.Instance.CurrentHex.GetComponent<Hex>().S);

        switch(GameHendler.Instance.buildingModel.BSelectedTileIndex)
        {
            case 0:
            {
                if (IsMapEdgePositioning(GameHendler.Instance.CurrentHex))  // BTileZoro always must stay on map!!!!!!!!!!!!!!!!!!!
                    return;
                    
                switch (GameHendler.Instance.buildingModel.rotation)
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

                GameHendler.Instance.buildingModel.ResetBTiles(GameHendler.Instance.CurrentHex, temp, temp1);
            }
            break;
            
            case 1:
            {
                switch (GameHendler.Instance.buildingModel.rotation)
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

                GameHendler.Instance.buildingModel.ResetBTiles(temp, GameHendler.Instance.CurrentHex, temp1);
            }
            break;
            
            case 2:
            {
                switch (GameHendler.Instance.buildingModel.rotation)
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

                GameHendler.Instance.buildingModel.ResetBTiles(temp, temp1, GameHendler.Instance.CurrentHex);
            }
            break;
        }
    }

    // Check for Map Edge
    private bool IsMapEdgePositioning(GameObject hex)
    {
        return (hex.GetComponent<Hex>().tile_Type == Tile_Type.MapEdge);
    }

    public void ChechForCorrectPlacement()
    {
        switch (GameHendler.Instance.buildingModel.buildingType)
        {
            case BuildingType.SingleTileBuilding:
            {
                if (GameHendler.Instance.buildingModel.BTileZero.GetComponent<Hex>().tile_Type == GameHendler.Instance.buildingModel.PlacingTile)
                {
                    // Add shader GREEN and set bool to true
                    Debug.Log("GREEN SHADER");
                }
                else
                {
                    // Add shader RED and set bool to false
                    Debug.Log("RED SHADER");
                }
            }
            break;
            
            case BuildingType.DoubleTileBuilding:
            {
                if (GameHendler.Instance.buildingModel.buildingID == (int)IDconstants.IDgelShaft)
                {
                    if (GameHendler.Instance.buildingModel.BTileZero.GetComponent<Hex>().tile_Type == GameHendler.Instance.buildingModel.PlacingTile &&
                        GameHendler.Instance.buildingModel.BTileOne.GetComponent<Hex>().tile_Type == GameHendler.Instance.buildingModel.PlacingTile_Optional)
                    {
                        // Add shader GREEN and set bool to true
                        Debug.Log("GREEN SHADER");
                    }
                    else
                    {
                        // Add shader RED and set bool to false
                        Debug.Log("RED SHADER");
                    }
                }
                else if (GameHendler.Instance.buildingModel.BTileZero.GetComponent<Hex>().tile_Type == GameHendler.Instance.buildingModel.PlacingTile &&
                    GameHendler.Instance.buildingModel.BTileOne.GetComponent<Hex>().tile_Type == GameHendler.Instance.buildingModel.PlacingTile)
                {
                    // Add shader GREEN and set bool to true
                    Debug.Log("GREEN SHADER");
                }
                else
                {
                    // Add shader RED and set bool to false
                    Debug.Log("RED SHADER");
                }
            }
            break;
            
            case BuildingType.TripleTileBuilding:
            {
                if (GameHendler.Instance.buildingModel.BTileZero.GetComponent<Hex>().tile_Type == GameHendler.Instance.buildingModel.PlacingTile &&
                    GameHendler.Instance.buildingModel.BTileOne.GetComponent<Hex>().tile_Type == GameHendler.Instance.buildingModel.PlacingTile && 
                    GameHendler.Instance.buildingModel.BTileTwo.GetComponent<Hex>().tile_Type == GameHendler.Instance.buildingModel.PlacingTile)
                {
                    // Add shader GREEN and set bool to true
                    Debug.Log("GREEN SHADER");
                }
                else
                {
                    // Add shader RED and set bool to false
                    Debug.Log("RED SHADER");
                }
            }
            break;
        }
    }

    private void StateReset() // Reseting all used variables
    {
        GameHendler.Instance.buildingModel.BSelectedTileIndex = 0; // For understanding, which Tile is Selected(BuildingTile)

        GameHendler.Instance.isBuildingSelected = false; 
        GameHendler.Instance.isFirstCollide = false;
        GameHendler.Instance.CurrentHex = null;
    }

}







// ADD COLOR CHECK OR ADD SHADER!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


        // GameHendler.Instance.selectTileState.setCurrentHex(GameHendler.Instance);
        // if (GameHendler.Instance.CurrentHex) 
        // {
        //     GameHendler.Instance.BuildingSprite.transform.position = GameHendler.Instance.CurrentHex.transform.position;

        //     if (GameHendler.Instance.BuildingSprite.GetComponent<TestingBuilding>().placingTile != GameHendler.Instance.CurrentHex.GetComponent<Hex>().tile_Type)
        //     {
        //         // Add posibilitie not to set building on map here
        //         Debug.Log("Black");
        //         GameHendler.Instance.BuildingSprite.GetComponent<SpriteRenderer>().color = Color.black;
        //     }
        //     else
        //     {
        //         GameHendler.Instance.BuildingSprite.GetComponent<SpriteRenderer>().color = GameHendler.Instance.buildingColor;
        //     }
        // }
