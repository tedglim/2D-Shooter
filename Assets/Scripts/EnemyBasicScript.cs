using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicScript : EnemyBaseScript
{
    [Header("Basic Enemy")]
    [SerializeField]
    private Vector2 _minMaxXSpawnUD;
    [SerializeField]
    private Vector2 _minMaxYSpawnUD;

    void Update()
    {
        CalculateMovement();
        ShootLaser();
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < _minMaxYSpawnUD[0])
        {
            float randXPos = Random.Range(_minMaxXSpawnUD[0], _minMaxXSpawnUD[1]);
            transform.position =
                new Vector3(randXPos, _minMaxYSpawnUD[1], transform.position.z);
        }
    }
}
