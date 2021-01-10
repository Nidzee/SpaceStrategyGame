using UnityEngine;

public class BM_IdleState : ITouchState
{
    public RaycastHit2D hit;

    public ITouchState DoState(GameHendler gh)
    {
        DomyState(gh);

        if (Input.GetKeyDown(KeyCode.R))
        {
            gh.buildingModel.RotateModel();
            gh.ResetBuildingSpritePositions();
            gh.BM_buildingMovementState.ChechForCorrectPlacement(gh);
            
            // gh.BuildingSprite.transform.position = gh.buildingModel.BTileZero.transform.position;
            // if (gh.buildingModel.BTileOne) 
            //     gh.BuildingSprite1.transform.position = gh.buildingModel.BTileOne.transform.position;
            // if (gh.buildingModel.BTileTwo) 
            //     gh.BuildingSprite2.transform.position = gh.buildingModel.BTileTwo.transform.position;
        }

        if (gh.isZooming) // Zooming state
        {
            Debug.Log("BM_zoomState");
            return gh.BM_zoomState;
        }

        else if (gh.isBuildingSelected) // if we hit Building
        {
            Debug.Log("BM_buildingMovementState");
            return gh.BM_buildingMovementState;
        }

        else if((!gh.isBuildingSelected) && gh.CurrentHex) // if we dont hit Building but we hit HEX
        {
            Debug.Log("BM_cameraMovementState");
            gh.isCameraState = false;
            return gh.BM_cameraMovementState;
        }

        else if((!gh.isBuildingSelected) && (!gh.CurrentHex) && (gh.isFirstCollide) )// if we press but dont hit anything
        {
            Debug.Log("How did u do that? Thats literally impossible :)/nYo press outside the map!");
            return gh.BM_idleState;
        }
        
        else return gh.BM_idleState; // Loop IdleState
    }

    private void DomyState(GameHendler gh)
    {
        if(Input.touchCount == 2) // Detects second Touch - ZoomState
        {
            gh.isZooming = true;
            gh.resetInfo();
            return;
        }
        
        if (Input.GetMouseButtonDown(0)) // Determine next state / loop until state change
        {
            // Cashing mouse and camera position
            gh.touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            gh.resetInfo();

            gh.selectTileState.setCurrentHex(gh); // Find HEX under mouse/touch

            BuildingSelection(gh); // If we press on Building

            gh.isFirstCollide = true;
        }
    }
    
    public void BuildingSelection(GameHendler gh)
    {
        hit = Physics2D.Raycast(gh.redPoint.transform.position, Vector3.forward, 10f);
            
        if (hit.collider != null && hit.collider.name == "Building(Clone)" && !gh.isFirstCollide)
        {
            gh.isBuildingSelected = true;

            if (gh.CurrentHex == gh.buildingModel.BTileZero)
            {
                gh.buildingModel.BSelectedTileIndex = 0;
            }
            else if (gh.CurrentHex == gh.buildingModel.BTileOne)
            {
                gh.buildingModel.BSelectedTileIndex = 1;
            }
            else if (gh.CurrentHex == gh.buildingModel.BTileTwo)
            {
                gh.buildingModel.BSelectedTileIndex = 2;
            }
            
            Debug.Log(gh.buildingModel.BSelectedTileIndex);

            //gh.buildingColor = gh.BuildingSprite.GetComponent<SpriteRenderer>().color; // TEMP
        }
    }
}

