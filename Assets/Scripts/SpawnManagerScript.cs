using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject _tripleShot;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _powerupContainer;
    [SerializeField]
    private float _xMinBound;
    [SerializeField]
    private float _xMaxBound;
    [SerializeField]
    private float _yHeight;
    [SerializeField]
    private int _enemySpawnRateSecs;
    [SerializeField]
    private float _powerupSpawnRateMin;
    [SerializeField]
    private float _powerupSpawnRateMax;
    private bool _stopEnemySpawning = false;
    private bool _stopTripleShotSpawning = false;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnTripleShotRoutine());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (!_stopEnemySpawning)
        {
            Vector3 spawnPos = new Vector3(Random.Range(_xMinBound, _xMaxBound), _yHeight, 0);
            GameObject newEnemy = Instantiate(_enemy, spawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnRateSecs);
        }
    }

    IEnumerator SpawnTripleShotRoutine()
    {
        while(!_stopTripleShotSpawning)
        {
            Vector3 spawnPos = new Vector3(Random.Range(_xMinBound, _xMaxBound), _yHeight, 0);
            GameObject newTripleShot = Instantiate(_tripleShot, spawnPos, Quaternion.identity);
            newTripleShot.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(Random.Range(_powerupSpawnRateMin, _powerupSpawnRateMax));
        }
    }

    public void OnPlayerDeath()
    {
        _stopEnemySpawning = true;
    }
}
