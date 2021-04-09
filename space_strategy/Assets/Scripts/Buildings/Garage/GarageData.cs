// using UnityEngine;
// using System.Collections.Generic;
// using System.Collections;

// public class GarageData
// {
//     public int ID;
//     public int[] _garageMembersIDs;             // Units that are living here    

//     public Unit _unitRef;
//     public bool _isMenuOpened;

//     public GameObject angar;
//     public GameObject relaxPoint1;
//     public GameObject relaxPoint2;
//     public GameObject relaxPoint3;
//     public GameObject relaxPoint4;
//     public GameObject relaxPointCENTER;

//     public GameObject _tileOccupied;              // Reference to real MapTile on which building is set
//     public GameObject _tileOccupied1;             // Reference to real MapTile on which building is set
//     public List<Unit> _garageMembers;             // Units that are living here    
//     public float _timerForCreatingUnit;
//     public int _queue;                              
//     public int _clicks;
//     public int _numberOfUnitsToCome;
//     public int roatation;

//     Garage _myGarage;


//     public GarageData(Garage thisGarage)
//     {
//         _myGarage = thisGarage;

//         _tileOccupied = null;
//         _tileOccupied1 = null;
//         _garageMembers = new List<Unit>();
//         _timerForCreatingUnit = 0f;
//         _queue = 0;
//         _clicks = 0;
//         _numberOfUnitsToCome = GarageStaticData._garageCapacity;
//     }

//     private void HelperObjectInit()
//     {
//         if (_myGarage.gameObject.transform.childCount != 0)
//         {
//             angar = _myGarage.gameObject.transform.GetChild(0).gameObject;

//             angar.tag = TagConstants.garageAngarTag;
//             angar.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
//             angar.transform.position = _tileOccupied1.transform.position;

//             relaxPoint1 = angar.transform.GetChild(0).gameObject;
//             relaxPoint2 = angar.transform.GetChild(1).gameObject;
//             relaxPoint3 = angar.transform.GetChild(2).gameObject;
//             relaxPoint4 = angar.transform.GetChild(3).gameObject;
//             relaxPointCENTER = angar.transform.GetChild(4).gameObject;            
//         }

//         else
//         {
//             Debug.LogError("ERROR!      No child object (For range) in shaft!     Cannot get dispenser coords!");
//         }
//     }

//     public void ConstructBuilding(Model model)
//     {
//         _tileOccupied = model.BTileZero;
//         _tileOccupied1 = model.BTileOne;
//         _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
//         _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

//         roatation = model.rotation;

//         // GarageStaticData.garage_counter++;

//         HelperObjectInit();

//         AddHomelessUnitAfterBuildingConstruction();
//     }

// #region  REDO Redion

//     public void DestroyBuilding()
//     {
//         List<MineShaft> shaftsToReloadSliders = new List<MineShaft>();

//         foreach (var unit in _garageMembers)
//         {
//             // If we found new home at run-time - dont delete work
//             bool temp = ResourceManager.Instance.SetNewHomeForUnitFromDestroyedGarage(unit, _myGarage);

//             if (temp)
//             {
//                 // Home is changed
//                 // WorkPlace is still the same
//             }
//             else
//             {
//                 // Delete home
//                 // If he was working - delete work
//                 unit.Home = null;

//                 if (unit.WorkPlace)
//                 {
//                     MineShaft sameWorkplace = unit.WorkPlace;
//                     unit.WorkPlace.RemoveUnit(unit);
//                     ResourceManager.Instance.homelessUnits.Add(unit);

//                     // If garage destroys but unit which was garage member was working and its shaft menu was open
//                     // if (sameWorkplace.mineShaftData.isMenuOpened)
//                     {
//                         MineShaftStaticData.shaftMenuReference.ReloadUnitSlider();
//                     }
//                     else
//                     {
//                         shaftsToReloadSliders.Add(sameWorkplace);
//                     }
//                 }
//                 else
//                 {
//                     ResourceManager.Instance.avaliableUnits.Remove(unit);
//                     ResourceManager.Instance.homelessUnits.Add(unit);
//                 }
//             }
//         }
        
