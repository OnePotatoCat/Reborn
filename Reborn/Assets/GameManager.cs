using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reborn
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MapGenerator mapGenerator;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject altas;




        private void Awake()
        {
            mapGenerator.GenerateMap();
            Vector3 altasPos = new Vector3(mapGenerator.CenterPos.x, 1, mapGenerator.CenterPos.y);
            Instantiate(altas, altasPos, Quaternion.identity);
            player.transform.position = new Vector3(mapGenerator.CenterPos.x, player.transform.position.y, mapGenerator.CenterPos.y);
        }

        public void PlayerDie()
        {
            player.transform.position = new Vector3(mapGenerator.CenterPos.x, player.transform.position.y, mapGenerator.CenterPos.y);
        }

    }
}