using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _minXScreenWidth;

    [SerializeField]
    private float _maxXScreenWidth;

    [SerializeField]
    private float _minYScreenHeight;

    [SerializeField]
    private float _maxYScreenHeight;

    [SerializeField]
    private int _pointValue;

    private PlayerScript _player;

    private Animator _animator;

    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _explosionClip;

    [SerializeField]
    private float _nextLaserHigh;

    [SerializeField]
    private float _nextLaserLow;

    public float _currTime;

    public float _nextFire;

    private bool _canFire;

    [SerializeField]
    private GameObject _laser;

    [SerializeField]
    private Vector3 _laserOffsetY;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _animator = transform.GetComponent<Animator>();
        _audioSource = transform.GetComponent<AudioSource>();
        if (_player == null)
        {
            Debug.LogError("Player is null");
        }
        if (_animator == null)
        {
            Debug.LogError("Animator is null");
        }
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource is Null");
        }
        else
        {
            _audioSource.clip = _explosionClip;
        }
        _nextFire = Time.time + Random.Range(_nextLaserLow, _nextLaserHigh);
        _canFire = true;
    }

    void Update()
    {
        CalculateMovement();
        ShootLaser();
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < _minYScreenHeight)
        {
            float randXPos = Random.Range(_minXScreenWidth, _maxXScreenWidth);
            transform.position =
                new Vector3(randXPos, _maxYScreenHeight, transform.position.z);
        }
    }

    private void ShootLaser()
    {
        if (_canFire && Time.time > _nextFire)
        {
            _nextFire += Random.Range(_nextLaserLow, _nextLaserHigh);
            Instantiate(_laser,
            transform.position + _laserOffsetY,
            Quaternion.Euler(0, 0, 180));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _canFire = false;
            PlayerScript player = other.transform.GetComponent<PlayerScript>();
            if (player != null)
            {
                player.Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audioSource.Play();
            Destroy(transform.gameObject, 1f);
        }
        else if (other.tag == "Laser")
        {
            _canFire = false;
            Destroy(other.transform.gameObject);
            if (_player != null)
            {
                _player.AddToScore (_pointValue);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(transform.gameObject, 1f);
        }
    }
}
