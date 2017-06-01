using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EndlessTile where tileCount creates finite number of tiles any cycles them according to position
/// </summary>
public class EndlessTile : MonoBehaviour
{
    public  Tile[]       tilePrefabs;
    public  GameObject[] obstaclePrefabs;
    public  int          tileCount = 20;
    public  int          tileSize  = 20;
    public  int[]        lanes     = new int[]{ -3, 0, 3 };

    private int          offset;
    private Tile[]       tiles;
    private GameObject[] obstacles;
    private GameObject   tileContainer;

	void Start ()
    {
        tileContainer = new GameObject("Tiles");
        tileContainer.transform.parent = transform;
        tileContainer.transform.localPosition = Vector3.zero;

        initObstacles();
        initTiles();
    }

    /// <summary>
    /// obstacles created randomly from obstaclePrefabsArray, obstacles will be created exactly same amount as tiles
    /// </summary>
    private void initObstacles()
    {
        obstacles = new GameObject[tileCount];

        for (int num = 0; num < tileCount; num++)
        {
            obstacles[num] = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)]) as GameObject;
            obstacles[num].SetActive(false);
        }
    }

    /// <summary>
    /// tiles created randomly from tilePrefabs array,amount shoud be set from tilecount from beginning
    /// </summary>
    private void initTiles()
    {
        tiles = new Tile[tileCount];

        for (int num = 0; num < tileCount; num++)
        {
            tiles[num] = Instantiate<Tile>(tilePrefabs[Random.Range(0, tilePrefabs.Length)]) as Tile;
            tiles[num].transform.parent = tileContainer.transform;
        }
        shuffleTiles();
    }

    /// <summary>
    /// helper method to shuffle array, required for shuffle tiles array
    /// </summary>
    private void shuffleArray<T>(T[] arr)
    {
        for (int num = 0; num < arr.Length; num++)
        {
            T   tmp   = arr[num];
            int indx  = Random.Range(0, arr.Length);
            arr[num ] = arr[indx];
            arr[indx] = tmp;
        }
    }


    /// <summary>
    /// shuffles tiles and repositions them, first tile willn't have any obstacle
    /// </summary>
    public void shuffleTiles()
    {
        shuffleArray<Tile>(tiles);

        for (int num = 0; num < tiles.Length; num++)
        {
            tiles[num].setTilePosition(num * tileSize);
            tiles[num].setObstacle((num == 0) ? null : obstacles[num], lanes[Random.Range(0, lanes.Length)]);
        }
    }

    /// <summary>
    /// updates tiles by position, if tile position will be below -tilesize it readds first tile at the end of the tiles
    /// </summary>
    public void updateTiles(float pos)
    {
        tileContainer.transform.localPosition = new Vector3(-pos, 0, 0);

        if (Mathf.Abs(pos) >= (offset + 1) * tileSize)
        {
            int index = (offset >= tiles.Length) ? offset - (Mathf.FloorToInt(offset / tiles.Length) * tiles.Length) : offset;
            
            tiles[index].setTilePosition((tiles.Length + offset) * tileSize);
            tiles[index].setObstacle(obstacles[index], lanes[Random.Range(0, lanes.Length)]);
            offset++;
        }
    }

    /// <summary>
    /// resets everything and shuffles tiles
    /// </summary>
    public void reset()
    {
        offset = 0;
        tileContainer.transform.localPosition = Vector3.zero;
        shuffleTiles();
    }
}
