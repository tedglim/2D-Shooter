using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float _initSpeed;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _boostedSpeed;

    private bool _speedBoostOn;

    [SerializeField]
    private float _totalThrusterTime;

    [SerializeField]
    private float _thrusterCD;

    private float _canThrusters;

    private float _currentThrusterTime;

    [SerializeField]
    private int _totalAmmo;

    private int _currAmmo;

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
    private GameObject _missilePrefab;

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
    private Vector3 _tripleShotOffset;

    [SerializeField]
    private float _boostDuration;

    private bool _isShieldActive;

    [SerializeField]
    private Transform _shieldVFX;

    private int _shieldLives = 3;

    private bool _canMissileShot;

    [SerializeField]
    private float _missileShotDuration;

    [SerializeField]
    private float _missileShotRate;

    private float _nextMissile = -1;
    [SerializeField]
    private float _magnetRadius = 5;
    [SerializeField]
    private float _powerupSpeed = 3;

    private int _score = 0;

    private UIManagerScript _uiManagerScript;

    [SerializeField]
    private GameObject

            _leftEnginePrefab,
            _rightEnginePrefab;

    [SerializeField]
    private AudioClip _laserClip;

    [SerializeField]
    private AudioClip _explosionClip;

    [SerializeField]
    private AudioClip _outOfAmmoClip;

    [SerializeField]
    private AudioClip _missileClip;

    private AudioSource _audioSource;

    private CameraShakeScript _cameraShake;

    private GameManagerScript _gameManager;

    void Start()
    {
        _spawnManagerScript =
            GameObject.Find("Spawn_Manager").GetComponent<SpawnManagerScript>();
        _uiManagerScript =
            GameObject.Find("UI_Manager").GetComponent<UIManagerScript>();
        _audioSource = transform.GetComponent<AudioSource>();
        _cameraShake =
            GameObject.Find("Shake").GetComponent<CameraShakeScript>();
        _gameManager =
            GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        if (_spawnManagerScript == null)
        {
            Debug.LogError("Spawn Manager is null");
        }
        if (_uiManagerScript == null)
        {
            Debug.LogError("UIManager is null");
        }
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource is null");
        }
        else
        {
            _audioSource.clip = _laserClip;
        }
        if (_cameraShake == null)
        {
            Debug.LogError("CameraShake is null");
        }
        if (_gameManager == null)
        {
            Debug.LogError("game manager is null");
        }
        _currentThrusterTime = _thrusterCD;
        _canThrusters = -1;
        _currAmmo = _totalAmmo;
        _uiManagerScript.UpdateAmmoCount (_currAmmo, _totalAmmo);
    }

    void Update()
    {
        MovePlayer();
        if (
            Input.GetKey(KeyCode.Space) &&
            _canMissileShot &&
            Time.time > _nextMissile
        )
        {
            FireMissile();
        }
        else if (
            Input.GetKey(KeyCode.Space) &&
            !_canMissileShot &&
            Time.time > _canFire
        )
        {
            FireLaser();
        }
        if (
            Input.GetKey(KeyCode.LeftShift) &&
            !_speedBoostOn &&
            Time.time > _canThrusters
        )
        {
            BurnThrusters();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _initSpeed;
            _canThrusters = Time.time + _thrusterCD;
        }
        else if (Time.time > _canThrusters)
        {
            RefillThrusters();
        }
        if (Input.GetKey(KeyCode.C))
        {
            CollectPowerups();
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
                new Vector3(_lowBoundX,
                    transform.position.y,
                    transform.position.z);
        }
        else if (transform.position.x < _lowBoundX)
        {
            transform.position =
                new Vector3(_upBoundX,
                    transform.position.y,
                    transform.position.z);
        }
    }

    private void FireLaser()
    {
        if (_currAmmo <= 0)
        {
            _audioSource.clip = _outOfAmmoClip;
            _audioSource.Play();
            return;
        }
        else if (_gameManager.hasGameStarted)
        {
            _currAmmo--;
            _uiManagerScript.UpdateAmmoCount (_currAmmo, _totalAmmo);
        }
        _canFire = Time.time + _fireRate;
        Vector3 laserStartPos =
            new Vector3(transform.position.x,
                transform.position.y + _laserOffsetY,
                transform.position.z);
        if (_canTripleShot)
        {
            Instantiate(_tripleShotPrefab,
            laserStartPos + _tripleShotOffset,
            Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, laserStartPos, Quaternion.identity);
        }
        _audioSource.clip = _laserClip;
        _audioSource.Play();
    }

    private void FireMissile()
    {
        _nextMissile = Time.time + _missileShotRate;
        Instantiate(_missilePrefab,
        transform.position,
        Quaternion.Euler(0, 0, 90));
        _audioSource.clip = _missileClip;
        _audioSource.Play();
    }

    private void BurnThrusters()
    {
        if (_speed != _boostedSpeed)
        {
            _initSpeed = _speed;
        }
        _speed = _boostedSpeed;
        _currentThrusterTime -= Time.deltaTime;
        if (_currentThrusterTime <= 0)
        {
            _currentThrusterTime = 0;
            _speed = _initSpeed;
        }
        _uiManagerScript.UpdateThrusters (_currentThrusterTime, _thrusterCD);
    }

    private void RefillThrusters()
    {
        if (_currentThrusterTime >= _thrusterCD)
        {
            _currentThrusterTime = _thrusterCD;
        }
        else
        {
            _currentThrusterTime += Time.deltaTime;
        }
        _uiManagerScript.UpdateThrusters (_currentThrusterTime, _thrusterCD);
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _shieldLives--;
            _uiManagerScript.UpdateShield (_shieldLives);
            if (_shieldLives == 0)
            {
                _isShieldActive = false;
                _shieldVFX.gameObject.SetActive(false);
            }
            return;
        }

        _lives--;
        _cameraShake.Shake();
        if (_lives >= 0)
        {
            _uiManagerScript.UpdateLivesImg (_lives);
            _audioSource.clip = _explosionClip;
            _audioSource.Play();
        }

        if (_lives == 2)
        {
            _rightEnginePrefab.SetActive(true);
        }
        if (_lives == 1)
        {
            _leftEnginePrefab.SetActive(true);
        }
        if (_lives < 1)
        {
            Destroy(transform.gameObject);
            _spawnManagerScript.OnPlayerDeath();
            _uiManagerScript.DisplayGameOver();
        }
    }

    public void TurnOnTripleShot()
    {
        _canTripleShot = true;
        StartCoroutine(DecayTripleShotTime());
    }

    IEnumerator DecayTripleShotTime()
    {
        yield return new WaitForSeconds(_boostDuration);
        _canTripleShot = false;
    }

    public void TurnOnMissileShot()
    {
        _canMissileShot = true;
        StartCoroutine(DecayMissileShotTime());
    }

    IEnumerator DecayMissileShotTime()
    {
        yield return new WaitForSeconds(_missileShotDuration);
        _canMissileShot = false;
    }

    public void RaiseSpeed()
    {
        _speedBoostOn = true;
        _initSpeed = _speed;
        _speed = _boostedSpeed;
        StartCoroutine(DecaySpeedBoostTime());
    }

    IEnumerator DecaySpeedBoostTime()
    {
        yield return new WaitForSeconds(_boostDuration);
        _speed = _initSpeed;
        _speedBoostOn = false;
    }

    public void TurnOnShields()
    {
        _isShieldActive = true;
        _shieldVFX.gameObject.SetActive(true);
        _shieldLives = 3;
        _uiManagerScript.UpdateShield (_shieldLives);
    }

    public void AddToScore(int points)
    {
        _score += points;
        _uiManagerScript.UpdateScoreText (_score);
        _spawnManagerScript.UpdateWaveProgress();
    }

    public void AddHealth()
    {
        if (_lives < 3)
        {
            _lives++;
            if (_lives == 3)
            {
                _rightEnginePrefab.SetActive(false);
            }
            if (_lives == 2)
            {
                _leftEnginePrefab.SetActive(false);
            }
            _uiManagerScript.UpdateLivesImg (_lives);
        }
    }

    public void AddAmmo()
    {
        _currAmmo = _totalAmmo;
        _uiManagerScript.UpdateAmmoCount (_currAmmo, _totalAmmo);
    }

    //reduce score
    public void NegativePowerup(int negativePoints)
    {
        if (_score >= negativePoints)
        {
            _score -= negativePoints;
        }
        else
        {
            _score = 0;
        }
        _uiManagerScript.UpdateScoreText (_score);
    }

    private void CollectPowerups()
    {
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(transform.position, _magnetRadius);
        foreach (Collider2D colliderr in colliders)
        {
            if (colliderr.gameObject.tag == "Powerup")
            {
                Debug.Log(colliderr.gameObject.name);
                StartCoroutine(AttractPowerup(colliderr.gameObject));
            }
        }
    }

    IEnumerator AttractPowerup(GameObject pwrUp)
    {
        PowerupScript pwrScript = pwrUp.GetComponent<PowerupScript>();
        pwrScript.TurnOffNormalMove();
        while (true)
        {
            if (pwrUp != null)
            {
                pwrUp.transform.position =
                    Vector3
                        .MoveTowards(pwrUp.gameObject.transform.position,
                        transform.position,
                        _powerupSpeed * Time.deltaTime);
                yield return new WaitForSeconds(0);
            }
            else
            {
                break;
            }
        }
    }
}
