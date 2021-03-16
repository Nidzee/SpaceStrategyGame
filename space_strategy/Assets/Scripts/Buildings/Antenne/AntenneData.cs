using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntenneData : MonoBehaviour
{
    private GameObject _tileOccupied = null;  // Reference to real MapTile on which building is set
    private GameObject _tileOccupied1 = null; // Reference to real MapTile on which building is set

    public bool isMenuOpened = false;

    public Antenne _myAntenne;

    public AntenneData(Antenne antenne)
    {
        _myAntenne = antenne;
    }


    public void Invoke()
    {
        AntenneStaticData.antenneMenuReference.ReloadPanel();
    }

    public void ConstructBuilding(Model model)
    {
        _tileOccupied = model.BTileZero;
        _tileOccupied1 = model.BTileOne;
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        GarageStaticData.garage_counter++;
        _myAntenne.gameObject.name = "AN1";
    }

    public void DestroyBuilding()
    {
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
        _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;

        if (isMenuOpened)
        {
            AntenneStaticData.antenneMenuReference.ExitMenu();
        }
    }

}
