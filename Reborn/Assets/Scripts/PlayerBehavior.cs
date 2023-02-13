using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Reborn
{    public class PlayerBehavior : MonoBehaviour
    {
        [SerializeField] private PlayerInput _PlayerInput;
        [SerializeField] private BulletPool _BulletPool;
        [SerializeField] private Transform _gunPos;
        [SerializeField] private Transform _firingPos;

        private void Update()
        {
            if (_PlayerInput.IsAttack)
            {
                GameObject bullet = _BulletPool.GetBullet();
                if (bullet != null)
                {
                    bullet.transform.position = _firingPos.transform.position;
                    bullet.transform.rotation = _gunPos.transform.rotation;
                    bullet.SetActive(true);
                }
            }

            if (_PlayerInput.IsSpecial)
            {
                Debug.Log("Special");
            }
        }
    }
}