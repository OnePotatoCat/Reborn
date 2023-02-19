using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reborn
{
    public class MinimanCamera : MonoBehaviour
    {
        [SerializeField] public Transform playerPos;
        [SerializeField] public Transform atlasPos;


        private void Update()
        {
            if (playerPos != null)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, 
                                                         new Vector3(playerPos.position.x, 48f, playerPos.position.z), 
                                                         25f * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(this.transform.position,
                                                         new Vector3(atlasPos.position.x, 48f, atlasPos.position.z),
                                                         25f * Time.deltaTime);
            }
        }
    }
}