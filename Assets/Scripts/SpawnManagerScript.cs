using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject _enemy;

    [SerializeField]
    private GameObject _powerupContainer;

    [SerializeField]
    private GameObject[] _powerups;

    [SerializeField]
    private float _xMinBound;

    [SerializeField]
    private float _xMaxBound;

    [SerializeField]
    private float _yHeight;

    [SerializeField]
    private int _enemySpawnRateSecs;

    [SerializeField]
    private float _powerupSpawnRateMin = 6f;

    [SerializeField]
    private float _powerupSpawnRateMax = 8f;

    private bool _stopEnemySpawning = false;

    private bool _stopPowerupSpawn = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (!_stopEnemySpawning)
        {
            Vector3 spawnPos =
                new Vector3(Random.Range(_xMinBound, _xMaxBound), _yHeight, 0);
            GameObject newEnemy =
                Instantiate(_enemy, spawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnRateSecs);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (!_stopPowerupSpawn)
        {
            Vector3 spawnPos =
                new Vector3(Random.Range(_xMinBound, _xMaxBound), _yHeight, 0);
            int rand = Random.Range(0, _powerups.Length);
            GameObject newPowerup =
                Instantiate(_powerups[rand], spawnPos, Quaternion.identity);
            newPowerup.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(Random
                        .Range(_powerupSpawnRateMin, _powerupSpawnRateMax));
        }
    }

    public void OnPlayerDeath()
    {
        _stopEnemySpawning = true;
        _stopPowerupSpawn = true;
    }
}
