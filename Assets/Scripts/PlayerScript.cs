using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _upBoundX;

    [SerializeField]
    private float _lowBoundX;

    [SerializeField]
    private float _upBoundY;

    [SerializeField]
    private float _lowBoundY;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private float _laserOffsetY;

    [SerializeField]
    private float _fireRate;

    private float _canFire = -1;

    [SerializeField]
    private int _lives = 3;

    private SpawnManagerScript _spawnManagerScript;

    private bool _canTripleShot;
    [SerializeField]
    private float _tripleShotDuration;

    void Start()
    {
        _spawnManagerScript =
            GameObject.Find("Spawn_Manager").GetComponent<SpawnManagerScript>();
        if (_spawnManagerScript == null)
        {
            Debug.LogError("Spawn Manager is Null");
        }
    }

    void Update()
    {
        MovePlayer();
        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    private void MovePlayer()
    {
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizInput, vertInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
        CreatePlayerBounds();
    }

    private void CreatePlayerBounds()
    {
        CreateVerticalBounds();
        CreateHorizontalWrap();
    }

    private void CreateVerticalBounds()
    {
        transform.position =
            new Vector3(transform.position.x,
                Mathf.Clamp(transform.position.y, _lowBoundY, _upBoundY),
                transform.position.z);
    }

    private void CreateHorizontalWrap()
    {
        if (transform.position.x > _upBoundX)
        {
            transform.position =
                new Vector3(-_upBoundX,
                    transform.position.y,
                    transform.position.z);
        }
        else if (transform.position.x < _lowBoundX)
        {
            transform.position =
                new Vector3(-_lowBoundX,
                    transform.position.y,
                    transform.position.z);
        }
    }

    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        Vector3 laserStartPos =
            new Vector3(transform.position.x,
                transform.position.y + _laserOffsetY,
                transform.position.z);
        if (_canTripleShot)
        {
            Instantiate(_tripleShotPrefab, laserStartPos, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, laserStartPos, Quaternion.identity);
        }
    }

    public void Damage()
    {
        _lives--;
        if (_lives < 1)
        {
            Destroy(transform.gameObject);
            _spawnManagerScript.OnPlayerDeath();
        }
    }

    public void TurnOnTripleShot()
    {
        _canTripleShot = true;
        StartCoroutine(DecayTripleShotTime());
    }

    IEnumerator DecayTripleShotTime()
    {
        yield return new WaitForSeconds(_tripleShotDuration);
        _canTripleShot = false;
    }
}
