using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Reborn
{    public class PlayerBehavior : MonoBehaviour
    {
        [SerializeField] private PlayerInput _PlayerInput;
        [SerializeField] public BulletPool bulletPool;
        [SerializeField] private Transform gunPos;
        [SerializeField] private Transform firingPos;

        [SerializeField] public GameManager gameManager;
        [SerializeField] private GameObject turretPrefab;

        [SerializeField] private float health = 5;


        private void Update()
        {
            if (_PlayerInput.IsAttack)
            {
                GameObject bullet = bulletPool.GetBullet();
                if (bullet != null)
                {
                    bullet.transform.position = firingPos.transform.position;
                    bullet.transform.rotation = gunPos.transform.rotation;
                    bullet.SetActive(true);
                }
            }

            if (_PlayerInput.IsSudoku)
            {
                CommitSoduku();
            }

        }

        public void TakeDamage(float damagePoint)
        {
            health -= damagePoint;
            if (health <= 0)
            {
                CommitSoduku();
            }
        }

        private void CommitSoduku()
        {
            health = 5;
            SpawnTurret();
            Die();
        }


        private void Die()
        {
            this.gameObject.SetActive(false);
            gameManager.PlayerDie();
        }

        private void SpawnTurret()
        {
            Vector3 pos = new Vector3 ((int)this.transform.position.x, 0.5f, (int)this.transform.position.z);
            GameObject newTurret = Instantiate(turretPrefab, pos, Quaternion.identity);
            newTurret.GetComponent<Turret>().bulletPool = this.bulletPool;
        }

    }
}