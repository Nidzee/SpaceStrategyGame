using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance {get; private set;}



    #region Shtab

        public static int _crystalNeedForExpand_ForPerks;
        public static int _ironNeedForForExpand_ForPerks;
        public static int _gelNeedForForExpand_ForPerks;

        public static int _crystalNeedForExpand_ToLvl2_Shtab;
        public static int _ironNeedForForExpand_ToLvl2_Shtab;
        public static int _gelNeedForForExpand_ToLvl2_Shtab;

        public static int _crystalNeedForExpand_ToLvl3_Shtab;
        public static int _ironNeedForForExpand_ToLvl3_Shtab;
        public static int _gelNeedForForExpand_ToLvl3_Shtab;

        public static int _maxHealth_Lvl1_Shtab; 
        public static int _maxHealth_Lvl2_Shtab; 
        public static int _maxHealth_Lvl3_Shtab;

        public static int _maxShiled_Lvl1_Shtab; 
        public static int _maxShiled_Lvl2_Shtab; 
        public static int _maxShiled_Lvl3_Shtab;

        public static int _deffencePoints_Lvl1_Shtab; 
        public static int _deffencePoints_Lvl2_Shtab; 
        public static int _deffencePoints_Lvl3_Shtab;


        public static void InitStaticFields()
        {
            _crystalNeedForExpand_ForPerks = 20;
            _ironNeedForForExpand_ForPerks = 20;
            _gelNeedForForExpand_ForPerks = 20;

            _crystalNeedForExpand_ToLvl2_Shtab = 20;
            _ironNeedForForExpand_ToLvl2_Shtab = 20;
            _gelNeedForForExpand_ToLvl2_Shtab = 20;

            _crystalNeedForExpand_ToLvl3_Shtab = 30;
            _ironNeedForForExpand_ToLvl3_Shtab = 30;
            _gelNeedForForExpand_ToLvl3_Shtab = 30;

            _maxHealth_Lvl1_Shtab = 200; 
            _maxHealth_Lvl2_Shtab = 300; 
            _maxHealth_Lvl3_Shtab = 400;

            _maxShiled_Lvl1_Shtab = 200; 
            _maxShiled_Lvl2_Shtab = 300; 
            _maxShiled_Lvl3_Shtab = 400;

            _deffencePoints_Lvl1_Shtab = 10; 
            _deffencePoints_Lvl2_Shtab = 15; 
            _deffencePoints_Lvl3_Shtab = 20;
        }

        public static void GetResourcesNeedToUpgrade___Shtab(out int crystalNeed, out int ironNeed, out int gelNeed)
        {
            if (ResourceManager.Instance.shtabReference.level == 1)
            {
                crystalNeed = _crystalNeedForExpand_ToLvl2_Shtab;
                ironNeed = _ironNeedForForExpand_ToLvl2_Shtab;
                gelNeed = _gelNeedForForExpand_ToLvl2_Shtab;
            }
            else
            {
                crystalNeed = _crystalNeedForExpand_ToLvl3_Shtab;
                ironNeed = _ironNeedForForExpand_ToLvl3_Shtab;
                gelNeed = _gelNeedForForExpand_ToLvl3_Shtab;
            }
        }

        public static void GetResourcesToBuyPerks(out int crystalNeed, out int ironNeed, out int gelNeed)
        {
            crystalNeed = _crystalNeedForExpand_ForPerks;
            ironNeed = _ironNeedForForExpand_ForPerks;
            gelNeed = _gelNeedForForExpand_ForPerks;
        }

        public static void InitCost_ForPerks()
        {
            ShtabStaticData.baseMenuReference._buyPerksButton.GetComponentInChildren<Text>().text = 
            _crystalNeedForExpand_ForPerks.ToString() + " " + 
            _ironNeedForForExpand_ForPerks.ToString() + " " + 
            _gelNeedForForExpand_ForPerks.ToString();
        }

        public static void InitCost_ToLvl2___Shtab()
        {
            ShtabStaticData.baseMenuReference._upgradeButton.GetComponentInChildren<Text>().text = 
            _crystalNeedForExpand_ToLvl2_Shtab.ToString() + " " + 
            _ironNeedForForExpand_ToLvl2_Shtab.ToString() + " " + 
            _gelNeedForForExpand_ToLvl2_Shtab.ToString();
        }

        public static void InitCost_ToLvl3___Shtab()
        {
            ShtabStaticData.baseMenuReference._upgradeButton.GetComponentInChildren<Text>().text = 
            _crystalNeedForExpand_ToLvl3_Shtab.ToString() + " " + 
            _ironNeedForForExpand_ToLvl3_Shtab.ToString() + " " + 
            _gelNeedForForExpand_ToLvl3_Shtab.ToString();
        }

    #endregion

    #region Garage

        public static int _crystalNeedForBuilding_Garage;
        public static int _ironNeedForBuilding_Garage;
        public static int _gelNeedForBuilding_Garage;

        public static int _crystalNeedForUnitCreation;
        public static int _ironNeedForForUnitCreation;
        public static int _gelNeedForForUnitCreation;



        public static int _maxHealth_Garage_Base_Lvl_1; 
        public static int _maxShiled_Garage_Base_Lvl_1; 
        public static int _maxDeffensePoints_Garage_Base_Lvl_1; 

        public static int _maxHealth_Garage_Base_Lvl_2; 
        public static int _maxShiled_Garage_Base_Lvl_2; 
        public static int _maxDeffensePoints_Garage_Base_Lvl_2; 

        public static int _maxHealth_Garage_Base_Lvl_3; 
        public static int _maxShiled_Garage_Base_Lvl_3; 
        public static int _maxDeffensePoints_Garage_Base_Lvl_3; 

        private void InitStats___Garage()
        {
            _crystalNeedForUnitCreation = 5;
            _ironNeedForForUnitCreation = 5;
            _gelNeedForForUnitCreation = 5;

            _crystalNeedForBuilding_Garage = 10;
            _ironNeedForBuilding_Garage = 10;
            _gelNeedForBuilding_Garage = 10;


            
            _maxHealth_Garage_Base_Lvl_1 = 100; 
            _maxShiled_Garage_Base_Lvl_1 = 100; 
            _maxDeffensePoints_Garage_Base_Lvl_1 = 10; 

            _maxHealth_Garage_Base_Lvl_2 = 200; 
            _maxShiled_Garage_Base_Lvl_2 = 200; 
            _maxDeffensePoints_Garage_Base_Lvl_2 = 20; 

            _maxHealth_Garage_Base_Lvl_3 = 300; 
            _maxShiled_Garage_Base_Lvl_3 = 300; 
            _maxDeffensePoints_Garage_Base_Lvl_3 = 30; 

        }











        public static string GetResourcesNeedToBuildAsText___Garage()
        {
            return _crystalNeedForBuilding_Garage.ToString() + " " + _ironNeedForBuilding_Garage.ToString() +" "+_gelNeedForBuilding_Garage.ToString();
        }
        
        public static void InitUnitCreationCost___Garage() // Initializing only once
        {
            GarageStaticData.garageMenuReference.InitUnitCostButton(_crystalNeedForUnitCreation, _ironNeedForForUnitCreation, _gelNeedForForUnitCreation);
        }

        public static void GetResourcesNeedToBuild___Garage(out int crystalNeed, out int ironNeed, out int gelNeed)
        {
            crystalNeed = _crystalNeedForBuilding_Garage;
            ironNeed = _ironNeedForBuilding_Garage;
            gelNeed = _gelNeedForBuilding_Garage;
        }

        public static void GetResourcesNeedToCreateUnit___Garage(out int crystalNeed, out int ironNeed, out int gelNeed)
        {
            crystalNeed = _crystalNeedForUnitCreation;
            ironNeed = _ironNeedForForUnitCreation;
            gelNeed = _gelNeedForForUnitCreation;
        }









    #endregion

    #region MineShaft

        public static int _crystalNeedForBuilding_Shaft;
        public static int _ironNeedForBuilding_Shaft;
        public static int _gelNeedForBuilding_Shaft;

        public static int _crystalNeedForExpand_ToLvl2_Shaft;
        public static int _ironNeedForForExpand_ToLvl2_Shaft;
        public static int _gelNeedForForExpand_ToLvl2_Shaft;

        public static int _crystalNeedForExpand_ToLvl3_Shaft;
        public static int _ironNeedForForExpand_ToLvl3_Shaft;
        public static int _gelNeedForForExpand_ToLvl3_Shaft;







        public static int _maxHealth_Lvl1_Shaft_Base_Lvl_1;
        public static int _maxHealth_Lvl1_Shaft_Base_Lvl_2;
        public static int _maxHealth_Lvl1_Shaft_Base_Lvl_3;

        public static int _maxHealth_Lvl2_Shaft_Base_Lvl_1; 
        public static int _maxHealth_Lvl2_Shaft_Base_Lvl_2; 
        public static int _maxHealth_Lvl2_Shaft_Base_Lvl_3;

        public static int _maxHealth_Lvl3_Shaft_Base_Lvl_1;
        public static int _maxHealth_Lvl3_Shaft_Base_Lvl_2;
        public static int _maxHealth_Lvl3_Shaft_Base_Lvl_3;


        public static int _maxShiled_Lvl1_Shaft_Base_Lvl_1; 
        public static int _maxShiled_Lvl1_Shaft_Base_Lvl_2; 
        public static int _maxShiled_Lvl1_Shaft_Base_Lvl_3; 

        public static int _maxShiled_Lvl2_Shaft_Base_Lvl_1; 
        public static int _maxShiled_Lvl2_Shaft_Base_Lvl_2; 
        public static int _maxShiled_Lvl2_Shaft_Base_Lvl_3; 
        
        public static int _maxShiled_Lvl3_Shaft_Base_Lvl_1;
        public static int _maxShiled_Lvl3_Shaft_Base_Lvl_2;
        public static int _maxShiled_Lvl3_Shaft_Base_Lvl_3;


        public static int _defensePoints_Lvl1_Shaft_Base_Lvl_1; 
        public static int _defensePoints_Lvl1_Shaft_Base_Lvl_2; 
        public static int _defensePoints_Lvl1_Shaft_Base_Lvl_3;

        public static int _defensePoints_Lvl2_Shaft_Base_Lvl_1; 
        public static int _defensePoints_Lvl2_Shaft_Base_Lvl_2; 
        public static int _defensePoints_Lvl2_Shaft_Base_Lvl_3;

        public static int _defensePoints_Lvl3_Shaft_Base_Lvl_1; 
        public static int _defensePoints_Lvl3_Shaft_Base_Lvl_2; 
        public static int _defensePoints_Lvl3_Shaft_Base_Lvl_3;



        public static void InitAllStaticFields___MineShaft()
        {
            _crystalNeedForBuilding_Shaft = 5;
            _ironNeedForBuilding_Shaft = 5;
            _gelNeedForBuilding_Shaft = 5;

            _crystalNeedForExpand_ToLvl2_Shaft = 10;
            _ironNeedForForExpand_ToLvl2_Shaft = 10;
            _gelNeedForForExpand_ToLvl2_Shaft = 10;

            _crystalNeedForExpand_ToLvl3_Shaft = 15;
            _ironNeedForForExpand_ToLvl3_Shaft = 15;
            _gelNeedForForExpand_ToLvl3_Shaft = 15;




                
            _maxHealth_Lvl1_Shaft_Base_Lvl_1 = 100;
            _maxHealth_Lvl1_Shaft_Base_Lvl_2 = 125;
            _maxHealth_Lvl1_Shaft_Base_Lvl_3 = 150;

            _maxHealth_Lvl2_Shaft_Base_Lvl_1 = 200; 
            _maxHealth_Lvl2_Shaft_Base_Lvl_2 = 225; 
            _maxHealth_Lvl2_Shaft_Base_Lvl_3 = 250;

            _maxHealth_Lvl3_Shaft_Base_Lvl_1 = 300;
            _maxHealth_Lvl3_Shaft_Base_Lvl_2 = 325;
            _maxHealth_Lvl3_Shaft_Base_Lvl_3 = 350;


            _maxShiled_Lvl1_Shaft_Base_Lvl_1 = 100; 
            _maxShiled_Lvl1_Shaft_Base_Lvl_2 = 125; 
            _maxShiled_Lvl1_Shaft_Base_Lvl_3 = 150; 

            _maxShiled_Lvl2_Shaft_Base_Lvl_1 = 200; 
            _maxShiled_Lvl2_Shaft_Base_Lvl_2 = 225; 
            _maxShiled_Lvl2_Shaft_Base_Lvl_3 = 250; 
            
            _maxShiled_Lvl3_Shaft_Base_Lvl_1 = 300;
            _maxShiled_Lvl3_Shaft_Base_Lvl_2 = 325;
            _maxShiled_Lvl3_Shaft_Base_Lvl_3 = 350;


            _defensePoints_Lvl1_Shaft_Base_Lvl_1 = 10; 
            _defensePoints_Lvl1_Shaft_Base_Lvl_2 = 11; 
            _defensePoints_Lvl1_Shaft_Base_Lvl_3 = 12;

            _defensePoints_Lvl2_Shaft_Base_Lvl_1 = 20; 
            _defensePoints_Lvl2_Shaft_Base_Lvl_2 = 21; 
            _defensePoints_Lvl2_Shaft_Base_Lvl_3 = 22;

            _defensePoints_Lvl3_Shaft_Base_Lvl_1 = 30; 
            _defensePoints_Lvl3_Shaft_Base_Lvl_2 = 31; 
            _defensePoints_Lvl3_Shaft_Base_Lvl_3 = 32;

        }













        public static string GetResourcesNeedToBuildAsText___MineShaft()
        {
            return StatsManager._crystalNeedForBuilding_Shaft.ToString() 
            + " " +StatsManager._ironNeedForBuilding_Shaft.ToString() 
            + " " +StatsManager._gelNeedForBuilding_Shaft.ToString();
        }

        public static void GetResourcesNeedToBuild___MineShaft(out int crystalNeed, out int ironNeed, out int gelNeed)
        {
            crystalNeed = _crystalNeedForBuilding_Shaft;
            ironNeed = _ironNeedForBuilding_Shaft;
            gelNeed = _gelNeedForBuilding_Shaft;
        }

        public static void InitCost_ToLvl2()
        {
            MineShaftStaticData.shaftMenuReference._upgradeButton.GetComponentInChildren<Text>().text = 
                    _crystalNeedForExpand_ToLvl2_Shaft.ToString() 
            + " " + _ironNeedForForExpand_ToLvl2_Shaft.ToString() 
            + " " + _gelNeedForForExpand_ToLvl2_Shaft.ToString();
        }

        public static void InitCost_ToLvl3()
        {
            MineShaftStaticData.shaftMenuReference._upgradeButton.GetComponentInChildren<Text>().text = 
                    _crystalNeedForExpand_ToLvl3_Shaft.ToString() 
            + " " + _ironNeedForForExpand_ToLvl3_Shaft.ToString() 
            + " " + _gelNeedForForExpand_ToLvl3_Shaft.ToString();
        }














    #endregion

    #region Power Plant

        public static int _crystalNeedForBuilding_PowerPlant;
        public static int _ironNeedForBuilding_PowerPlant;
        public static int _gelNeedForBuilding_PowerPlant;


        public static int _maxHealth_PowerPlant_Base_Lvl_1; 
        public static int _maxShiled_PowerPlant_Base_Lvl_1; 
        public static int _maxDeffensePoints_PowerPlant_Base_Lvl_1; 

        public static int _maxHealth_PowerPlant_Base_Lvl_2; 
        public static int _maxShiled_PowerPlant_Base_Lvl_2; 
        public static int _maxDeffensePoints_PowerPlant_Base_Lvl_2; 

        public static int _maxHealth_PowerPlant_Base_Lvl_3; 
        public static int _maxShiled_PowerPlant_Base_Lvl_3; 
        public static int _maxDeffensePoints_PowerPlant_Base_Lvl_3; 

        public static string GetResourcesNeedToBuildAsText___PowerPlant()
        {
            return _crystalNeedForBuilding_PowerPlant.ToString() + " " + _ironNeedForBuilding_PowerPlant.ToString() +" "+_gelNeedForBuilding_PowerPlant.ToString();
        }

        public static void GetResourcesNeedToBuild___PowerPlant(out int crystalNeed, out int ironNeed, out int gelNeed)
        {
            crystalNeed = _crystalNeedForBuilding_PowerPlant;
            ironNeed = _ironNeedForBuilding_PowerPlant;
            gelNeed = _gelNeedForBuilding_PowerPlant;
        }

        private static void InitStats___PowerPlant()
        {
            _crystalNeedForBuilding_PowerPlant = 25;
            _ironNeedForBuilding_PowerPlant = 25;
            _gelNeedForBuilding_PowerPlant = 25;

            _maxHealth_PowerPlant_Base_Lvl_1 = 100; 
            _maxShiled_PowerPlant_Base_Lvl_1 = 100; 
            _maxDeffensePoints_PowerPlant_Base_Lvl_1 = 10; 

            _maxHealth_PowerPlant_Base_Lvl_2 = 200; 
            _maxShiled_PowerPlant_Base_Lvl_2 = 200; 
            _maxDeffensePoints_PowerPlant_Base_Lvl_2 = 20; 

            _maxHealth_PowerPlant_Base_Lvl_3 = 300; 
            _maxShiled_PowerPlant_Base_Lvl_3 = 300; 
            _maxDeffensePoints_PowerPlant_Base_Lvl_3 = 30; 
        }

    #endregion

    #region Antenne

        public static int _crystalNeedForBuilding_Antenne;
        public static int _ironNeedForBuilding_Antenne;
        public static int _gelNeedForBuilding_Antenne;

        public static int _maxHealth_Antenne_Base_Lvl_1; 
        public static int _maxShiled_Antenne_Base_Lvl_1; 
        public static int _maxDeffensePoints_Antenne_Base_Lvl_1; 

        public static int _maxHealth_Antenne_Base_Lvl_2; 
        public static int _maxShiled_Antenne_Base_Lvl_2; 
        public static int _maxDeffensePoints_Antenne_Base_Lvl_2; 

        public static int _maxHealth_Antenne_Base_Lvl_3; 
        public static int _maxShiled_Antenne_Base_Lvl_3; 
        public static int _maxDeffensePoints_Antenne_Base_Lvl_3; 


        // Static info about building - determins all info about every object of this building class
        public static void InitStaticFields___Antenne()
        {
            _crystalNeedForBuilding_Antenne = 50;
            _ironNeedForBuilding_Antenne = 50;
            _gelNeedForBuilding_Antenne = 50;

            
            _maxHealth_Antenne_Base_Lvl_1 = 100; 
            _maxShiled_Antenne_Base_Lvl_1 = 100; 
            _maxDeffensePoints_Antenne_Base_Lvl_1 = 10; 

            _maxHealth_Antenne_Base_Lvl_2 = 200; 
            _maxShiled_Antenne_Base_Lvl_2 = 200; 
            _maxDeffensePoints_Antenne_Base_Lvl_2 = 20; 

            _maxHealth_Antenne_Base_Lvl_3 = 300; 
            _maxShiled_Antenne_Base_Lvl_3 = 300; 
            _maxDeffensePoints_Antenne_Base_Lvl_3 = 30; 
        }





        public static string GetResourcesNeedToBuildAsText___Antenne()
        {
            return _crystalNeedForBuilding_Antenne.ToString() + " " + _ironNeedForBuilding_Antenne.ToString() +" "+_gelNeedForBuilding_Antenne.ToString();
        }

        public static void GetResourcesNeedToBuild___Antenne(out int crystalNeed, out int ironNeed, out int gelNeed)
        {
            crystalNeed = _crystalNeedForBuilding_Antenne;
            ironNeed = _ironNeedForBuilding_Antenne;
            gelNeed = _gelNeedForBuilding_Antenne;
        }

    #endregion

    #region MTurret

        public static int _crystalNeedForBuilding_MisileTurret;
        public static int _ironNeedForBuilding_MisileTurret;
        public static int _gelNeedForBuilding_MisileTurret;

        public static int _crystalNeedForExpand_ToLvl2_MisileTurret;
        public static int _ironNeedForForExpand_ToLvl2_MisileTurret;
        public static int _gelNeedForForExpand_ToLvl2_MisileTurret;

        public static int _crystalNeedForExpand_ToLvl3_MisileTurret;
        public static int _ironNeedForForExpand_ToLvl3_MisileTurret;
        public static int _gelNeedForForExpand_ToLvl3_MisileTurret;








        public static int _maxHealth_Lvl1_MisileTurret_Base_Lvl_1;
        public static int _maxHealth_Lvl1_MisileTurret_Base_Lvl_2;
        public static int _maxHealth_Lvl1_MisileTurret_Base_Lvl_3;

        public static int _maxHealth_Lvl2_MisileTurret_Base_Lvl_1; 
        public static int _maxHealth_Lvl2_MisileTurret_Base_Lvl_2; 
        public static int _maxHealth_Lvl2_MisileTurret_Base_Lvl_3;

        public static int _maxHealth_Lvl3_MisileTurret_Base_Lvl_1;
        public static int _maxHealth_Lvl3_MisileTurret_Base_Lvl_2;
        public static int _maxHealth_Lvl3_MisileTurret_Base_Lvl_3;


        public static int _maxShiled_Lvl1_MisileTurret_Base_Lvl_1; 
        public static int _maxShiled_Lvl1_MisileTurret_Base_Lvl_2; 
        public static int _maxShiled_Lvl1_MisileTurret_Base_Lvl_3; 

        public static int _maxShiled_Lvl2_MisileTurret_Base_Lvl_1; 
        public static int _maxShiled_Lvl2_MisileTurret_Base_Lvl_2; 
        public static int _maxShiled_Lvl2_MisileTurret_Base_Lvl_3; 
        
        public static int _maxShiled_Lvl3_MisileTurret_Base_Lvl_1;
        public static int _maxShiled_Lvl3_MisileTurret_Base_Lvl_2;
        public static int _maxShiled_Lvl3_MisileTurret_Base_Lvl_3;


        public static int _defensePoints_Lvl1_MisileTurret_Base_Lvl_1; 
        public static int _defensePoints_Lvl1_MisileTurret_Base_Lvl_2; 
        public static int _defensePoints_Lvl1_MisileTurret_Base_Lvl_3;

        public static int _defensePoints_Lvl2_MisileTurret_Base_Lvl_1; 
        public static int _defensePoints_Lvl2_MisileTurret_Base_Lvl_2; 
        public static int _defensePoints_Lvl2_MisileTurret_Base_Lvl_3;

        public static int _defensePoints_Lvl3_MisileTurret_Base_Lvl_1; 
        public static int _defensePoints_Lvl3_MisileTurret_Base_Lvl_2; 
        public static int _defensePoints_Lvl3_MisileTurret_Base_Lvl_3;













        public static float _timerStep_Turret;

        public static string GetResourcesNeedToBuildAsText___MisileTurret()
        {
            return _crystalNeedForBuilding_MisileTurret.ToString() + " " + _ironNeedForBuilding_MisileTurret.ToString() +" "+_gelNeedForBuilding_MisileTurret.ToString();
        }

        public static void GetResourcesNeedToBuild___MisileTurret(out int crystalNeed, out int ironNeed, out int gelNeed)
        {
            crystalNeed = _crystalNeedForBuilding_MisileTurret;
            ironNeed = _ironNeedForBuilding_MisileTurret;
            gelNeed = _gelNeedForBuilding_MisileTurret;
        }

        public static void InitCost_ToLvl2___MisileTurret()
        {
            TurretStaticData.turretMenuReference._upgradeButton.GetComponentInChildren<Text>().text =
            _crystalNeedForExpand_ToLvl2_MisileTurret.ToString() 
            + " " + _ironNeedForForExpand_ToLvl2_MisileTurret.ToString() 
            + " " + _gelNeedForForExpand_ToLvl2_MisileTurret.ToString();
        }

        public static void InitCost_ToLvl3___MisileTurret()
        {
            TurretStaticData.turretMenuReference._upgradeButton.GetComponentInChildren<Text>().text = 
            _crystalNeedForExpand_ToLvl3_MisileTurret.ToString() 
            + " " + _ironNeedForForExpand_ToLvl3_MisileTurret.ToString() 
            + " " + _gelNeedForForExpand_ToLvl3_MisileTurret.ToString();
        }

        // Static info about building - determins all info about every object of this building class
        public static void InitStaticFields___MisileTurret()
        {
            _crystalNeedForBuilding_MisileTurret = 60;
            _ironNeedForBuilding_MisileTurret = 60;
            _gelNeedForBuilding_MisileTurret = 60;

            _crystalNeedForExpand_ToLvl2_MisileTurret = 30;
            _ironNeedForForExpand_ToLvl2_MisileTurret = 30;
            _gelNeedForForExpand_ToLvl2_MisileTurret = 30;

            _crystalNeedForExpand_ToLvl3_MisileTurret = 40;
            _ironNeedForForExpand_ToLvl3_MisileTurret = 40;
            _gelNeedForForExpand_ToLvl3_MisileTurret = 40;

            _timerStep_Turret = 0.25f;










                

            _maxHealth_Lvl1_MisileTurret_Base_Lvl_1 = 100;
            _maxHealth_Lvl1_MisileTurret_Base_Lvl_2 = 125;
            _maxHealth_Lvl1_MisileTurret_Base_Lvl_3 = 150;

            _maxHealth_Lvl2_MisileTurret_Base_Lvl_1 = 200; 
            _maxHealth_Lvl2_MisileTurret_Base_Lvl_2 = 225; 
            _maxHealth_Lvl2_MisileTurret_Base_Lvl_3 = 250;

            _maxHealth_Lvl3_MisileTurret_Base_Lvl_1 = 300;
            _maxHealth_Lvl3_MisileTurret_Base_Lvl_2 = 325;
            _maxHealth_Lvl3_MisileTurret_Base_Lvl_3 = 350;


            _maxShiled_Lvl1_MisileTurret_Base_Lvl_1 = 100; 
            _maxShiled_Lvl1_MisileTurret_Base_Lvl_2 = 125; 
            _maxShiled_Lvl1_MisileTurret_Base_Lvl_3 = 150; 

            _maxShiled_Lvl2_MisileTurret_Base_Lvl_1 = 200; 
            _maxShiled_Lvl2_MisileTurret_Base_Lvl_2 = 225; 
            _maxShiled_Lvl2_MisileTurret_Base_Lvl_3 = 250; 
            
            _maxShiled_Lvl3_MisileTurret_Base_Lvl_1 = 300;
            _maxShiled_Lvl3_MisileTurret_Base_Lvl_2 = 325;
            _maxShiled_Lvl3_MisileTurret_Base_Lvl_3 = 350;


            _defensePoints_Lvl1_MisileTurret_Base_Lvl_1 = 10; 
            _defensePoints_Lvl1_MisileTurret_Base_Lvl_2 = 11; 
            _defensePoints_Lvl1_MisileTurret_Base_Lvl_3 = 12;

            _defensePoints_Lvl2_MisileTurret_Base_Lvl_1 = 20; 
            _defensePoints_Lvl2_MisileTurret_Base_Lvl_2 = 21; 
            _defensePoints_Lvl2_MisileTurret_Base_Lvl_3 = 22;

            _defensePoints_Lvl3_MisileTurret_Base_Lvl_1 = 30; 
            _defensePoints_Lvl3_MisileTurret_Base_Lvl_2 = 31; 
            _defensePoints_Lvl3_MisileTurret_Base_Lvl_3 = 32;


        }

        public static void GetResourcesNeedToExpand___MisileTurret(out int crystalNeed, out int ironNeed, out int gelNeed, TurretMisile mt)
        {
            if (mt.level == 1)
            {
                crystalNeed = _crystalNeedForExpand_ToLvl2_MisileTurret;
                ironNeed = _ironNeedForForExpand_ToLvl2_MisileTurret;
                gelNeed = _gelNeedForForExpand_ToLvl2_MisileTurret;
            }
            else
            {
                crystalNeed = _crystalNeedForExpand_ToLvl3_MisileTurret;
                ironNeed = _ironNeedForForExpand_ToLvl3_MisileTurret;
                gelNeed = _gelNeedForForExpand_ToLvl3_MisileTurret;
            }
        }

    #endregion

    #region LTurret

        public static int _crystalNeedForBuilding_LaserTurret;
        public static int _ironNeedForBuilding_LaserTurret;
        public static int _gelNeedForBuilding_LaserTurret;

        public static int _crystalNeedForExpand_ToLvl2_LaserTurret;
        public static int _ironNeedForForExpand_ToLvl2_LaserTurret;
        public static int _gelNeedForForExpand_ToLvl2_LaserTurret;

        public static int _crystalNeedForExpand_ToLvl3_LaserTurret;
        public static int _ironNeedForForExpand_ToLvl3_LaserTurret;
        public static int _gelNeedForForExpand_ToLvl3_LaserTurret;




        public static int _maxHealth_Lvl1_LaserTurret_Base_Lvl_1;
        public static int _maxHealth_Lvl1_LaserTurret_Base_Lvl_2;
        public static int _maxHealth_Lvl1_LaserTurret_Base_Lvl_3;

        public static int _maxHealth_Lvl2_LaserTurret_Base_Lvl_1; 
        public static int _maxHealth_Lvl2_LaserTurret_Base_Lvl_2; 
        public static int _maxHealth_Lvl2_LaserTurret_Base_Lvl_3;

        public static int _maxHealth_Lvl3_LaserTurret_Base_Lvl_1;
        public static int _maxHealth_Lvl3_LaserTurret_Base_Lvl_2;
        public static int _maxHealth_Lvl3_LaserTurret_Base_Lvl_3;


        public static int _maxShiled_Lvl1_LaserTurret_Base_Lvl_1; 
        public static int _maxShiled_Lvl1_LaserTurret_Base_Lvl_2; 
        public static int _maxShiled_Lvl1_LaserTurret_Base_Lvl_3; 

        public static int _maxShiled_Lvl2_LaserTurret_Base_Lvl_1; 
        public static int _maxShiled_Lvl2_LaserTurret_Base_Lvl_2; 
        public static int _maxShiled_Lvl2_LaserTurret_Base_Lvl_3; 
        
        public static int _maxShiled_Lvl3_LaserTurret_Base_Lvl_1;
        public static int _maxShiled_Lvl3_LaserTurret_Base_Lvl_2;
        public static int _maxShiled_Lvl3_LaserTurret_Base_Lvl_3;


        public static int _defensePoints_Lvl1_LaserTurret_Base_Lvl_1; 
        public static int _defensePoints_Lvl1_LaserTurret_Base_Lvl_2; 
        public static int _defensePoints_Lvl1_LaserTurret_Base_Lvl_3;

        public static int _defensePoints_Lvl2_LaserTurret_Base_Lvl_1; 
        public static int _defensePoints_Lvl2_LaserTurret_Base_Lvl_2; 
        public static int _defensePoints_Lvl2_LaserTurret_Base_Lvl_3;

        public static int _defensePoints_Lvl3_LaserTurret_Base_Lvl_1; 
        public static int _defensePoints_Lvl3_LaserTurret_Base_Lvl_2; 
        public static int _defensePoints_Lvl3_LaserTurret_Base_Lvl_3;



        public static string GetResourcesNeedToBuildAsText___LaserTurret()
        {
            return _crystalNeedForBuilding_LaserTurret.ToString() + " " + _ironNeedForBuilding_LaserTurret.ToString() +" "+_gelNeedForBuilding_LaserTurret.ToString();
        }

        public static void GetResourcesNeedToBuild___LaserTurret(out int crystalNeed, out int ironNeed, out int gelNeed)
        {
            crystalNeed = _crystalNeedForBuilding_LaserTurret;
            ironNeed = _ironNeedForBuilding_LaserTurret;
            gelNeed = _gelNeedForBuilding_LaserTurret;
        }

        // Static info about building - determins all info about every object of this building class
        public static void InitStaticFields___LaserTurret()
        {
            _crystalNeedForBuilding_LaserTurret = 60;
            _ironNeedForBuilding_LaserTurret = 60;
            _gelNeedForBuilding_LaserTurret = 60;

            _crystalNeedForExpand_ToLvl2_LaserTurret = 30;
            _ironNeedForForExpand_ToLvl2_LaserTurret = 30;
            _gelNeedForForExpand_ToLvl2_LaserTurret = 30;

            _crystalNeedForExpand_ToLvl3_LaserTurret = 40;
            _ironNeedForForExpand_ToLvl3_LaserTurret = 40;
            _gelNeedForForExpand_ToLvl3_LaserTurret = 40;





            


                

            _maxHealth_Lvl1_LaserTurret_Base_Lvl_1 = 100;
            _maxHealth_Lvl1_LaserTurret_Base_Lvl_2 = 125;
            _maxHealth_Lvl1_LaserTurret_Base_Lvl_3 = 150;

            _maxHealth_Lvl2_LaserTurret_Base_Lvl_1 = 200; 
            _maxHealth_Lvl2_LaserTurret_Base_Lvl_2 = 225; 
            _maxHealth_Lvl2_LaserTurret_Base_Lvl_3 = 250;

            _maxHealth_Lvl3_LaserTurret_Base_Lvl_1 = 300;
            _maxHealth_Lvl3_LaserTurret_Base_Lvl_2 = 325;
            _maxHealth_Lvl3_LaserTurret_Base_Lvl_3 = 350;


            _maxShiled_Lvl1_LaserTurret_Base_Lvl_1 = 100; 
            _maxShiled_Lvl1_LaserTurret_Base_Lvl_2 = 125; 
            _maxShiled_Lvl1_LaserTurret_Base_Lvl_3 = 150; 

            _maxShiled_Lvl2_LaserTurret_Base_Lvl_1 = 200; 
            _maxShiled_Lvl2_LaserTurret_Base_Lvl_2 = 225; 
            _maxShiled_Lvl2_LaserTurret_Base_Lvl_3 = 250; 
            
            _maxShiled_Lvl3_LaserTurret_Base_Lvl_1 = 300;
            _maxShiled_Lvl3_LaserTurret_Base_Lvl_2 = 325;
            _maxShiled_Lvl3_LaserTurret_Base_Lvl_3 = 350;


            _defensePoints_Lvl1_LaserTurret_Base_Lvl_1 = 10; 
            _defensePoints_Lvl1_LaserTurret_Base_Lvl_2 = 11; 
            _defensePoints_Lvl1_LaserTurret_Base_Lvl_3 = 12;

            _defensePoints_Lvl2_LaserTurret_Base_Lvl_1 = 20; 
            _defensePoints_Lvl2_LaserTurret_Base_Lvl_2 = 21; 
            _defensePoints_Lvl2_LaserTurret_Base_Lvl_3 = 22;

            _defensePoints_Lvl3_LaserTurret_Base_Lvl_1 = 30; 
            _defensePoints_Lvl3_LaserTurret_Base_Lvl_2 = 31; 
            _defensePoints_Lvl3_LaserTurret_Base_Lvl_3 = 32;
        }

        public static void InitCost_ToLvl2___LaserTurret()
        {
            TurretStaticData.turretMenuReference._upgradeButton.GetComponentInChildren<Text>().text = 
            _crystalNeedForExpand_ToLvl2_LaserTurret.ToString() 
            + " " + _ironNeedForForExpand_ToLvl2_LaserTurret.ToString() 
            + " " + _gelNeedForForExpand_ToLvl2_LaserTurret.ToString();
        }

        public static void InitCost_ToLvl3___LaserTurret()
        {
            TurretStaticData.turretMenuReference._upgradeButton.GetComponentInChildren<Text>().text = 
            _crystalNeedForExpand_ToLvl3_LaserTurret.ToString() 
            + " " + _ironNeedForForExpand_ToLvl3_LaserTurret.ToString() 
            + " " + _gelNeedForForExpand_ToLvl3_LaserTurret.ToString();
        }

        public static void GetResourcesNeedToExpand___LaserTurret(out int crystalNeed, out int ironNeed, out int gelNeed, TurretLaser lt)
        {
            if (lt.level == 1)
            {
                crystalNeed = StatsManager._crystalNeedForExpand_ToLvl2_LaserTurret;
                ironNeed = StatsManager._ironNeedForForExpand_ToLvl2_LaserTurret;
                gelNeed = StatsManager._gelNeedForForExpand_ToLvl2_LaserTurret;
            }
            else
            {
                crystalNeed = StatsManager._crystalNeedForExpand_ToLvl3_LaserTurret;
                ironNeed = StatsManager._ironNeedForForExpand_ToLvl3_LaserTurret;
                gelNeed = StatsManager._gelNeedForForExpand_ToLvl3_LaserTurret;
            }
        }

    #endregion

    #region Unit

        public static int maxUnit_Health;
        public static int maxUnit_Shield;
        public static int maxUnit_defense;

        public void InitStaticFields___Unit()
        {
            maxUnit_Health = 50;
            maxUnit_Shield = 50;
            maxUnit_defense = 5;
        }

    #endregion

    #region Shield Generator

        public static int _crystalNeedForBuilding_ShieldGenerator;
        public static int _ironNeedForBuilding_ShieldGenerator;
        public static int _gelNeedForBuilding_ShieldGenerator;

        public static int _crystalNeedForExpand_ToLvl2_ShieldGenerator;
        public static int _ironNeedForForExpand_ToLvl2_ShieldGenerator;
        public static int _gelNeedForForExpand_ToLvl2_ShieldGenerator;

        public static int _crystalNeedForExpand_ToLvl3_ShieldGenerator;
        public static int _ironNeedForForExpand_ToLvl3_ShieldGenerator;
        public static int _gelNeedForForExpand_ToLvl3_ShieldGenerator;



        
        public static int _maxHealth_Lvl1_ShieldGenerator_Base_Lvl_1; 
        public static int _maxHealth_Lvl1_ShieldGenerator_Base_Lvl_2;
        public static int _maxHealth_Lvl1_ShieldGenerator_Base_Lvl_3;

        public static int _maxHealth_Lvl2_ShieldGenerator_Base_Lvl_1;
        public static int _maxHealth_Lvl2_ShieldGenerator_Base_Lvl_2;
        public static int _maxHealth_Lvl2_ShieldGenerator_Base_Lvl_3;

        public static int _maxHealth_Lvl3_ShieldGenerator_Base_Lvl_1;
        public static int _maxHealth_Lvl3_ShieldGenerator_Base_Lvl_2;
        public static int _maxHealth_Lvl3_ShieldGenerator_Base_Lvl_3;

        public static int _maxShiled_Lvl1_ShieldGenerator_Base_Lvl_1;
        public static int _maxShiled_Lvl1_ShieldGenerator_Base_Lvl_2;
        public static int _maxShiled_Lvl1_ShieldGenerator_Base_Lvl_3;

        public static int _maxShiled_Lvl2_ShieldGenerator_Base_Lvl_1;
        public static int _maxShiled_Lvl2_ShieldGenerator_Base_Lvl_2;
        public static int _maxShiled_Lvl2_ShieldGenerator_Base_Lvl_3;

        public static int _maxShiled_Lvl3_ShieldGenerator_Base_Lvl_1;
        public static int _maxShiled_Lvl3_ShieldGenerator_Base_Lvl_2;
        public static int _maxShiled_Lvl3_ShieldGenerator_Base_Lvl_3;

        public static int _defensePoints_Lvl1_ShieldGenerator_Base_Lvl_1;
        public static int _defensePoints_Lvl1_ShieldGenerator_Base_Lvl_2;
        public static int _defensePoints_Lvl1_ShieldGenerator_Base_Lvl_3;

        public static int _defensePoints_Lvl2_ShieldGenerator_Base_Lvl_1; 
        public static int _defensePoints_Lvl2_ShieldGenerator_Base_Lvl_2; 
        public static int _defensePoints_Lvl2_ShieldGenerator_Base_Lvl_3; 
        
        public static int _defensePoints_Lvl3_ShieldGenerator_Base_Lvl_1;
        public static int _defensePoints_Lvl3_ShieldGenerator_Base_Lvl_2;
        public static int _defensePoints_Lvl3_ShieldGenerator_Base_Lvl_3;







        // Static info about building - determins all info about every object of this building class
        public static void InitStaticFields___ShieldGenerator()
        {
            _crystalNeedForBuilding_ShieldGenerator = 20;
            _ironNeedForBuilding_ShieldGenerator = 20;
            _gelNeedForBuilding_ShieldGenerator = 20;

            _crystalNeedForExpand_ToLvl2_ShieldGenerator = 30;
            _ironNeedForForExpand_ToLvl2_ShieldGenerator = 30;
            _gelNeedForForExpand_ToLvl2_ShieldGenerator = 30;

            _crystalNeedForExpand_ToLvl3_ShieldGenerator = 40;
            _ironNeedForForExpand_ToLvl3_ShieldGenerator = 40;
            _gelNeedForForExpand_ToLvl3_ShieldGenerator = 40;



            _maxHealth_Lvl1_ShieldGenerator_Base_Lvl_1 = 100; 
            _maxHealth_Lvl1_ShieldGenerator_Base_Lvl_2 = 125;
            _maxHealth_Lvl1_ShieldGenerator_Base_Lvl_3 = 150;

            _maxHealth_Lvl2_ShieldGenerator_Base_Lvl_1 = 200;
            _maxHealth_Lvl2_ShieldGenerator_Base_Lvl_2 = 225;
            _maxHealth_Lvl2_ShieldGenerator_Base_Lvl_3 = 250;

            _maxHealth_Lvl3_ShieldGenerator_Base_Lvl_1 = 300;
            _maxHealth_Lvl3_ShieldGenerator_Base_Lvl_2 = 325;
            _maxHealth_Lvl3_ShieldGenerator_Base_Lvl_3 = 350;

            _maxShiled_Lvl1_ShieldGenerator_Base_Lvl_1 = 100;
            _maxShiled_Lvl1_ShieldGenerator_Base_Lvl_2 = 125;
            _maxShiled_Lvl1_ShieldGenerator_Base_Lvl_3 = 150;

            _maxShiled_Lvl2_ShieldGenerator_Base_Lvl_1 = 200;
            _maxShiled_Lvl2_ShieldGenerator_Base_Lvl_2 = 225;
            _maxShiled_Lvl2_ShieldGenerator_Base_Lvl_3 = 250;

            _maxShiled_Lvl3_ShieldGenerator_Base_Lvl_1 = 300;
            _maxShiled_Lvl3_ShieldGenerator_Base_Lvl_2 = 325;
            _maxShiled_Lvl3_ShieldGenerator_Base_Lvl_3 = 350;

            _defensePoints_Lvl1_ShieldGenerator_Base_Lvl_1 = 10;
            _defensePoints_Lvl1_ShieldGenerator_Base_Lvl_2 = 11;
            _defensePoints_Lvl1_ShieldGenerator_Base_Lvl_3 = 12;

            _defensePoints_Lvl2_ShieldGenerator_Base_Lvl_1 = 20; 
            _defensePoints_Lvl2_ShieldGenerator_Base_Lvl_2 = 21; 
            _defensePoints_Lvl2_ShieldGenerator_Base_Lvl_3 = 22; 
            
            _defensePoints_Lvl3_ShieldGenerator_Base_Lvl_1 = 30;
            _defensePoints_Lvl3_ShieldGenerator_Base_Lvl_2 = 31;
            _defensePoints_Lvl3_ShieldGenerator_Base_Lvl_3 = 32;
        }

















        // Building menu cost
        public static string GetResourcesNeedToBuildAsText___ShieldGenerator()
        {
            return 
            _crystalNeedForBuilding_ShieldGenerator.ToString() + " " + 
            _ironNeedForBuilding_ShieldGenerator.ToString() + " " + 
            _gelNeedForBuilding_ShieldGenerator.ToString();
        }

        public static void GetResourcesNeedToBuild___ShieldGenerator(out int crystalNeed, out int ironNeed, out int gelNeed)
        {
            crystalNeed = _crystalNeedForBuilding_ShieldGenerator;
            ironNeed = _ironNeedForBuilding_ShieldGenerator;
            gelNeed = _gelNeedForBuilding_ShieldGenerator;
        }

        public static void GetResourcesNeedToExpand___ShieldGenerator(out int crystalNeed, out int ironNeed, out int gelNeed, ShieldGenerator sg)
        {
            if (sg.level == 1)
            {
                crystalNeed = _crystalNeedForExpand_ToLvl2_ShieldGenerator;
                ironNeed = _ironNeedForForExpand_ToLvl2_ShieldGenerator;
                gelNeed = _gelNeedForForExpand_ToLvl2_ShieldGenerator;
            }
            else
            {
                crystalNeed = _crystalNeedForExpand_ToLvl3_ShieldGenerator;
                ironNeed = _ironNeedForForExpand_ToLvl3_ShieldGenerator;
                gelNeed = _gelNeedForForExpand_ToLvl3_ShieldGenerator;
            }
        }

        public static void InitCost_ToLvl2___ShieldGenerator()
        {
            ShiledGeneratorStaticData.shieldGeneratorMenuReference.upgradeButton.GetComponentInChildren<Text>().text = 
            _crystalNeedForExpand_ToLvl2_ShieldGenerator.ToString() + " " + 
            _ironNeedForForExpand_ToLvl2_ShieldGenerator.ToString() + " " + 
            _gelNeedForForExpand_ToLvl2_ShieldGenerator.ToString();
        }

        public static void InitCost_ToLvl3___ShieldGenerator()
        {
            ShiledGeneratorStaticData.shieldGeneratorMenuReference.upgradeButton.GetComponentInChildren<Text>().text = 
            _crystalNeedForExpand_ToLvl3_ShieldGenerator.ToString() + " " + 
            _ironNeedForForExpand_ToLvl3_ShieldGenerator.ToString() + " " + 
            _gelNeedForForExpand_ToLvl3_ShieldGenerator.ToString();
        }

    #endregion

    #region Enemy Bomber

        public static int _maxHealth_Bomber;
        public static int _maxShield_Bomber;
        public static int _maxDefense_Bomber;

        public void InitStaticFields___Bomber()
        {
            _maxHealth_Bomber = 50;
            _maxShield_Bomber = 50;
            _maxDefense_Bomber = 5;
        }

    #endregion


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitStaticFields___Antenne();
        InitStaticFields___Unit();
        InitStaticFields___Bomber();
        InitStats___PowerPlant();
        InitStats___Garage();
        InitStaticFields___MisileTurret();
        InitStaticFields___LaserTurret();
        InitAllStaticFields___MineShaft();
        InitStaticFields___ShieldGenerator();
        InitUnitCreationCost___Garage();

        InitCost_ForPerks();
        InitStaticFields();
        
        InitCost_ToLvl2___Shtab();
    }
}
