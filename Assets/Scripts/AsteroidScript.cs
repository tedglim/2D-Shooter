using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed;

    [SerializeField]
    private GameObject _explosion;

    private SpawnManagerScript _spawnManager;

    void Start()
    {
        _spawnManager =
            GameObject.Find("Spawn_Manager").GetComponent<SpawnManagerScript>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }
    }

    void Update()
    {
        transform.Rotate(0f, 0f, _rotationSpeed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            GameObject explosion =
                Instantiate(_explosion,
                transform.position,
                Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(GetComponent<Collider2D>());
            _spawnManager.StartSpawning();
            Destroy(transform.gameObject, .5f);
        }
    }
}