//         _garageMembers.Clear();
//         _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
//         _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;

//         if (_isMenuOpened)
//         {
//             GarageStaticData.garageMenuReference.ExitMenu();
//         }

//         // По суті - коли ми ремуваємо юніта - то викликається івент і все там собі оновлює
//         // REDO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//         // GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterGarageDestroying(shaftsToReloadSliders);
//     }

// #endregion

// #region Unit Manipulating

//     public void AddHomelessUnitAfterBuildingConstruction()
//     {
//         if (ResourceManager.Instance.homelessUnits.Count != 0)
//         {
//             for (int i = 0; i < GarageStaticData._garageCapacity; i++)
//             {
//                 _unitRef = ResourceManager.Instance.homelessUnits[(ResourceManager.Instance.homelessUnits.Count)-1];

//                 AddHomelessUnit(_unitRef);

//                 ResourceManager.Instance.homelessUnits.Remove(_unitRef);
//                 ResourceManager.Instance.avaliableUnits.Add(_unitRef);

//                 // Check there are still homeless units (decrements above!)
//                 if (ResourceManager.Instance.homelessUnits.Count == 0)
//                 {
//                     // Reload unit images because we add new unit
//                     if (_isMenuOpened)
//                     {
//                         GarageStaticData.garageMenuReference.ReloadPanel(_myGarage);
//                     }

//                     // GameViewMenu.Instance.ReloadMainUnitCount();

//                     return;
//                 }
//             }
//         }
//     }

//     public void RemoveUnit(Unit deadUnit)
//     {
//         deadUnit.Home = null;
//         _garageMembers.Remove(deadUnit);

//         _clicks--;
//         _numberOfUnitsToCome++;
//     }

//     public void AddHomelessUnit(Unit newUnit)
//     {
//         newUnit.Home = _myGarage;
//         _garageMembers.Add(newUnit);
        
//         _clicks++;
//         _numberOfUnitsToCome--;
//     }

//     public void AddCreatedByButtonUnit(Unit newUnit)
//     {
//         // No nned for clicks modifying because they were modifyed after button touch
//         newUnit.Home = _myGarage;
//         _garageMembers.Add(newUnit);
//     }


//     public Transform GetUnitDestination()
//     {
//         return angar.transform;
//     }

//     public void StartUnitCreation()
//     {
//         _queue++;                    // Increments queue
//         _numberOfUnitsToCome--;       // Decrease number of incoming homeless units
//         _clicks++;                    // Clicks increment

//         if (_queue == 1)
//         {
//             _myGarage.StartCoroutine(CreateUnitLogic());
//         }
//     }

//     public IEnumerator CreateUnitLogic()
//     {
//         while (true)
//         {
//             while (_timerForCreatingUnit < 1)
//             {
//                 _timerForCreatingUnit += GarageStaticData._timerStep * Time.deltaTime;

//                 if (_isMenuOpened)
//                 {
//                     GarageStaticData.garageMenuReference.loadingBar.fillAmount = _timerForCreatingUnit;
//                 }

//                 yield return null;
//             }

//             _timerForCreatingUnit = 0f;
            
//             _myGarage.CreateUnit();

//             _queue--;

//             if (_queue == 0)
//             {
//                 yield break;
//             }
//         }
//     }

// #endregion













//     public void InitGarageDataFromFile(GarageSavingData garageSavedInfo)
//     {
//         ID = garageSavedInfo.ID;
//         roatation = garageSavedInfo.rotation;

//         // _tileOccupied = GameObject.Find(garageSavedInfo._tileOccupiedName);
//         // _tileOccupied1 = GameObject.Find(garageSavedInfo._tileOccupied1Name);
        
//         _garageMembersIDs = garageSavedInfo._garageMembersIDs;

//         _timerForCreatingUnit = garageSavedInfo._timerForCreatingUnit;
//         _queue = garageSavedInfo._queue;
//         _clicks = garageSavedInfo._clicks;
//         _numberOfUnitsToCome = garageSavedInfo._numberOfUnitsToCome;
        
//         HelperObjectInit();
//     }
// }