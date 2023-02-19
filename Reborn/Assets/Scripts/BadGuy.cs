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
        [SerializeField] public ScoreManager scoreManager;
        [SerializeField] private float _MovementSpeed = 5f;
        [SerializeField] private float _RotateSpeed = 180f;
        [SerializeField] private Transform _Rotation;

        [SerializeField]
        private float _Health = 3f;
        private float damage = 1f;

        [SerializeField] private EnemyState _State = EnemyState.move;
        [SerializeField] private float sightRange = 8f;
        [SerializeField] private float attakRange = 1f;
        [SerializeField] public int level;

        // Sound Effects
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip[] explodeSFX;
        [SerializeField] private float sfxVolume = 1f;


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
                scoreManager.AddScore(1);
                StartCoroutine(SelfDestruct());
            }
        }

        public EnemyState State
        {
            get => _State;
            set => _State = value;
        }

        
        private GameObject target;
        private int playerLayer;
        private int turretLayer;
        const float posUpdate = 0.8f;
        private float posUpdateTimer;
        private float normalSpeed;
        private bool dead = false;

        private void Start()
        {
            posUpdateTimer = posUpdate;
            playerLayer = LayerMask.GetMask("Player");
            turretLayer = LayerMask.GetMask("Turret");
            _Health = _Health + Mathf.Floor(level / 3);
            damage = damage + Mathf.Floor(level / 4);
            normalSpeed =  _MovementSpeed * ( 1 + (level-1)/10);
            agent.speed = normalSpeed;
            agent.autoRepath = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (!dead)
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
                    agent.speed = normalSpeed * 2.5f;
                    State = EnemyState.attack;
                    target = players[ClosetTargetIndex(distance)].gameObject;
                    agent.isStopped = true;
                    transform.position = Vector3.MoveTowards(transform.position, players[ClosetTargetIndex(distance)].transform.position, normalSpeed * 1.5f * Time.deltaTime);

                }
                else
                {
                    agent.isStopped = false;
                    agent.speed = normalSpeed;
                    State = EnemyState.move;
                    target = Altas;
                }


                Vector3 moveDirection = new Vector3(target.transform.position.x - this.transform.position.x, 0f, target.transform.position.z - this.transform.position.z);
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

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.collider.name);
            if (collision.collider.tag == "Player")
            {
                // reduce enemy health
                var player = collision.collider.gameObject.GetComponent<PlayerBehavior>();
                player.TakeDamage(damage);
                StartCoroutine(SelfDestruct());
            }
            else if (collision.collider.tag == "Turret")
            {
                // reduce enemy health
                var turret = collision.collider.gameObject.GetComponent<Turret>();
                turret.TakeDamage(damage);
                StartCoroutine(SelfDestruct());
            }
            else if (collision.collider.tag == "Atlas")
            {
                // reduce enemy health
                var atlas = collision.collider.gameObject.GetComponent<Atlas>();
                atlas.TakeDamage(damage);
                StartCoroutine(SelfDestruct());
            }

        }

        IEnumerator SelfDestruct()
        {
            int clipToPlay = Random.Range(0, explodeSFX.Length);
            audioSource.PlayOneShot(explodeSFX[clipToPlay], sfxVolume);
            agent.isStopped=true;
            dead = true;
            yield return new WaitForSeconds(explodeSFX[clipToPlay].length);
            Destroy(this.gameObject);
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