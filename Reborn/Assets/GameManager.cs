using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reborn
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MapGenerator mapGenerator;
        [SerializeField] private BulletPool bulletPool;
        [SerializeField] private GameObject atlasPrefab;
        [SerializeField] private GameObject playerPrefab;

        Atlas atlas;
        PlayerBehavior playerBehavior;

        private void Awake()
        {
            mapGenerator.GenerateMap();
            Vector3 altasPos = new Vector3(mapGenerator.CenterPos.x, 1, mapGenerator.CenterPos.y);

            GameObject altasObj = Instantiate(atlasPrefab, altasPos, Quaternion.identity);
            atlas = altasObj.GetComponent<Atlas>();

            GameObject playerObj = Instantiate(playerPrefab, atlas.SpawnLocation.transform);
            atlas.player = playerObj;
            playerBehavior = playerObj.GetComponent<PlayerBehavior>();
            playerBehavior.gameManager = this.GetComponent<GameManager>();
            playerBehavior.bulletPool = this.bulletPool;
        }

        public void PlayerDie()
        {
            atlas.Respawn();
        }

    }
}