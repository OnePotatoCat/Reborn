using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Reborn
{
    public class BadGuy : MonoBehaviour
    {
        public enum EnemyState
        {
            move,
            attack
        }

        public NavMeshAgent agent;
        [SerializeField] public GameObject Player;
        [SerializeField] public GameObject Altas;
        [SerializeField] private float _MovementSpeed = 1.5f;
        [SerializeField] private float _MovementSpeedMultiplier = 1f;
        [SerializeField] private float _RotateSpeed = 130f;
        [SerializeField] private Transform _Rotation;

        [SerializeField] private float _Health;

        [SerializeField] private EnemyState _State = EnemyState.move;
        [SerializeField] private float sightRange = 8f;
        [SerializeField] private float attakRange = 1f;

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

        public EnemyState State
        {
            get => _State;
            set => _State = value;
        }

        
        private GameObject target;
        private int playerLayer;
        const float posUpdate = 0.8f;
        private float posUpdateTimer;

        private void Start()
        {
            posUpdateTimer = posUpdate;
            playerLayer = LayerMask.GetMask("Player");
            agent.speed = _MovementSpeedMultiplier * _MovementSpeed;
        }

        // Update is called once per frame
        void Update()
        {
            // Check if in range of any players objects
            Collider[] players = Physics.OverlapSphere(this.transform.position, sightRange, playerLayer);
            if (players.Length > 0)
            {
                float[] distance = new float[players.Length];
                for (int i = 0; i < players.Length; i++)
                {
                    distance[i] = Vector3.Distance(this.transform.position, players[i].transform.position);
                }

                State = EnemyState.attack;
                target = players[ClosetTargetIndex(distance)].gameObject;
            }
            else
            {
                State = EnemyState.move;
                target = Altas;
            }


            Vector3 moveDirection = new Vector3(target.transform.position.x  - this.transform.position.x, 0f, target.transform.position.z - this.transform.position.z);
            moveDirection = moveDirection.normalized;

            posUpdateTimer -= Time.deltaTime;
            if (posUpdateTimer <= 0)
            {
                posUpdateTimer = posUpdate;
                agent.SetDestination(target.transform.position);
            }
            
            if (moveDirection != Vector3.zero)
            {
                FaceDirection = Vector3.RotateTowards(FaceDirection, moveDirection, _RotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);
            }
        }

        private int ClosetTargetIndex(float[] dis)
        {
            int minIndex = 0;
            float minDist = dis[0];
            for (int i = 1; i < dis.Length; ++i)
            {
                if (dis[i] < minDist)
                {
                    minDist = dis[i];
                    minIndex = i;
                }
            }
            return minIndex;
        }

        public void UpgradeSpeed()
        {
            _MovementSpeedMultiplier += 0.1f;
            agent.speed = _MovementSpeedMultiplier * _MovementSpeed;
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + FaceDirection * 2f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, sightRange);
        }
#endif
    }
}
