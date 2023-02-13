using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Reborn
{    public class PlayerBehavior : MonoBehaviour
    {
        [SerializeField] private PlayerInput _PlayerInput;
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Transform _gunPos;

        private void Update()
        {
            
            if (_PlayerInput.IsAttack)
            {
                GameObject bullet = GameObject.Instantiate(_bullet, _gunPos.transform.position, _gunPos.transform.rotation);
                //bullet.GetComponent<Bullet>().
            }

            if (_PlayerInput.IsSpecial)
            {
                Debug.Log("Special");
            }
        }
    }

}