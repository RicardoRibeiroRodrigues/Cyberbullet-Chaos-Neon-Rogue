using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    const int tileSize = 22;
    private List<GameObject> world = new List<GameObject>();
    private GameObject player;
    private HashSet<Vector2Int> currentTiles = new HashSet<Vector2Int>();
    public float MinSwapDistance = 88f;
    // Current tiles
    private Vector2Int curretPlayerTile = new Vector2Int(0, 0);


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        // Create a inicial grid of tiles
        var grid_size = 8;
        currentTiles.Add(new Vector2Int(0, 0));
        for (int x = -grid_size; x <= grid_size; x++)
        {
            for (int y = -grid_size; y <= grid_size; y++)
            {
                if (x == 0 && y == 0) continue;
                // The new tile MUST be a child of the WorldGenerator
                AddTile(x * tileSize, y * tileSize);
                currentTiles.Add(new Vector2Int(x, y));
            }
        }
    }

    void AddTile(int x, int y)
    {
        GameObject tile = Instantiate(tilePrefabs[Random.Range(0, tilePrefabs.Length)]);
        tile.transform.position = new Vector3(x, y, 0);
        tile.transform.parent = transform;
        // Add tile to world list
        world.Add(tile);
    }

    void SwapTile(int x, int y)
    {
        for (int i = 0; i < world.Count; i++)
        {
            var tile = world[i];
            // Se a distancia entre o tile e o player for maior que MinSwapDistance, troca o tile de lugar
            if (Vector2.Distance(tile.transform.position, player.transform.position) >= MinSwapDistance)
            {
                var tile_x_num = (int)tile.transform.position.x / tileSize;
                var tile_y_num = (int)tile.transform.position.y / tileSize;
                // Remove tile from curretnTiles
                currentTiles.Remove(new Vector2Int(tile_x_num, tile_y_num));
                world[i].transform.position = new Vector3(x * tileSize, y * tileSize, 0);
                // Add tile to currentTiles
                currentTiles.Add(new Vector2Int(x, y));
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        
        var playerPos = player.transform.position;
        var currPlayerTileX = (int)playerPos.x / tileSize;
        var currPlayerTileY = (int)playerPos.y / tileSize;

        // So e necessario atualizar os tiles se o player mudou de tile
        if (currPlayerTileX == curretPlayerTile.x && currPlayerTileY == curretPlayerTile.y) return;
        curretPlayerTile = new Vector2Int(currPlayerTileX, currPlayerTileY);

        for (int x = currPlayerTileX - 1; x <= currPlayerTileX + 1; x++)
        {
            for (int y = currPlayerTileY - 1; y <= currPlayerTileY + 1; y++)
            {
                if (!currentTiles.Contains(new Vector2Int(x, y)))
                {
                    SwapTile(x, y);
                }
            }
        }
    }
}
