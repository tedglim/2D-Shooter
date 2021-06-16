using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour
{
    public enum EnemyType
    {
        Right,
        Left
    }

    [Header("Base Stats")]
    [SerializeField]
    protected float _speed;

    [SerializeField]
    private int _pointValue;

    [SerializeField]
    private Vector2 _laserFireCD;

    private float _nextFire;

    private bool _canFire;

    protected PlayerScript _player;

    protected Animator _animator;

    private AudioSource _audioSource;

    [Header("Misc")]
    [SerializeField]
    protected GameObject _laser;

    [SerializeField]
    private Vector3 _laserOffsetY;

    [SerializeField]
    private AudioClip _explosionClip;

    private Transform _shieldFX;

    void Awake()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _shieldFX = transform.Find("Shields_VFX_01");
        _animator = transform.GetComponent<Animator>();
        _audioSource = transform.GetComponent<AudioSource>();
        if (_player == null)
        {
            Debug.LogError("Player is null");
        }
        if (_shieldFX)
        {
            Debug.Log("No Shield");
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
        _nextFire = Time.time + Random.Range(_laserFireCD[0], _laserFireCD[1]);
        _canFire = true;
    }

    protected void ShootLaser()
    {
        if (_canFire && Time.time > _nextFire)
        {
            _nextFire += Random.Range(_laserFireCD[0], _laserFireCD[1]);
            Instantiate(_laser,
            transform.position + _laserOffsetY,
            Quaternion.Euler(0, 0, 180));
        }
    }

    //check shield behavior~!!!
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
            if (CheckShieldOn()) return;
            CleanupAndDie();
        }
        else if (other.tag == "Laser")
        {
            if (CheckShieldOn()) return;
            _canFire = false;
            Destroy(other.transform.gameObject);
            if (_player != null)
            {
                _player.AddToScore (_pointValue);
            }
            CleanupAndDie();
        }
        else if (other.tag == "Missile")
        {
            if (CheckShieldOn()) return;
            _canFire = false;
            if (_player != null)
            {
                _player.AddToScore (_pointValue);
            }
            CleanupAndDie();
        } else if (other.tag == "PowerupDetectorCollider")
        {
            return;
        }
    }

    private void CleanupAndDie()
    {
        _animator.SetTrigger("OnEnemyDeath");
        _speed = 0f;
        _audioSource.Play();

        // AudioSource.PlayClipAtPoint(_audioSource.clip, transform.position);
        Destroy(GetComponent<Collider2D>());
        Destroy(transform.gameObject, 1f);
    }

    private bool CheckShieldOn()
    {
        if (_shieldFX == null) return false;
        if (_shieldFX != null && _shieldFX.gameObject.activeInHierarchy)
        {
            _shieldFX.gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}
