using UnityEngine;

public class BM_IdleState : ITouchState
{
    private RaycastHit2D hit;
    private bool isZooming = false;
    private bool isBuildingSelected = false;

    public ITouchState DoState()
    {
        DomyState();

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameHendler.Instance.buildingModel.RotateModel();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) && GameHendler.Instance.buildingModel.isModelPlacable)
        {
            Debug.Log("Create Building");
            GameHendler.Instance.buildingModel.CreateBuildingFromModel();
            GameHendler.Instance.ResetCurrentHexAndSelectedHex();
            return GameHendler.Instance.idleState;
        }


        if (isZooming) 
        {
            StateReset();
            return GameHendler.Instance.BM_zoomState;
        }

        else if (isBuildingSelected) 
        {
            StateReset();
            return GameHendler.Instance.BM_buildingMovementState;
        }

        else if ((!isBuildingSelected) && GameHendler.Instance.CurrentHex)
        {
            StateReset();
            return GameHendler.Instance.BM_cameraMovementState;
        }
        
        else 
            return GameHendler.Instance.BM_idleState;
    }

    private void DomyState()
    {
        if (Input.touchCount == 2) // Detects second Touch - ZoomState
        {
            isZooming = true;
            return;
        }
        
        if (Input.GetMouseButtonDown(0)) // Determine next state / loop until state change
        {
            GameHendler.Instance.touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Cashing mouse and camera position
            GameHendler.Instance.setCurrentHex(); // Find HEX under mouse/touch
            ModelSelection(); // If we press on Building
        }
    }
    
    private void ModelSelection()
    {
        hit = Physics2D.Raycast(GameHendler.Instance.redPoint.transform.position, Vector3.forward, 10f, GameHendler.Instance.BMidelLayerMask);
        
        Debug.Log(hit.collider.name);

        // if we hit sth that is model
        if (hit.collider != null && hit.collider.tag == TagConstants.modelTag)
        {
            isBuildingSelected = true;

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
        }
    }

    private void StateReset()
    {
        GameHendler.Instance.ResetCurrentHexAndSelectedHex();
        isBuildingSelected = false;
        isZooming = false;
    }
}

