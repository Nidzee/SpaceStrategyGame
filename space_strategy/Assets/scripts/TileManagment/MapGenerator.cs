using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject hefPrefab_MapEdge;
    [SerializeField] private GameObject hexPrefab_FreeTile;
    [SerializeField] private GameObject hexPrefab_MapEdge;
    [SerializeField] private GameObject hexPrefab_ClosedTile;
    [SerializeField] private GameObject hexPrefab_RS1_crystal;
    [SerializeField] private GameObject hexPrefab_RS2_iron;
    [SerializeField] private GameObject hexPrefab_RS3_gel;
    [SerializeField] private GameObject hexPrefab_EnemyTile;

    private Hex temp;

    private int MapSizeColumn = 30;
    private int MapSizeRow = 30;

    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        for (int column = 0; column <=MapSizeColumn; column++)
        {
            for (int row = 0; row <= MapSizeRow; row++)
            {
                GameObject hexGO = Instantiate(hexPrefab_FreeTile, Vector3.zero, Quaternion.identity, this.transform);
                temp = hexGO.GetComponent<Hex>();
                temp.Initialize_with_arr_pos(column, row);
                hexGO.transform.position = temp.Position();
                //hexGO.name = string.Format(column + "." + row);
                hexGO.name = string.Format(temp.Q + "." + temp.R + "." + temp.S);

                SpriteRenderer sr = hexGO.GetComponentInChildren<SpriteRenderer>();
                
                if((column == 0 || column == MapSizeColumn) || (row == 0 || row == MapSizeRow)
                || (column == 1 || column == MapSizeColumn-1) || (row == 1 || row == MapSizeRow-1))
                    hexGO.GetComponent<Hex>().tile_Type = Tile_Type.MapEdge;

                if(column == 5 && row == 4)
                {
                    hexGO.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
                }
                if(column == 5 && row == 5)
                {
                    hexGO.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
                }
                if(column == 6 && row == 4)
                {
                    hexGO.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
                }
                if(column == 6 && row == 5)
                {
                    hexGO.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
                }

                
                switch(hexGO.GetComponent<Hex>().tile_Type)
                {
                    case Tile_Type.MapEdge: sr.color = Color.gray; 
                    break;

                    case Tile_Type.FreeTile: sr.color = Color.green;
                    break;

                    case Tile_Type.ClosedTile: sr.color = Color.red;
                    break;

                    case Tile_Type.RS1_crystal: sr.color = Color.yellow;
                    break;

                    case Tile_Type.RS2_iron: sr.color = Color.yellow;
                    break;

                    case Tile_Type.RS3_gel: sr.color = Color.yellow;
                    break;

                    case Tile_Type.EnemyTile: sr.color = Color.magenta;
                    break;
                }
                
            }
        }
    }
}
