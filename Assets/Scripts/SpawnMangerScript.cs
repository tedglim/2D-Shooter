using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMangerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private float _xMinBound;
    [SerializeField]
    private float _xMaxBound;
    [SerializeField]
    private float _yHeight;
    [SerializeField]
    private int _spawnRateSecs;
    private bool _stopSpawning = false;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator SpawnRoutine()
    {
        while (!_stopSpawning)
        {
            Vector3 spawnPos = new Vector3(Random.Range(_xMinBound, _xMaxBound), _yHeight, 0);
            GameObject newEnemy = Instantiate(_enemy, spawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnRateSecs);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
