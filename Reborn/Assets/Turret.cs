using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Reborn
{

    public class Turret : MonoBehaviour
    {
        public enum TurretState
        {
            idle,
            attack
        }

        [SerializeField] private Transform gunRotation;
        [SerializeField] private Transform firingDir;
        [SerializeField] private Transform firingPos;
        [SerializeField] private float _Range = 8f;
        [SerializeField] private float _RotateSpeed = 180f;
        [SerializeField] private float _Damage = 0.5f;
        [SerializeField] private float _FireRate = 3.0f;
        [SerializeField] private TurretState _State = TurretState.idle;
        [SerializeField] public BulletPool bulletPool;

        public Vector3 gunDirection
        {
            get => gunRotation.forward;
            set => gunRotation.forward = value;
        }

        public float Range
        {
            get => _Range;
            set => _Range = value;
        }

        public float Damage
        {
            get => _Damage;
            set => _Damage = value;
        }

        public float FireRate
        {
            get => _FireRate;
            set => _FireRate = value;
        }

        public TurretState State 
        {
            get => _State;
            set => _State = value;
        }

        private GameObject target;
        private Vector3 targetFaceDirection;

        // Update is called once per frame
        void Update()
        {
            FireRate -= Time.deltaTime;
            FireRate = Mathf.Clamp(FireRate, 0, 10);

            int enemyLayer = LayerMask.GetMask("Enemy");
            Collider[] enemys = Physics.OverlapSphere(this.transform.position, _Range, enemyLayer);
            if (enemys.Length > 0)
            {
                float[] distance = new float[enemys.Length];
                for (int i = 0; i < enemys.Length; i++)
                {
                    distance[i] = Vector3.Distance(this.transform.position, enemys[i].transform.position);
                }

                target = enemys[ClosetEnemyIndex(distance)].gameObject;

                targetFaceDirection = target.transform.position - this.transform.position;
                targetFaceDirection.y = 0;
                if (targetFaceDirection != Vector3.zero)
                {
                    gunDirection = Vector3.RotateTowards(gunDirection, targetFaceDirection, _RotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);
                }
                State = TurretState.attack;
            }
            else
            {
                State = TurretState.idle;
            }

            if (State == TurretState.attack && FireRate == 0)
            {
                FireRate = 3f;
                GameObject bullet = bulletPool.GetBullet();
                if (bullet != null)
                {
                    bullet.transform.position = firingPos.transform.position;
                    bullet.transform.rotation = firingDir.transform.rotation;
                    bullet.SetActive(true);
                }
            }

        }

        private int ClosetEnemyIndex(float[] dis)
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



#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(gunRotation.position, gunRotation.position + gunDirection * 2f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gunRotation.position, _Range);
        }
#endif
    }
}