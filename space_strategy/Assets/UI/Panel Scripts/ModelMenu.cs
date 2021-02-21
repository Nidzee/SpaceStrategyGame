using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ModelMenu : MonoBehaviour
{
    [SerializeField] private Button placeButton;

    private bool helper = false;


    // Button activation managment /////////////////////////////////////////////////////////////////////////      CAN BE IMPROVED       !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    private void Update()
    {
        if (!GameHendler.Instance.buildingModel.isModelPlacable && !helper)
        {
            helper = true;
            placeButton.interactable = false;
        }
        else if (GameHendler.Instance.buildingModel.isModelPlacable && helper)
        {
            helper = false;
            placeButton.interactable = true;
        }
    }


    // Rotate model
    public void RotateModel()
    {
        GameHendler.Instance.buildingModel.RotateModel();
    }


    // Place model
    public void PlaceBuilding()
    {
        GameHendler.Instance.buildingModel.CreateBuildingFromModel();
        GameHendler.Instance.ResetCurrentHexAndSelectedHex();

        GameHendler.Instance.currentState = GameHendler.Instance.idleState;


        // Delete resources here
        ResourceManager.Instance.DeleteResourcesAfterAction();



        Debug.Log("Create Building");
        UIPannelManager.Instance.ResetPanels("GameView");
    }


    // Cancel building process
    public void Cancel()
    {
        GameHendler.Instance.currentState = GameHendler.Instance.idleState;
        GameHendler.Instance.buildingModel.ResetModel();

        UIPannelManager.Instance.ResetPanels("GameView");
    }
}
