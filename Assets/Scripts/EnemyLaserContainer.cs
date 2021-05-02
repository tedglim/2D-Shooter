using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserContainer : MonoBehaviour
{
    [SerializeField]
    private float _lowBoundY;
    void Update()
    {
        if(transform.childCount == 0)
        {
            Destroy(transform.gameObject);
        }
        if (transform.position.y < _lowBoundY)
        {
            Destroy(transform.gameObject);
        }
    }
}
