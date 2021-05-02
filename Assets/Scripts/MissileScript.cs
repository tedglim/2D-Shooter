using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float _upBoundY;

    private AudioSource _audioSource;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        if (transform.position.y > _upBoundY)
        {
            Destroy(transform.gameObject);
        }
    }
}
