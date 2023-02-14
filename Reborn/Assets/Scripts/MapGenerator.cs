using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//https://github.com/skylarbeaty/ProceduralGenerationOfNuclearThrone/blob/master/NuclearThroneGeneration/Assets/Scripts/LevelGenerator.cs
namespace Reborn
{
    public class MapGenerator : MonoBehaviour
    {
        public enum TileType
        {
            empty,
            floor,
            wall
        }

        [Header("Tile Grid")]
        [SerializeField] int mapHeight;
        [SerializeField] int  mapWidth;
        private TileType[,] tiles;

        private Vector2 _CenterPos;
        public Vector2 CenterPos
        {
            get => _CenterPos;
            set => _CenterPos = value;
        }

        struct Walker
        {
            public Vector2 dir;
            public Vector2 pos;
        }

        private List<Walker> walkers;
        private float chanceChangeDir = 0.5f;
        private float chanceDuplicate = 0.05f;
        private float chanceDestroy = 0.05f;
        private int maxWalker = 5;
        float percentToFill = 0.5f;
        int maxIteration = 50000;

        [SerializeField] private GameObject wallPrefab, floorPrefab;

        private void Setup()
        {
            //Generate empty tiles to be fill later
            tiles = new TileType[mapHeight, mapWidth];
            for (int x = 0; x < mapWidth - 1; x++)
            {
                for (int y = 0; y < mapHeight -1; y++)
                {
                    tiles[x, y] = TileType.empty;
                }
            }

            //Spawn walkers to populate floors
            walkers = new List<Walker>();
            Walker newWalker = new Walker();
            newWalker.dir = RandomDirection();
            Vector2 centerPos = new Vector2(Mathf.RoundToInt(mapWidth / 2.0f),
                                     Mathf.RoundToInt(mapHeight / 2.0f));

            _CenterPos = centerPos * 2 + new Vector2(1 ,1);

            // Set Center Stage
            for (int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2 ; y++)
                {
                    tiles[(int)centerPos.x + x, (int)centerPos.y + y] = TileType.floor;
                }
            }

            newWalker.pos = centerPos;
            walkers.Add(newWalker);

        }

        private void CreateFloors()
        {
            int iteration = 0;
            do
            {
                // Create floor on current walker position
                foreach (Walker myWalker in walkers)
                {
                    tiles[(int)myWalker.pos.x, (int)myWalker.pos.y] = TileType.floor;
                }

                // chance: Destroy walker
                int walkerCount = walkers.Count;
                for (int i = 0; i < walkerCount; i++)
                {
                    if (Random.value < chanceDestroy && walkerCount > 1)
                    {
                        walkers.RemoveAt(i);
                        break;
                    }
                }

                // chance: Change walker direction
                for (int i = 0; i < walkers.Count; i++)
                {
                    if (Random.value < chanceChangeDir)
                    {
                        Walker tempWalker = walkers[i];
                        tempWalker.dir = RandomDirection();
                        walkers[i] = tempWalker;
                    }
                }

                // chance: Duplicate walker
                walkerCount = walkers.Count;
                for (int i = 0; i < walkerCount; i++)
                {
                    if (Random.value < chanceDuplicate && walkers.Count < maxWalker)
                    {
                        Walker newWalker = new Walker();
                        newWalker.dir = RandomDirection();
                        newWalker.pos = walkers[i].pos;
                        walkers.Add(newWalker);
                    }
                }

                // Move walkers and clamp position within boarders
                for (int i = 0; i < walkers.Count; i++)
                {
                    Walker currentWalker = walkers[i];
                    currentWalker.pos += currentWalker.dir;
                    currentWalker.pos.x = Mathf.Clamp(currentWalker.pos.x, 1, mapWidth - 2);
                    currentWalker.pos.y = Mathf.Clamp(currentWalker.pos.y, 1, mapHeight - 2);
                    walkers[i] = currentWalker;
                }

                // Check percent of floors
                if ((float)NumberOfFloors() / (float)tiles.Length > percentToFill)
                {
                    break;
                }

                iteration++;

            } while (iteration < maxIteration);
        }

