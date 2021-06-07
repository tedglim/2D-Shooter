using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmartScript : EnemyBaseScript
{
    [Header("Basic Enemy")]
    [SerializeField]
    private Vector2 _minMaxXSpawnUD;

    [SerializeField]
    private Vector2 _minMaxYSpawnUD;

    private bool canFireBehind;

    [SerializeField]
    private float fireRangeX;
    [SerializeField]
    private Vector3 _laserBehindOffsetY;

    void Update()
    {
        CalculateMovement();
        ShootLaser();
        ShootBehind();
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < _minMaxYSpawnUD[0])
        {
            float randXPos =
                Random.Range(_minMaxXSpawnUD[0], _minMaxXSpawnUD[1]);
            transform.position =
                new Vector3(randXPos, _minMaxYSpawnUD[1], transform.position.z);
        }
    }

    private void ShootBehind()
    {
        if (
            canFireBehind &&
            _player.transform.position.y > this.transform.position.y &&
            _player.transform.position.x >
            this.transform.position.x - fireRangeX &&
            _player.transform.position.x <
            this.transform.position.x + fireRangeX
        )
        {
            Instantiate(_laser,
            transform.position + _laserBehindOffsetY,
            Quaternion.identity);
            canFireBehind = false;
        } else if (_player.transform.position.y < this.transform.position.y)
        {
            canFireBehind = true;
        }
    }
}
