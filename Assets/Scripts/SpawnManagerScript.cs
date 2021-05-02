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

    [SerializeField]
    private float _moddedMissileRate;

    private bool _stopEnemySpawning = false;

    private bool _stopPowerupSpawn = false;

    private GameManagerScript _gameManager;

    void Start()
    {
        _gameManager =
            GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }

    public void StartSpawning()
    {
        _gameManager.GameStart();
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
            rand = ReRollOnce(rand, _powerups[rand]);
            GameObject newPowerup =
                Instantiate(_powerups[rand], spawnPos, Quaternion.identity);
            newPowerup.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(Random
                        .Range(_powerupSpawnRateMin, _powerupSpawnRateMax));
        }
    }

    //Higher modded missile rate more likely to reroll
    private int ReRollOnce(int orig, GameObject powerup)
    {
        if (powerup.name == "Missile_Powerup")
        {
            int rand = Random.Range(0, 100);
            if (rand <= _moddedMissileRate)
            {
                return Random.Range(0, _powerups.Length);
            }
        }
        return orig;
    }

    public void OnPlayerDeath()
    {
        _stopEnemySpawning = true;
        _stopPowerupSpawn = true;
    }
}
