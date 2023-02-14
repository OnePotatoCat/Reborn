using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Reborn
{
    public class BadGuy : MonoBehaviour
    {
        [SerializeField] public GameObject Player;
        [SerializeField] private CharacterController _CharController;
        [SerializeField] private float _MovementSpeed = 1.5f;
        [SerializeField] private float _MovementSpeedMultiplier = 1f;
        [SerializeField] private float _RotateSpeed = 130f;
        [SerializeField] private Transform _Rotation;
        [SerializeField] private float _Health;

        public Vector3 FaceDirection
        {
            get => _Rotation.forward;
            set => _Rotation.forward = value;
        }

        public void TakeDamage(float damagePoint)
        {
            _Health -= damagePoint;
            if (_Health <= 0)
            {
                Destroy(this.gameObject);
            }
        }


        // Update is called once per frame
        void Update()
        {
            Vector3 moveDirection = new Vector3(Player.transform.position.x  - this.transform.position.x, 0f, Player.transform.position.z - this.transform.position.z);
            moveDirection = moveDirection.normalized;
            Vector3 motion = moveDirection * (_MovementSpeed * _MovementSpeedMultiplier * Time.deltaTime);
            _CharController.Move(motion);

            if (moveDirection != Vector3.zero)
            {
                FaceDirection = Vector3.RotateTowards(FaceDirection, moveDirection, _RotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + FaceDirection * 2f);
        }
#endif
    }
}
