using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Reborn
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] public SpawnerPlacement spawnerPlacement;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] public GameObject player;
        [SerializeField] public GameObject altas;
        [SerializeField] public ScoreManager scoreManager;
        [SerializeField] public Transform enemyContainer;

        [SerializeField] public int level = 1;
        [SerializeField] private float spawnInterval = 4f;
        [SerializeField] private int numberToSpawn = 10;

        private float accumTime = 0f;
        private int spawnedCount = 0;

        private bool finsihSpawn = false;
        public bool FinishSpawn 
        { 
            get => finsihSpawn;
            set
            {
                if (finsihSpawn != value)
                {
                    finsihSpawn = value;
                    if (finsihSpawn)
                    {
                        spawnerPlacement.SpawnerCompleteWave();
                    }
                }
            }
                 
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Debug.Log(name + " |To spawn: " + spawnedCount + "|Over needed :" + numberToSpawn);
            if (spawnedCount < numberToSpawn)
            {
                accumTime += Time.fixedDeltaTime;
                if (accumTime >= spawnInterval)
                {
                    GameObject spawnedEnemy = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity, enemyContainer);
                    spawnedEnemy.SetActive(false);
                    BadGuy badGuy = spawnedEnemy.GetComponent<BadGuy>();
                    badGuy.Player = player;
                    badGuy.Altas = altas;
                    badGuy.scoreManager = scoreManager;
                    badGuy.level = level;
                    spawnedEnemy.SetActive(true);
                    spawnedCount++;
                    accumTime = 0f;
                }
            }
            else
            {
                FinishSpawn = true;
            }
        }

        public void ResetSpawn()
        {
            FinishSpawn = false;
            spawnedCount = 0;
        }

    }
}

