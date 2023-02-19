using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reborn
{
    public class GameManager : MonoBehaviour
    {
        public enum GameState
        {   
            intro,
            game_round,
            next_round            
        }

        // Managers and object pools
        [SerializeField] private MapGenerator mapGenerator;
        [SerializeField] private SpawnerPlacement spawnerPlacement;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private BulletPool bulletPool;
        [SerializeField] private NavigationBaker nb;
        [SerializeField] private HealthBar AtlasHPBar;
        [SerializeField] private HealthBar PlayerHPBar;
        [SerializeField] private Camera SecondaryCamera;
        [SerializeField] private MinimanCamera minimapCamera;
        [SerializeField] private RespawnText respawnText;
        [SerializeField] private GameObject gameOverUI;

        // Atlas Game Object
        [SerializeField] private GameObject atlasPrefab;
        private GameObject atlasObj;

        // Player Game Object
        [SerializeField] private GameObject playerPrefab;
        private GameObject playerObj;

        // Enemy Spawner
        [SerializeField] public GameObject spawnerPrefab;
        [SerializeField] private int numberOfSpawner = 1;
        private List<Vector2Int> spawnerPos = new List<Vector2Int>();
        public List<Vector2> SpawnerActPos;

        // Spawner and Enemy Containers
        public Transform spawnerContainer;
        public Transform enemyContainer;

        Atlas atlas;
        PlayerBehavior playerBehavior;
        public float respawnTimer = 3f;

        public int point = 0;
        private GameState gameState = GameState.intro;
        private int level = 1;

        public GameState State
        {
            get => gameState;
            set
            {
                if (gameState != value)
                {
                    gameState = value;
                    switch (gameState)
                    {
                        case GameState.next_round:

                            break;

                        case GameState.game_round:
                            break;
                    }
                }
            }
        }

        private bool enemyClear = false;

        public bool EnemyClear
        {
            get => enemyClear;
            set
            {
                if (enemyClear != value)
                {
                    enemyClear = value;
                }
            }

        }

        //TO REPLACE
        float waitTime = 0f;
        float nextRoundTimer = 3f;

        private void Awake()
        {
            // generate map
            mapGenerator.GenerateMap();

            // place atlas
            Vector3 altasPos = new Vector3(mapGenerator.CenterPos.x, 1, mapGenerator.CenterPos.y);
            atlasObj = Instantiate(atlasPrefab, altasPos, Quaternion.identity);
            atlas = atlasObj.GetComponent<Atlas>();
            atlas.gameManager = this.GetComponent<GameManager>();
            SecondaryCamera = atlas.SecondaryCamera;
            atlas.healthBar = AtlasHPBar;

            // place player
            playerObj = Instantiate(playerPrefab, atlas.SpawnLocation.transform.position, Quaternion.identity);
            atlas.player = playerObj;
            playerBehavior = playerObj.GetComponent<PlayerBehavior>();
            playerBehavior.gameManager = this.GetComponent<GameManager>();
            playerBehavior.atlas = atlas;
            playerBehavior.healthBar = PlayerHPBar;
            playerBehavior.nb = nb;
            playerBehavior.bulletPool = this.bulletPool;

            // place spawners
            spawnerPlacement.PlaceSpawner(1, atlasObj, playerObj, level);
            Time.timeScale = 0f;
            StartCoroutine(ReadyStart());

            // minimap
            minimapCamera.playerPos = playerObj.transform;
            minimapCamera.atlasPos = atlasObj.transform;
        }

        IEnumerator ReadyStart()
        {
            yield return new WaitForSecondsRealtime(3f);
            Time.timeScale =1f;
        }

        private void Update()
        {
            //Debug.Log(spawnerPlacement.WaveComplete);
            if (spawnerPlacement.WaveComplete && enemyContainer.childCount == 0 && !EnemyClear)
            {
                EnemyClear = true;
            }

            if (EnemyClear)
            {
                waitTime += Time.deltaTime;
                if (waitTime >= nextRoundTimer)
                {
                    //Disable countdown UI
                    EnemyClear = false;
                    PrepareNextLevel();
                    waitTime = 0;

                }
            }
        }

        public void PrepareNextLevel()
        {
            level++;
            Debug.Log("Preparing Wave " + level);
            spawnerPlacement.ResetSpawners();
            if (level % 2 == 0)
            {
                spawnerPlacement.PlaceSpawner(1, atlasObj, playerObj, level);
            }
            
        }

        public void PlayerDie()
        {
            respawnText.gameObject.SetActive(true);
            respawnText.start = true;
            StartCoroutine(RespawnPlayer());
        }

        public void GameOver()
        {
            int finalScore = scoreManager.score;
            gameOverUI.SetActive(true);
            gameOverUI.GetComponent<GameOverUI>().UpdateFinalScore(finalScore);
        }


        IEnumerator RespawnPlayer()
        {
            respawnTimer += 0.2f;
            yield return new WaitForSeconds(respawnTimer);
            atlas.Respawn();
        }

        IEnumerator WaitNextRound()
        {
            //Show CountDown UI
            yield return new WaitForSeconds(5f);
        }

    }
}