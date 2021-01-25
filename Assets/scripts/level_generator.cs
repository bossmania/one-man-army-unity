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
    public Tilemap water_tilemap;
    public Tile water_tile;
    public GameObject elevator_prefab;
    public GameObject darkness;
    public GameObject trap;
    public GameObject claymore;
    public Transform player_trans;
    public player player_scr;
    public Transform camera_trans;

    int enemy = 0;
    int maxEnemies = 0;
    float wait = 2f;
    bool scanned = false;
    string type = null;
    int FLOOR = 1;
    int WALL = 2;
    int START = 3;
    int END = 4;

    void Start()
    {

        player_trans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player_scr = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        camera_trans = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

        Object[] enemies = Resources.LoadAll("Prefabs/enemies");
        maxEnemies = player_scr.gameLevel + Random.Range(0, 3);

        if (player_scr.gameLevel > 3) {
            int num = Random.Range(0, 10);
            if (num <= 6) {
                type = null;
            } else {
                if (num == 7) {
                    type = "dark";
                } else if (num == 8) {
                    type = "water";
                } else if (num == 9) {
                    type = "claymore";
                }
            }
        }

        Debug.Log(type);

        int[,] grid = new int[GRID_SIZE, GRID_SIZE];
        int x = GRID_SIZE/2;
        int y = GRID_SIZE/2;

        while (true) {
            try
            {
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
                    x = Mathf.Clamp(x, 3, GRID_SIZE-3);
                    y = Mathf.Clamp(y, 3, GRID_SIZE-3);
                    grid[x, y] = FLOOR;
                }

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


                    int rdir = Random.Range(1, 5);
                    int xx = 0;
                    int yy = 0;

                    while (true) {
                        switch (rdir) {
                            case 1:
                                xx++;
                            break;
                            case 2:
                                yy++;
                            break;
                            case 3:
                                xx--;
                            break;
                            case 4:
                                yy--;
                            break;
                        }
                        if (grid[GRID_SIZE/2 + xx, GRID_SIZE/2 + yy] == WALL) {
                            grid[GRID_SIZE/2 + xx, GRID_SIZE/2 + yy] = START;
                            break;
                        }
                    }

                    rdir = Random.Range(1, 5);
                    xx = 0;
                    yy = 0;

                    while (true) {
                        switch (rdir) {
                            case 1:
                                xx++;
                            break;
                            case 2:
                                yy++;
                            break;
                            case 3:
                                xx--;
                            break;
                            case 4:
                                yy--;
                            break;
                        }
                        if (grid[x + xx, y + yy] == WALL) {
                            grid[x + xx, y + yy] = END;
                            break;
                        }
                    }

                    for (int i=0; i<GRID_SIZE; i++) {
                        for (int j=0; j<GRID_SIZE; j++) {
                            if (grid[i, j] == START) {
                                Vector3 pos = floor_tilemap.CellToWorld(new Vector3Int(i, j, -3)) + new Vector3(1.5f, 1.5f, 0);
                                Quaternion dire = Quaternion.identity;
                                if (grid[i+1, j] == FLOOR) dire = Quaternion.Euler(0, 0, 0);
                                if (grid[i-1, j] == FLOOR) dire = Quaternion.Euler(0, 0, 180);
                                if (grid[i, j+1] == FLOOR) dire = Quaternion.Euler(0, 0, 90);
                                if (grid[i, j-1] == FLOOR) dire = Quaternion.Euler(0, 0, 270);
                                GameObject elevator_obj = Instantiate(elevator_prefab, pos, dire);
                                elevator elevator_scr = elevator_obj.GetComponent<elevator>();
                                elevator_scr.type = "start";
                                player_trans.position = pos;
                                camera_trans.position = new Vector3(pos.x, pos.y, camera_trans.position.z);
                            }
                            if (grid[i, j] == END) {
                                Vector3 pos = floor_tilemap.CellToWorld(new Vector3Int(i, j, -3)) + new Vector3(1.5f, 1.5f, 0);
                                Quaternion dire = Quaternion.identity;
                                if (grid[i+1, j] == FLOOR) dire = Quaternion.Euler(0, 0, 0);
                                if (grid[i-1, j] == FLOOR) dire = Quaternion.Euler(0, 0, 180);
                                if (grid[i, j+1] == FLOOR) dire = Quaternion.Euler(0, 0, 90);
                                if (grid[i, j-1] == FLOOR) dire = Quaternion.Euler(0, 0, 270);
                                GameObject elevator_obj2 = Instantiate(elevator_prefab, pos, dire);
                                elevator elevator_scr = elevator_obj2.GetComponent<elevator>();
                                elevator_scr.type = "exit";
                            }
                        }
                    }

                for (int i=0; i<GRID_SIZE; i++) {
                    for (int j=0; j<GRID_SIZE; j++) {
                        if (grid[i, j] == FLOOR) {
                            if (type == "water" && Random.Range(1, 25) == 1) {
                                water_tilemap.SetTile(new Vector3Int(i, j, 0), water_tile);
                            } else {
                                floor_tilemap.SetTile(new Vector3Int(i, j, 0), floor_tile);
                            }

                            if (type == "claymore" && Random.Range(1, 25) == 1) {
                                Instantiate(claymore, floor_tilemap.CellToWorld(new Vector3Int(i, j, -3)) + new Vector3(0.5f, 0.5f, 0), Quaternion.Euler(0, 0, 90*Random.Range(0, 4)));
                            }

                            float dis = Vector3.Distance(floor_tilemap.CellToWorld(new Vector3Int(i, j, -3)), player_trans.position);
                            if (Random.Range(1, 20) == 1 && dis >= 20 && enemy < maxEnemies) {
                                int type = Random.Range(0, enemies.Length-1);
                                Instantiate(enemies[type], floor_tilemap.CellToWorld(new Vector3Int(i, j, -3)), new Quaternion(0, 0, 0, 0));
                                enemy++;
                            }
                        }
                        if (grid[i, j] == WALL) {
                            wall_tilemap.SetTile(new Vector3Int(i, j, 0), wall_tile);
                        }
                    }
                }

                if (type == "dark") {
                    Instantiate(darkness, camera_trans.position, camera_trans.rotation);
                }
                break;

            }
            catch (System.IndexOutOfRangeException e)
            {
                Debug.Log("We avoided a level that didnt generated");
                Debug.Log(e);
                continue;
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
