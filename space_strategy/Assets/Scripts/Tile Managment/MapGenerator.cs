﻿using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance {get; private set;}

    [SerializeField] private GameObject hefPrefab_MapEdge;
    [SerializeField] private GameObject hexPrefab_FreeTile;
    [SerializeField] private GameObject hexPrefab_MapEdge;
    [SerializeField] private GameObject hexPrefab_ClosedTile;
    [SerializeField] private GameObject hexPrefab_RS1_crystal;
    [SerializeField] private GameObject hexPrefab_RS2_iron;
    [SerializeField] private GameObject hexPrefab_RS3_gel;
    [SerializeField] private GameObject hexPrefab_EnemyTile;

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
    }


#region Map generating region
    public int width;
	public int height;

	public string seed;
	public bool useRandomSeed;

	[Range(0,100)]
	public int randomFillPercent;

	int[,] map;

	public void GenerateMap() 
    {
		map = new int[width,height];
		RandomFillMap();

		for (int i = 0; i < 5; i ++) 
        {
			SmoothMap();
		}

        CreateMapFromArray();
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
#endregion


    // 0 - Map Edges
    // 1 - Free tile
    // 2 - Closed Tiles
    // 3 - RS1
    // 4 - RS2
    // 5 - RS3
    // 6 - Enemy

    GameObject newMapTile = null;

    public void CreateMapFromArray()
    {
        for (int column = 0; column < width; column++)
        {
            for (int row = 0; row < height; row++)
            {





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

                if (column == 4 && row == 9)
                {
                    map[column, row] = 6;
                }





                switch (map[column, row])
                {
                    case 0:
                    newMapTile = Instantiate(hexPrefab_FreeTile, Vector3.zero, Quaternion.identity, this.transform);
                    break;

                    case 1:
                    newMapTile = Instantiate(hexPrefab_MapEdge, Vector3.zero, Quaternion.identity, this.transform);
                    break;

                    case 2:
                    newMapTile = Instantiate(hexPrefab_ClosedTile, Vector3.zero, Quaternion.identity, this.transform);
                    break;

                    case 3: 
                    newMapTile = Instantiate(hexPrefab_RS1_crystal, Vector3.zero, Quaternion.identity, this.transform);
                    break;

                    case 4: 
                    newMapTile = Instantiate(hexPrefab_RS2_iron, Vector3.zero, Quaternion.identity, this.transform);
                    break;

                    case 5:
                    newMapTile = Instantiate(hexPrefab_RS3_gel, Vector3.zero, Quaternion.identity, this.transform);
                    break;

                    case 6: 
                    newMapTile = Instantiate(hexPrefab_EnemyTile, Vector3.zero, Quaternion.identity, this.transform);
                    break;
                }

                newMapTile.GetComponent<Hex>().CreateMapTile(column, row);
            }
        }
    }
}