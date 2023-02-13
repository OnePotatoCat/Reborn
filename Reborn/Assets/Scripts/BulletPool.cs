using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reborn
{
    public class BulletPool : MonoBehaviour
    {
        public static BulletPool SharedInstance;
        public List<GameObject> pooledBullets;
        public GameObject bullet;
        public int amountToPool;

        // Start is called before the first frame update
        void Awake()
        {
            SharedInstance = this;
        }

        // Update is called once per frame
        void Start()
        {
            pooledBullets = new List<GameObject>();
            GameObject tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(bullet, this.transform);
                tmp.SetActive(false);
                pooledBullets.Add(tmp);
            }
        }

        public GameObject GetBullet()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                if (!pooledBullets[i].activeInHierarchy)
                {
                    return pooledBullets[i];
                }
            }
            return null;
        }
    }

}