        void CreateWalls()
        {
            //loop though every grid space
            for (int x = 0; x < mapWidth - 1; x++)
            {
                for (int y = 0; y < mapHeight - 1; y++)
                {
                    //if theres a floor, check the spaces around it
                    if (tiles[x, y] == TileType.floor)
                    {
                        //if any surrounding spaces are empty, place a wall
                        if (tiles[x, y + 1] == TileType.empty)
                        {
                            tiles[x, y + 1] = TileType.wall;
                        }
                        if (tiles[x, y - 1] == TileType.empty)
                        {
                            tiles[x, y - 1] = TileType.wall;
                        }
                        if (tiles[x + 1, y] == TileType.empty)
                        {
                            tiles[x + 1, y] = TileType.wall;
                        }
                        if (tiles[x - 1, y] == TileType.empty)
                        {
                            tiles[x - 1, y] = TileType.wall;
                        }
                    }
                }
            }
        }

        private void RemoveSingleWalls()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (tiles[x,y] == TileType.wall)
                    {
                        for (int checkX = -1; checkX <= 1; checkX++)
                        {
                            for (int checkY = -1; checkY <= 1; checkY++)
                            {
                                // Skip checking corners and centers
                                if ((checkX != 0 && checkY != 0) || (checkX == 0 && checkY == 0))
                                {
                                    continue;
                                }

                                // Skip checking if out of bound
                                if (x + checkX < 0 || x + checkX > mapWidth -1 || y + checkY < 0 || y + checkY > mapHeight -1)
                                {
                                    continue;
                                }

                                // Find wall chunk group and remove them if the group are less than 3 walls 
                                List<(int, int)> coorList= new List<(int, int)>();
                                coorList = FindWallGroup(x + checkX, y + checkY, coorList);
                                if (!CheckWallGroupBesideEmpty(coorList) && coorList.Count < 3)
                                {
                                    foreach (var coor in coorList)
                                    {
                                        tiles[coor.Item1, coor.Item2] = TileType.floor;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Find the group of wall
        private List<(int, int)> FindWallGroup(int currentX, int currentY, List<(int, int)> listCoor) 
        {
            listCoor.Add((currentX, currentY));
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    // Skip checking corners and centers
                    if ((x != 0 && y != 0) || (x == 0 && y == 0))
                    {
                        continue;
                    }

                    // Skip checking if out of bound
                    if (x + currentX < 0 || x + currentX > mapWidth - 1 || y + currentY < 0 || y + currentY > mapHeight - 1)
                    {
                        continue;
                    }

                    // Check neighbouring tile is floor or not
                    if (tiles[x + currentX, y + currentY] == TileType.wall)
                    {
                        if (!listCoor.Contains((x + currentX, y + currentY))){
                            listCoor = FindWallGroup(x + currentX, y + currentY, listCoor);
                        }
                    }
                }
            }
            return listCoor;
        }

        private bool CheckWallGroupBesideEmpty(List<(int, int)> listCoor)
        {
            int currentX;
            int currentY;
            foreach (var coor in listCoor)
            {
                currentX = coor.Item1;
                currentY = coor.Item2;
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        // Skip checking corners and centers
                        if ((x != 0 && y != 0) || (x == 0 && y == 0))
                        {
                            continue;
                        }

                        // Skip checking if out of bound
                        if (x + currentX < 0 || x + currentX > mapWidth - 1 || y + currentY < 0 || y + currentY > mapHeight - 1)
                        {
                            return true;
                        }

                        // Check neighbouring tile is floor or not
                        if (tiles[x + currentX, y + currentY] == TileType.empty)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        } 
        
        private void SpawnMap()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    switch (tiles[x, y])
                    {
                        case TileType.wall:
                            SpawnObject(x, y, wallPrefab);
                            break;
                        case TileType.floor:
                            break;
                        case TileType.empty:
                            break;
                    }
                }
            }
        }

        private void SpawnObject(int x, int y, GameObject wallPrefab)
        {
            Instantiate(wallPrefab, new Vector3((float)x*2 + 1f, 1.5f, (float)y*2 + 1f), Quaternion.identity);
        }

        public void GenerateMap()
        {
            Setup();
            CreateFloors();
            CreateWalls();
            RemoveSingleWalls();
            SpawnMap();
        }

        Vector2 RandomDirection()
        {
            //pick random int between 0 and 3
            int choice = Mathf.FloorToInt(Random.value * 3.99f);
            //use that int to chose a direction
            switch (choice)
            {
                case 0:
                    return Vector2.down;    // (0, -1)
                case 1:
                    return Vector2.left;    // (-1, 0)
                case 2:
                    return Vector2.up;      // (0, 1)
                default:
                    return Vector2.right;   // (1, 0)
            }
        }

        int NumberOfFloors()
        {
            int count = 0;
            foreach (TileType tile in tiles)
            {
                if (tile == TileType.floor)
                {
                    count++;
                }
            }
            return count;
        }
    }
}