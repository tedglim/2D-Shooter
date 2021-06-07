using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveRLScript : EnemyBaseScript
{
    [Header("RL Movement Enemy")]
    [SerializeField]
    private Vector2 _minMaxXSpawnRL;
    [SerializeField]
    private EnemyType _moveType;

    void Update()
    {
        if(_moveType == EnemyType.Right)
        {
            CalculateMovementRight();
        } else if(_moveType == EnemyType.Left)
        {
            CalculateMovementLeft();
        }
        ShootLaser();
    }

    private void CalculateMovementRight()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);
        if (transform.position.x > _minMaxXSpawnRL[1])
        {
            transform.position = new Vector3(_minMaxXSpawnRL[0], transform.position.y, transform.position.z);
        }
    }

    private void CalculateMovementLeft()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime);
        if (transform.position.x < _minMaxXSpawnRL[0])
        {
            transform.position = new Vector3(_minMaxXSpawnRL[1], transform.position.y, transform.position.z);
        }
    }
}
