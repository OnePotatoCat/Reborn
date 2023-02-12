using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reborn
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerInput PlayerInput;

        [SerializeField] private CharacterController _CharController;
        [SerializeField] private float _MovementSpeed = 5f;
        [SerializeField] private float _MovementSpeedMultiplier = 1f;
        [SerializeField] private float _RotateSpeed = 130f;
        [SerializeField] private Camera _Camera;
        [SerializeField] private Transform _Rotation;
        public Vector3 FaceDirection
        {
            get => _Rotation.forward;
            set => _Rotation.forward = value;
        }

        private Vector3 TargetFaceDirection { get; set; }

        // Update is called once per frame
        void Update()
        {
            if (PlayerInput.MoveAxis != Vector2.zero)
            {
                Vector3 moveDirection = new Vector3(PlayerInput.MoveAxis.x, 1f, PlayerInput.MoveAxis.y);
                moveDirection = moveDirection.normalized;
                Vector3 motion = moveDirection * (_MovementSpeed * _MovementSpeedMultiplier * Time.deltaTime);
                _CharController.Move(motion);
            }

            Vector3 TargetLocation = Vector3.zero;
            Ray ray = _Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                TargetLocation = new Vector3(raycastHit.point.x, 1f, raycastHit.point.z);
            }

            Vector3 position = transform.position;
            position.y = 1f;
            transform.position = position;
            TargetFaceDirection = TargetLocation - transform.position;

            if (TargetFaceDirection != Vector3.zero)
            {
                FaceDirection = Vector3.RotateTowards(FaceDirection, TargetFaceDirection, _RotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);
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