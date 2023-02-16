using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Reborn
{
    public class Atlas : MonoBehaviour
    {
        [SerializeField] public Transform SpawnLocation;
        public GameObject player;


        public void Respawn()
        {
            player.transform.position = SpawnLocation.position;
            player.SetActive(true);
        }
    }
}
