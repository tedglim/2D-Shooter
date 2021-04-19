using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _upBoundY;

    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y > _upBoundY)
        {
            if (transform.parent != null)
            {
                Transform parent = transform.parent;
                Destroy(parent.gameObject);
            }
            Destroy(transform.gameObject);
        }
    }
}
