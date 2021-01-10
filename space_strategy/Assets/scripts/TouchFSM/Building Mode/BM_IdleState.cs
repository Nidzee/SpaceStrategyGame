using UnityEngine;

public class BM_IdleState : ITouchState
{
    private RaycastHit2D hit;

    public ITouchState DoState()
    {
        DomyState();

        if (GameHendler.Instance.isZooming) // Zooming state
        {
            //Debug.Log("BM_zoomState");
            GameHendler.Instance.resetInfo();
            return GameHendler.Instance.BM_zoomState;
        }

        else if (GameHendler.Instance.isBuildingSelected) // if we hit Building
        {
            Debug.Log("BM_buildingMovementState");
            return GameHendler.Instance.BM_buildingMovementState;
        }

        else if((!GameHendler.Instance.isBuildingSelected) && GameHendler.Instance.CurrentHex) // if we dont hit Building but we hit HEX
        {
           // Debug.Log("BM_cameraMovementState");
            GameHendler.Instance.isCameraState = true;
            return GameHendler.Instance.BM_cameraMovementState;
        }

        else if((!GameHendler.Instance.isBuildingSelected) && (!GameHendler.Instance.CurrentHex) && (GameHendler.Instance.isFirstCollide) )// if we press but dont hit anything
        {
            Debug.Log("How did u do that? Thats literally impossible :)/nYo pressed outside the map!");
            return GameHendler.Instance.BM_idleState;
        }
        
        else 
            return GameHendler.Instance.BM_idleState; // Loop IdleState
    }

    private void DomyState()
    {
        if (Input.touchCount == 2) // Detects second Touch - ZoomState
        {
            GameHendler.Instance.isZooming = true;
            return;
        }
        
        if (Input.GetMouseButtonDown(0)) // Determine next state / loop until state change
        {
            // Cashing mouse and camera position
            GameHendler.Instance.touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            GameHendler.Instance.resetInfo();
            GameHendler.Instance.selectTileState.setCurrentHex(); // Find HEX under mouse/touch

            BuildingSelection(); // If we press on Building

            GameHendler.Instance.isFirstCollide = true;
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameHendler.Instance.buildingModel.RotateModel();
            GameHendler.Instance.ResetBuildingSpritePositions();
            GameHendler.Instance.BM_buildingMovementState.ChechForCorrectPlacement();
            
            // gameHendler.BuildingSprite.transform.position = gameHendler.buildingModel.BTileZero.transform.position;
            // if (gameHendler.buildingModel.BTileOne) 
            //     gameHendler.BuildingSprite1.transform.position = gameHendler.buildingModel.BTileOne.transform.position;
            // if (gameHendler.buildingModel.BTileTwo) 
            //     gameHendler.BuildingSprite2.transform.position = gameHendler.buildingModel.BTileTwo.transform.position;
        }
    }
    
    public void BuildingSelection()
    {
        hit = Physics2D.Raycast(GameHendler.Instance.redPoint.transform.position, Vector3.forward, 10f);
        
        // if we hit sth that is building
        // TODO hitiing the model - not the "Building(Clone)"
        if (hit.collider != null && hit.collider.name == "Building(Clone)" && !GameHendler.Instance.isFirstCollide)
        {
            GameHendler.Instance.isBuildingSelected = true;

            // Here we found 2 references. 
            // 1 - CurrentHex - it is a reference to hex on map (found by algorithm (GameObject.Finf()))
            // 2 - BTileX - it is a reference to hex on map but with mathematic founding methodic

            if (GameHendler.Instance.CurrentHex == GameHendler.Instance.buildingModel.BTileZero)
                GameHendler.Instance.buildingModel.BSelectedTileIndex = 0;

            else if (GameHendler.Instance.CurrentHex == GameHendler.Instance.buildingModel.BTileOne)
                GameHendler.Instance.buildingModel.BSelectedTileIndex = 1;

            else if (GameHendler.Instance.CurrentHex == GameHendler.Instance.buildingModel.BTileTwo)
                GameHendler.Instance.buildingModel.BSelectedTileIndex = 2;

            else
                Debug.Log("Error!");
            
            Debug.Log(GameHendler.Instance.buildingModel.BSelectedTileIndex); // DEBUG

            //gameHendler.buildingColor = gameHendler.BuildingSprite.GetComponent<SpriteRenderer>().color; // TEMP
        }
    }
}

