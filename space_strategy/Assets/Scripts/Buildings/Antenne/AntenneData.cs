using UnityEngine;

public class AntenneData
{
    public GameObject _tileOccupied = null;
    public GameObject _tileOccupied1 = null;

    public bool isMenuOpened = false;

    public int rotation;

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

        rotation = model.rotation;
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
