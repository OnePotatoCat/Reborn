using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 _ShootDir;
    public Vector3 ShootDir
    {
        get => _ShootDir;
        set
        {
            var oldDir = _ShootDir;
            if (oldDir != value)
            {
                _ShootDir = value;
                //Update flying dirction
            }
        }
    }

    [SerializeField] private float _ShootSpeed = 2.0f;


    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //this.transform.position = this.transform.position + Vector3.forward*ShootDir * Time.deltaTime;
    }
}
