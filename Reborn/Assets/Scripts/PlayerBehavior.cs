using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Reborn
{    public class PlayerBehavior : MonoBehaviour
    {
        [SerializeField] private PlayerInput _PlayerInput;
        [SerializeField] private BulletPool _BulletPool;
        [SerializeField] private Transform gunPos;
        [SerializeField] private Transform firingPos;

        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject turretPrefab;

        [SerializeField] private int health = 5;


        private void Update()
        {
            if (_PlayerInput.IsAttack)
            {
                GameObject bullet = _BulletPool.GetBullet();
                if (bullet != null)
                {
                    bullet.transform.position = firingPos.transform.position;
                    bullet.transform.rotation = gunPos.transform.rotation;
                    bullet.SetActive(true);
                }
            }

            if (_PlayerInput.IsSpecial)
            {
                Debug.Log("Special");
            }

            if (health <= 0)
            {
                health = 5;
                SpawnTurret();
                Die();
            }
        }

        private void Die()
        {
            gameManager.PlayerDie();
        }

        private void SpawnTurret()
        {
            Vector3 pos = new Vector3 ((int)this.transform.position.x, 0.5f, (int)this.transform.position.z);
            GameObject newTurret = Instantiate(turretPrefab, pos, Quaternion.identity); 
        }

    }
}