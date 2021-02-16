using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Vector3 mapCenter;

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

    int[,] myArr;

    private void Start()
    {
        GanarteArray();
        CreateMapFromArray();
    }

    private void GanarteArray()
    {
        myArr = new int[MapSizeColumn, MapSizeRow];

        // 0 - Map Edges
        // 1 - Free tile
        // 2 - Closed Tiles
        // 3 - RS1
        // 4 - RS2
        // 5 - RS3

        for (int column = 0; column < MapSizeColumn; column++)
        {
            for (int row = 0; row < MapSizeRow; row++)
            {
                myArr[column, row] = 1;

                if((column == 0 || column == MapSizeColumn) || (row == 0 || row == MapSizeRow)
                || (column == 1 || column == MapSizeColumn-1) || (row == 1 || row == MapSizeRow-1))
                    myArr[column, row] = 0;


                if(column == 5 && row == 4)
                {
                    myArr[column, row] = 2;
                }
                if(column == 5 && row == 5)
                {
                    myArr[column, row] = 2;
                }
                if(column == 6 && row == 4)
                {
                    myArr[column, row] = 2;
                }
                if(column == 6 && row == 5)
                {
                    myArr[column, row] = 2;
                }
            }
        }
    }

    private void CreateMapFromArray()
    {
        for (int column = 0; column < MapSizeColumn; column++)
        {
            for (int row = 0; row < MapSizeRow; row++)
            {
                GameObject hexGO = null;

                switch (myArr[column, row])
                {
                    case 0:
                    hexGO = Instantiate(hexPrefab_MapEdge, Vector3.zero, Quaternion.identity, this.transform);
                    temp = hexGO.GetComponent<Hex>();
                    temp.Initialize_with_arr_pos(column, row);
                    temp.tile_Type = Tile_Type.MapEdge;
                    break;

                    case 1:
                    hexGO = Instantiate(hexPrefab_FreeTile, Vector3.zero, Quaternion.identity, this.transform);
                    temp = hexGO.GetComponent<Hex>();
                    temp.Initialize_with_arr_pos(column, row);
                    temp.tile_Type = Tile_Type.FreeTile;
                    break;

                    case 2:
                    hexGO = Instantiate(hexPrefab_ClosedTile, Vector3.zero, Quaternion.identity, this.transform);
                    temp = hexGO.GetComponent<Hex>();
                    temp.Initialize_with_arr_pos(column, row);
                    temp.tile_Type = Tile_Type.ClosedTile;
                    break;

                    case 3: 
                    hexGO = Instantiate(hexPrefab_RS1_crystal, Vector3.zero, Quaternion.identity, this.transform);
                    temp = hexGO.GetComponent<Hex>();
                    temp.Initialize_with_arr_pos(column, row);
                    temp.tile_Type = Tile_Type.RS1_crystal;
                    break;

                    case 4: 
                    hexGO = Instantiate(hexPrefab_RS2_iron, Vector3.zero, Quaternion.identity, this.transform);
                    temp = hexGO.GetComponent<Hex>();
                    temp.Initialize_with_arr_pos(column, row);
                    temp.tile_Type = Tile_Type.RS2_iron;
                    break;

                    case 5:
                    hexGO = Instantiate(hexPrefab_RS3_gel, Vector3.zero, Quaternion.identity, this.transform);
                    temp = hexGO.GetComponent<Hex>();
                    temp.Initialize_with_arr_pos(column, row);
                    temp.tile_Type = Tile_Type.RS3_gel;
                    break;

                    case 6: 
                    hexGO = Instantiate(hexPrefab_EnemyTile, Vector3.zero, Quaternion.identity, this.transform);
                    temp = hexGO.GetComponent<Hex>();
                    temp.Initialize_with_arr_pos(column, row);
                    temp.tile_Type = Tile_Type.EnemyTile;
                    break;
                }

                hexGO.transform.position = temp.Position();
                //hexGO.name = string.Format(column + "." + row);
                hexGO.name = string.Format(temp.Q + "." + temp.R + "." + temp.S);

                SpriteRenderer sr = hexGO.GetComponentInChildren<SpriteRenderer>();
            }
        }
    }
}
