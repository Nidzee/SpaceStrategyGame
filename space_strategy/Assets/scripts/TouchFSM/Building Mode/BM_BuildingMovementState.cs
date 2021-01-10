using UnityEngine;

public class BM_BuildingMovementState : ITouchState
{
    public ITouchState DoState(GameHendler gh)
    {
        DomyState(gh);

        if (gh.isZooming)
            return gh.BM_zoomState;

        else if (!gh.isBuildingSelected)
            return gh.BM_idleState;
        
        else
            return gh.BM_buildingMovementState;
    }

    private void DomyState(GameHendler gh)
    {
        if(Input.touchCount == 2)
        {
            gh.isZooming = true;
            gh.resetInfo(); // In here are all *ResetState* variables
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Building_movement(gh);
            ChechForCorrectPlacement(gh);//////////////////////////////////////////////////////
        }

        if (Input.GetMouseButtonUp(0))
        {
            //gh.BM_BuildingIsSet = true;
            StateReset(gh);
        }
    }

    private void StateReset(GameHendler gh) // Reseting all used variables
    {
        gh.buildingModel.BSelectedTileIndex = 0; // For understanding, which Tile is Selected(BuildingTile)

        gh.isBuildingSelected = false; 
        gh.isFirstCollide = false;
        gh.CurrentHex = null;
    }

    private void Building_movement(GameHendler gh) // Movement in Building Mode
    {
        gh.selectTileState.setCurrentHex(gh);

        switch (gh.buildingModel.buildingType)
        {
            case BuildingType.SingleTileBuilding:
                SingleTileBuildingMovement(gh);
            break;

            case BuildingType.DoubleTileBuilding:
                DoubleTileBuilding(gh);
            break;

            case BuildingType.TripleTileBuilding:
                TripleTileBuilding(gh);
            break;
        }

        gh.ResetBuildingSpritePositions();
    }

    private void SingleTileBuildingMovement(GameHendler gh)
    {
        if (IsMapEdgePositioning(gh.CurrentHex))
             return;

        gh.buildingModel.ResetBTiles(gh.CurrentHex);
    }

    private void DoubleTileBuilding(GameHendler gh)
    {
        GameObject temp = null;
        int q = (gh.CurrentHex.GetComponent<Hex>().Q);
        int r = (gh.CurrentHex.GetComponent<Hex>().R);
        int s = (gh.CurrentHex.GetComponent<Hex>().S);

        // Which Tile we hit
        switch(gh.buildingModel.BSelectedTileIndex)
        {
            case 0:
                if (IsMapEdgePositioning(gh.CurrentHex))
                    return;
                    
                // Finding Hex *temp* on map
                switch (gh.buildingModel.rotation)
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

                gh.buildingModel.ResetBTiles(gh.CurrentHex, temp);
            break;

            case 1:
                switch (gh.buildingModel.rotation)
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
                
                if (IsMapEdgePositioning(temp))
                    return;
                
                gh.buildingModel.ResetBTiles(temp, gh.CurrentHex);
            break;
        }
    }

    private void TripleTileBuilding(GameHendler gh)
    {
        GameObject temp = null;
        GameObject temp1 = null;

        int q = (gh.CurrentHex.GetComponent<Hex>().Q);
        int r = (gh.CurrentHex.GetComponent<Hex>().R);
        int s = (gh.CurrentHex.GetComponent<Hex>().S);

        switch(gh.buildingModel.BSelectedTileIndex)
        {
            case 0:
            {
                if (IsMapEdgePositioning(gh.CurrentHex))
                    return;
                    
                switch (gh.buildingModel.rotation)
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

                gh.buildingModel.ResetBTiles(gh.CurrentHex, temp, temp1);
            }
            break;
            
            case 1:
            {
                switch (gh.buildingModel.rotation)
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

                if (IsMapEdgePositioning(temp))
                    return;

                gh.buildingModel.ResetBTiles(temp, gh.CurrentHex, temp1);
            }
            break;
            
            case 2:
            {
                switch (gh.buildingModel.rotation)
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

                if (IsMapEdgePositioning(temp))
                    return;

                gh.buildingModel.ResetBTiles(temp, temp1, gh.CurrentHex);
            }
            break;
        }
    }

    // Check for Map Edge
    private bool IsMapEdgePositioning(GameObject hex)
    {
        return (hex.GetComponent<Hex>().tile_Type == Tile_Type.MapEdge);
    }

    public void ChechForCorrectPlacement(GameHendler gh)
    {
        switch (gh.buildingModel.buildingType)
        {
            case BuildingType.SingleTileBuilding:
            {
                if (gh.buildingModel.BTileZero.GetComponent<Hex>().tile_Type == gh.buildingModel.PlacingTile)
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
                if (gh.buildingModel.buildingID == (int)IDconstants.IDgelShaft)
                {
                    if(gh.buildingModel.BTileZero.GetComponent<Hex>().tile_Type == gh.buildingModel.PlacingTile &&
                        gh.buildingModel.BTileOne.GetComponent<Hex>().tile_Type == gh.buildingModel.PlacingTile_Optional)
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
                else if (gh.buildingModel.BTileZero.GetComponent<Hex>().tile_Type == gh.buildingModel.PlacingTile &&
                    gh.buildingModel.BTileOne.GetComponent<Hex>().tile_Type == gh.buildingModel.PlacingTile)
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
                if (gh.buildingModel.BTileZero.GetComponent<Hex>().tile_Type == gh.buildingModel.PlacingTile &&
                    gh.buildingModel.BTileOne.GetComponent<Hex>().tile_Type == gh.buildingModel.PlacingTile && 
                    gh.buildingModel.BTileTwo.GetComponent<Hex>().tile_Type == gh.buildingModel.PlacingTile)
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

}







// ADD COLOR CHECK OR ADD SHADER!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


        // gh.selectTileState.setCurrentHex(gh);
        // if (gh.CurrentHex) 
        // {
        //     gh.BuildingSprite.transform.position = gh.CurrentHex.transform.position;

        //     if (gh.BuildingSprite.GetComponent<TestingBuilding>().placingTile != gh.CurrentHex.GetComponent<Hex>().tile_Type)
        //     {
        //         // Add posibilitie not to set building on map here
        //         Debug.Log("Black");
        //         gh.BuildingSprite.GetComponent<SpriteRenderer>().color = Color.black;
        //     }
        //     else
        //     {
        //         gh.BuildingSprite.GetComponent<SpriteRenderer>().color = gh.buildingColor;
        //     }
        // }
