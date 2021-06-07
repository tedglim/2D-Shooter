using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManagerScript : MonoBehaviour
{
    [System.Serializable]
    public class WaveInfo
    {
        [SerializeField]
        private UnityEvent waveEvent;

        [SerializeField]
        private int totalEnemies;

        private int destroyedEnemies;

        public int TotalEnemies
        {
            get
            {
                return totalEnemies;
            }
            set
            {
                totalEnemies = value;
            }
        }

        public int DestroyedEnemies
        {
            get
            {
                return destroyedEnemies;
            }
            set
            {
                destroyedEnemies = value;
            }
        }

        public UnityEvent WaveEvent
        {
            get
            {
                return waveEvent;
            }
            set
            {
                waveEvent = value;
            }
        }
    }

    [System.Serializable]
    public class PowerupSpawnInfo
    {
        [SerializeField]
        public GameObject powerup;

        [SerializeField]
        public int dropRate;
    }

    [Header("Enemies")]
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _enemyTypes;

    [SerializeField]
    private int _enemySpawnRate;

    [Header("Powerups")]
    [SerializeField]
    private GameObject _powerupContainer;

    // [SerializeField]
    // private GameObject[] _powerups;
    [SerializeField]
    private PowerupSpawnInfo[] _powerupData;

    [SerializeField]
    private Vector2 _powerupSpawnRateMinMax;

    private bool _stopEnemySpawning = false;

    private bool _stopPowerupSpawn = false;

    private GameManagerScript _gameManager;

    [Header("WaveInfo")]
    [SerializeField]
    private WaveInfo[] _waveData;

    private int _currWaveIdx;

    [SerializeField]
    private Vector2 _minMaxX;

    [SerializeField]
    private float _yHeight;

    private WaveInfo _currWave;

    private UIManagerScript _uiManagerScript;

    void Start()
    {
        _currWaveIdx = 0;
        _gameManager =
            GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        _uiManagerScript =
            GameObject.Find("UI_Manager").GetComponent<UIManagerScript>();
    }

    public void StartSpawning()
    {
        _gameManager.GameStart();
        NextWave();
    }

    public void NextWave()
    {
        _waveData[_currWaveIdx].WaveEvent.Invoke();
    }

    public void SpawnWave01()
    {
        SetupWaveTracker();
        StartCoroutine(GenerateWave01());
        StartCoroutine(SpawnPowerupRoutine());
    }

    public void SpawnWave02()
    {
        SetupWaveTracker();
        StartCoroutine(GenerateWave02());
        //StartCoroutine(SpawnPowerupRoutine());
    }

    private void SetupWaveTracker()
    {
        if (_currWaveIdx < _waveData.Length)
        {
            _currWave = new WaveInfo();
            _currWave.TotalEnemies = _waveData[_currWaveIdx].TotalEnemies;
            _currWave.DestroyedEnemies = 0;
        }
    }

    //1st Wave
    IEnumerator GenerateWave01()
    {
        //Wave start text
        _uiManagerScript.UpdateWaveStatus(false, true, 1);
        yield return new WaitForSeconds(2.0f);
        _uiManagerScript.UpdateWaveStatus(true, true, 1);

        while (!_stopEnemySpawning)
        {
            Vector3 spawnPos =
                new Vector3(Random.Range(_minMaxX[0], _minMaxX[1]),
                    _yHeight,
                    0);
            GameObject newEnemy =
                Instantiate(_enemyTypes[0], spawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnRate);
        }
    }

    IEnumerator GenerateWave02()
    {
        float space = 2;
        _uiManagerScript.UpdateWaveStatus(false, true, 2);
        yield return new WaitForSeconds(2.0f);
        _uiManagerScript.UpdateWaveStatus(true, true, 2);
        for (int i = 0; i < 5; i++)
        {
            SpawnEnemyAtPos(_minMaxX[0], _yHeight - space, _enemyTypes[1]);
            SpawnEnemyAtPos(_minMaxX[1], _yHeight - 2 * space, _enemyTypes[2]);
            SpawnEnemyAtPos(_minMaxX[0], _yHeight - 3 * space, _enemyTypes[1]);
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void SpawnEnemyAtPos(float x, float y, GameObject gObj)
    {
        Vector3 spawnPos = new Vector3(x, y, 0);
        GameObject enemy = Instantiate(gObj, spawnPos, Quaternion.identity);
        enemy.transform.parent = _enemyContainer.transform;
    }

    public void UpdateWaveProgress()
    {
        _currWave.DestroyedEnemies++;
        if (_currWave.DestroyedEnemies >= _currWave.TotalEnemies)
        {
            StopAllCoroutines();
            StartCoroutine(CleanupEnemies());
            //StartCoroutine(CleanupPowerups());
        }
    }

    IEnumerator CleanupEnemies()
    {
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < _enemyContainer.transform.childCount; i++)
        {
            Destroy(_enemyContainer.transform.GetChild(i).gameObject);
        }

        if(!CheckLastWave())
        {
            //Wave complete text
            _uiManagerScript.UpdateWaveStatus(false, false, _currWaveIdx);
            yield return new WaitForSeconds(2.0f);
            _uiManagerScript.UpdateWaveStatus(true, false, _currWaveIdx);
        
            NextWave();
        }
    }

    IEnumerator CleanupPowerups()
    {
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < _powerupContainer.transform.childCount; i++)
        {
            Destroy(_powerupContainer.transform.GetChild(i).gameObject);
        }
    }

    private bool CheckLastWave()
    {
        _currWaveIdx++;
        if (_currWaveIdx >= _waveData.Length)
        {
            //All waves completed
            _uiManagerScript.UpdateWaveEndStatus();
            Debug.Log("Stop. No more Waves");
            return true;
        }
        return false;
    }

    // IEnumerator SpawnEnemyRoutine()
    // {
    //     yield return new WaitForSeconds(2.0f);
    //     while (!_stopEnemySpawning)
    //     {
    //         Vector3 spawnPos =
    //             new Vector3(Random.Range(_minMaxX[0], _minMaxX[1]),
    //                 _yHeight,
    //                 0);
    //         GameObject newEnemy =
    //             Instantiate(_enemyTypes[0], spawnPos, Quaternion.identity);
    //         newEnemy.transform.parent = _enemyContainer.transform;
    //         yield return new WaitForSeconds(_enemySpawnRate);
    //     }
    // }
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (!_stopPowerupSpawn)
        {
            Vector3 spawnPos =
                new Vector3(Random.Range(_minMaxX[0], _minMaxX[1]),
                    _yHeight,
                    0);

            // int rand = Random.Range(0, _powerups.Length);
            // rand = ReRollOnce(rand, _powerups[rand]);
            // GameObject newPowerup =
            //     Instantiate(_powerups[rand], spawnPos, Quaternion.identity);
            int rand = ChooseRandomPowerup();
            if(rand == -1) 
            {
                Debug.LogError("Powerup Spawner broke");
                break;
            }
            GameObject newPowerup =
                Instantiate(_powerupData[rand].powerup,
                spawnPos,
                Quaternion.identity);
            newPowerup.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(Random
                        .Range(_powerupSpawnRateMinMax[0],
                        _powerupSpawnRateMinMax[1]));
        }
    }

    private int ChooseRandomPowerup()
    {
        int total = 0;
        int runningTotal = 0;
        int randVal = 0;
        for (int i = 0; i < _powerupData.Length; i++)
        {
            total += _powerupData[i].dropRate;
        }
        randVal = Random.Range(0, total);
        for (int i = 0; i < _powerupData.Length; i++)
        {
            runningTotal += _powerupData[i].dropRate;
            if (randVal < runningTotal)
            {
                return i;
            }
        }
        return -1;
    }

    public void OnPlayerDeath()
    {
        _stopEnemySpawning = true;
        _stopPowerupSpawn = true;
    }
}
