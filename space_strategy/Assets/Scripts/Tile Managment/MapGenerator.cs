using UnityEngine;
using System;


public class MapGenerator : MonoBehaviour
{
    public int width;
	public int height;

	public string seed;
	public bool useRandomSeed;

	[Range(0,100)]
	public int randomFillPercent;

	int[,] map;



	// void Update() {
	// 	if (Input.GetMouseButtonDown(0)) {
	// 		GenerateMap();
    //         CreateMapFromArray();
	// 	}
	// }

	void GenerateMap() 
    {
		map = new int[width,height];
		RandomFillMap();

		for (int i = 0; i < 5; i ++) 
        {
			SmoothMap();
		}
	}


	void RandomFillMap() 
    {
		if (useRandomSeed) 
        {
			seed = System.DateTime.Now.ToString();
            Debug.Log(System.DateTime.Now);
		}

		System.Random pseudoRandom = new System.Random(seed.GetHashCode());

		for (int x = 0; x < width; x ++) 
        {
			for (int y = 0; y < height; y ++) 
            {
				if (x == 0 || x == width-1 || y == 0 || y == height -1) 
                {
					map[x,y] = 1;
				}
				else 
                {
					map[x,y] = (pseudoRandom.Next(0,100) < randomFillPercent)? 1: 0;
				}
			}
		}
	}

	void SmoothMap() 
    {
		for (int x = 0; x < width; x ++) 
        {
			for (int y = 0; y < height; y ++) 
            {
				int neighbourWallTiles = GetSurroundingWallCount(x,y);

				if (neighbourWallTiles > 4)
					map[x,y] = 1;
				else if (neighbourWallTiles < 4)
					map[x,y] = 0;

			}
		}
	}

	int GetSurroundingWallCount(int gridX, int gridY) 
    {
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) 
        {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY ++) 
            {
				if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) 
                {
					if (neighbourX != gridX || neighbourY != gridY) 
                    {
						wallCount += map[neighbourX,neighbourY];
					}
				}
				else 
                {
					wallCount ++;
				}
			}
		}

		return wallCount;
	}




    
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

    private void Start()
    {
		GenerateMap();

        // GanarteArray();

        CreateMapFromArray();
    }

    // private void GanarteArray()
    // {
    //     myArr = new int[MapSizeColumn, MapSizeRow];

    //     // 0 - Map Edges
    //     // 1 - Free tile
    //     // 2 - Closed Tiles
    //     // 3 - RS1
    //     // 4 - RS2
    //     // 5 - RS3

    //     for (int column = 0; column < MapSizeColumn; column++)
    //     {
    //         for (int row = 0; row < MapSizeRow; row++)
    //         {
    //             myArr[column, row] = 1;

    //             if((column == 0 || column == MapSizeColumn) || (row == 0 || row == MapSizeRow)
    //             || (column == 1 || column == MapSizeColumn-1) || (row == 1 || row == MapSizeRow-1))
    //                 myArr[column, row] = 0;


    //             if(column == 5 && row == 4)
    //             {
    //                 myArr[column, row] = 2;
    //             }
    //             if(column == 5 && row == 5)
    //             {
    //                 myArr[column, row] = 2;
    //             }
    //             if(column == 6 && row == 4)
    //             {
    //                 myArr[column, row] = 2;
    //             }
    //             if(column == 6 && row == 5)
    //             {
    //                 myArr[column, row] = 2;
    //             }


    //             if(column == 5 && row == 2)
    //             {
    //                 myArr[column, row] = 3;
    //             }

    //             if(column == 3 && row == 5)
    //             {
    //                 myArr[column, row] = 4;
    //             }

    //             if(column == 4 && row == 4)
    //             {
    //                 myArr[column, row] = 5;
    //             }
    //         }
    //     }
    // }

    private void CreateMapFromArray()
    {
        for (int column = 0; column < width; column++)
        {
            for (int row = 0; row < height; row++)
            {
                GameObject hexGO = null;

                if(column == 4 && row == 7)
                {
                    map[column, row] = 3;
                }

                if(column == 3 && row == 5)
                {
                    map[column, row] = 4;
                }

                if(column == 2 && row == 4)
                {
                    map[column, row] = 5;
                }

                switch (map[column, row])
                {
                    case 1:
                    hexGO = Instantiate(hexPrefab_MapEdge, Vector3.zero, Quaternion.identity, this.transform);
                    temp = hexGO.GetComponent<Hex>();
                    temp.Initialize_with_arr_pos(column, row);
                    temp.tile_Type = Tile_Type.MapEdge;
                    break;

                    case 0:
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
