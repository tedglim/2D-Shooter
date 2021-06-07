using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroScript : EnemyBaseScript
{
    [Header("Aggro Enemy")]
    [SerializeField]
    private Vector2 _minMaxXSpawnUD;
    [SerializeField]
    private Vector2 _minMaxYSpawnUD;
    // private PlayerScript _player;
    [SerializeField]
    private float _aggroDistance;
    private bool _aggroSpeedOn;
    private float _initSpd;

    void Start() {

        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        if (_player == null)
        {
            Debug.LogError("Player is null");
        }
        _initSpd = _speed;
    }

    void Update()
    {
        if(!_aggroSpeedOn)
        {
            SetAggressive();
        } else 
        {
            SetPassive();
        }
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

    private void SetAggressive()
    {
        if(_player == null) return;
        if(Vector3.Distance(this.transform.position, _player.gameObject.transform.position) < _aggroDistance)
        {
            _aggroSpeedOn = true;
            _speed *= 2;
        } 
    }

    private void SetPassive()
    {
        if(_player == null) return;
        if(Vector3.Distance(this.transform.position, _player.gameObject.transform.position) >= _aggroDistance)
        {
            _aggroSpeedOn = false;
            _speed = _initSpd;
        } 
    }
}
