using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaragePanel : MonoBehaviour
{
    [SerializeField]private Image buildingIcon;         // same for all garages
    
    [SerializeField]private Image garageComplectationStatus;
    [SerializeField]private Image garageComplectation1; // same
    [SerializeField]private Image garageComplectation2; // same
    [SerializeField]private Image garageComplectation3; // same
    [SerializeField]private Image garageComplectation4; // same
    [SerializeField]private Image garageComplectation5; // same

    [SerializeField]private Text buildingName;
    [SerializeField]private Slider garageHP;
    [SerializeField]private Slider garageSP;

    [SerializeField]private Button createUnitButton;      // activates refer to garageComplectation
    [SerializeField]private Button destroyBuildingButton; // always active

    private Garage garageRef = null;
    private int garageComplectationNumber = 0;

    private void Update()
    {
        // update sliders and images and buttons if building is hurt or unit dies
        
        // if (garageHP.value != garageRef.HealthPoints || garageSP.value != garageRef.ShieldPoints)
        // {
        //     HPSPSlidersReset();
        // }

        // if (garageRef.garageMembers.Count != garageComplectationNumber) // buttons change here
        // {
        //     ComplectationImagesReset();
        // }
    }

    public void ReloadPanel(Garage garage) // Reload all UI elements aquoting to garage object taken as parametr
    {
        garageRef = garage;

        buildingName.text = garage.gameObject.name;
        HPSPSlidersReset();
        ComplectationImagesReset();
        // buttons are the same except garageComplectation5 - in switch case we make it non interactible
    }

    private void HPSPSlidersReset()
    {
        garageHP.value = garageRef.HealthPoints;
        garageSP.value = garageRef.ShieldPoints;
    }

    private void ComplectationImagesReset()
    {
        switch (garageRef.garageMembers.Count)
        {
            case 1:
            {
                garageComplectationNumber = 1;
                garageComplectationStatus = garageComplectation1;
            }
            break;

            case 2:
            {
                garageComplectationNumber = 2;
                garageComplectationStatus = garageComplectation1;
            }
            break;
            
            case 3:
            {
                garageComplectationNumber = 3;
                garageComplectationStatus = garageComplectation1;
            }
            break;
            
            case 4:
            {
                garageComplectationNumber = 4;
                garageComplectationStatus = garageComplectation1;
            }
            break;
            
            case 5:
            {
                garageComplectationNumber = 5;
                garageComplectationStatus = garageComplectation1;
                // make button createUnitButton noninteractible
            }
            break;
        }
    }
}
