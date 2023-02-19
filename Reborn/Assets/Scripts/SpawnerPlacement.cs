using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reborn
{
    public class SpawnerPlacement : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private MapGenerator mapGenerator;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] public Transform enemyContainer;

        // Enemy Spawner
        [SerializeField] public GameObject spawnerPrefab;
        [SerializeField] private List<Vector2Int> spawnerPos = new List<Vector2Int>();
        private List<Vector2Int> newSpawnerPos = new List<Vector2Int>();
        private List<GameObject> spawnerObjs = new List<GameObject>();

        private int level;

        private bool waveComplete = false;
        public bool WaveComplete 
        { 
            get => waveComplete;
            set
            {
                if (waveComplete != value)
                {
                    waveComplete = value;

                    if (waveComplete)
                    {
                        
                        
                    }
                }
            }
        }

        public void PlaceSpawner(int n, GameObject atlasObj, GameObject playerObj, int level)
        {
            this.level = level;
            AddSpawner(n);
            SpawnerPositionInGrid();
            SpawnSpawner(atlasObj, playerObj);
        }

        private void AddSpawner(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Vector2Int pos = SpawnerPositionInGrid();
                newSpawnerPos.Add(pos);
            }
        }

        private Vector2Int SpawnerPositionInGrid()
        {
            Vector2Int centerPos = mapGenerator.centerPosInGrid;
            do
            {
                int x = Random.Range(0, mapGenerator.mapWidth);
                int y = Random.Range(0, mapGenerator.mapHeight);

                if (x <= centerPos.x + 8 && x >= centerPos.x - 8 && y <= centerPos.y + 8 && y >= centerPos.y - 8)
                {
                    continue;
                }

                if (mapGenerator.tiles[x, y] != MapGenerator.TileType.floor)
                {
                    continue;
                }
                
                Vector2Int pos = new Vector2Int(x, y);
                if (spawnerPos.Contains(pos) || newSpawnerPos.Contains(pos))
                {
                    continue;
                }

                return pos;
            } while (true);
        }

        private void SpawnSpawner(GameObject atlasObj, GameObject playerObj)
        {
            foreach (var pos in newSpawnerPos)
            {
                GameObject spawner = Instantiate(spawnerPrefab, new Vector3((float)pos.x * 2 + 1f, 0.05f, (float)pos.y * 2 + 1f), Quaternion.identity, this.transform);
                EnemySpawner enemySpawnerScript = spawner.GetComponent<EnemySpawner>();
                enemySpawnerScript.spawnerPlacement = this;
                enemySpawnerScript.player = playerObj;
                enemySpawnerScript.altas = atlasObj;
                enemySpawnerScript.scoreManager = scoreManager;
                enemySpawnerScript.enemyContainer = enemyContainer;
                spawnerPos.Add(pos);
                spawnerObjs.Add(spawner);
            }
            newSpawnerPos.Clear();
        }


        int counter = 0;
        public void SpawnerCompleteWave()
        {
            counter++;
            Debug.Log(counter + "/" + spawnerObjs.Count);
            if (counter == spawnerObjs.Count)
            {
                counter = 0;
                WaveComplete = true;
                return;
            }
        }

        public void ResetSpawners()
        {
            WaveComplete = false;
            foreach (var spawner in spawnerObjs)
            {
                EnemySpawner enemySpawner = spawner.GetComponent<EnemySpawner>();
                enemySpawner.level = level;
                enemySpawner.ResetSpawn();
            }
        }

    }
}