using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reborn
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private BulletPool bp;
        [SerializeField] private float _ShootSpeed = 12.0f;
        [SerializeField] private float damage = 1f;

        private float accumTime;

        private void OnCollisionEnter(Collision collision)
        {
            switch (collision.collider.tag)
            {
                case ("Enemy"):
                    // reduce enemy health
                    var enemy = collision.collider.gameObject.GetComponent<BadGuy>();
                    enemy.TakeDamage(damage);
                    this.gameObject.SetActive(false);
                    break;

                case ("Wall"):
                    this.gameObject.SetActive(false);
                    break;

                default: break;
            }

        }

        private void Awake()
        {
            accumTime = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position += transform.TransformDirection(Vector3.up) * _ShootSpeed * Time.deltaTime;
            accumTime += Time.deltaTime;

            if (accumTime > 90f)
            {
                accumTime = 0f;
                this.gameObject.SetActive(false);
            }
        }
    }

}