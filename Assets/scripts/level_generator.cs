using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class level_generator : MonoBehaviour
{
    public int GRID_SIZE = 50;
    public int tiles = 500;
    public Tilemap floor_tilemap;
    public Tile floor_tile;
    public Tilemap wall_tilemap;
    public Tile wall_tile;
    public GameObject elevator_prefab;
    public Transform player_trans;
    public Transform camera_trans;
    float wait = 1f;
    bool scanned = false;
    int FLOOR = 1;
    int WALL = 2;
    int START = 3;
    int END = 4;

    void Start()
    {
        Object[] enemies = Resources.LoadAll("Prefabs/enemies");

        int[,] grid = new int[GRID_SIZE, GRID_SIZE];
        int x = GRID_SIZE/2;
        int y = GRID_SIZE/2;

        for (int i=0; i<tiles; i++) {
            int dir = Random.Range(1, 5);
            switch (dir) {
                case 1:
                    x++;
                break;
                case 2:
                    y++;
                break;
                case 3:
                    x--;
                break;
                case 4:
                    y--;
                break;
            }
            x = Mathf.Clamp(x, 2, GRID_SIZE-2);
            y = Mathf.Clamp(y, 2, GRID_SIZE-2);
            grid[x, y] = FLOOR;
        }

        grid[GRID_SIZE/2, GRID_SIZE/2] = START;
        grid[x, y] = END;

        for (int i=1; i<GRID_SIZE-1; i++) {
            for (int j=1; j<GRID_SIZE-1; j++) {
                if (grid[i, j] == FLOOR) {
                    if (grid[i+1, j] == 0) grid[i+1, j] = WALL;
                    if (grid[i-1, j] == 0) grid[i-1, j] = WALL;
                    if (grid[i, j+1] == 0) grid[i, j+1] = WALL;
                    if (grid[i, j-1] == 0) grid[i, j-1] = WALL;
                    if (grid[i+1, j+1] == 0) grid[i+1, j+1] = WALL;
                    if (grid[i+1, j-1] == 0) grid[i+1, j-1] = WALL;
                    if (grid[i-1, j+1] == 0) grid[i-1, j+1] = WALL;
                    if (grid[i-1, j-1] == 0) grid[i-1, j-1] = WALL;
                }
            }
        }

        for (int i=1; i<GRID_SIZE-1; i++) {
            for (int j=1; j<GRID_SIZE-1; j++) {
                if (grid[i, j] == WALL) {
                    if (grid[i+1, j] == FLOOR) {
                        if (grid[i-1, j] == FLOOR) {
                            if (grid[i, j+1] == FLOOR) {
                                if (grid[i, j-1] == FLOOR) {
                                    grid[i, j] = FLOOR;
                                }
                            }
                        }
                    }
                 }
            }
        }

        for (int i=0; i<GRID_SIZE; i++) {
            for (int j=0; j<GRID_SIZE; j++) {
                if (grid[i, j] == FLOOR) {
                    floor_tilemap.SetTile(new Vector3Int(i, j, 0), floor_tile);
                    if (Random.Range(1, 20) == 1) {
                        int type = Random.Range(0, enemies.Length-1);
                        Instantiate(enemies[type], floor_tilemap.CellToWorld(new Vector3Int(i, j, -3)), new Quaternion(0, 0, 0, 0));
                    }
                }
                if (grid[i, j] == WALL) {
                    wall_tilemap.SetTile(new Vector3Int(i, j, 0), wall_tile);
                }
                if (grid[i, j] == START) {
                    Vector3 pos = floor_tilemap.CellToWorld(new Vector3Int(i, j, -3));
                    Instantiate(elevator_prefab, pos, new Quaternion(0, 0, 0, 0));
                    player_trans.position = pos + new Vector3(1.5f, 1.5f, 0);
                    camera_trans.position = new Vector3(pos.x, pos.y, camera_trans.position.z);
                }
                if (grid[i, j] == END) {
                    Vector3 pos = floor_tilemap.CellToWorld(new Vector3Int(i, j, -3));
                    Instantiate(elevator_prefab, pos, new Quaternion(0, 0, 0, 0));
                }
            }
        }
    }

    void Update() {
        wait -= Time.deltaTime;
        if (wait < 0 && scanned == false) {
            AstarPath.active.Scan();
            scanned = true;
        }
    }
}
