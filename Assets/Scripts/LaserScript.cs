using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _upBoundY;

    [SerializeField]
    private float _lowBoundY;

    private AudioSource _audioSource;

    void Start()
    {
        if (transform.tag == "EnemyLaser")
        {
            _audioSource = transform.GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                Debug.LogError("AudioSource is null");
            }
        }
    }

    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y > _upBoundY)
        {
            if (transform.parent != null)
            {
                Transform parent = transform.parent;
                Destroy(parent.gameObject);
            }
            Destroy(transform.gameObject);
        }
        if (transform.position.y < _lowBoundY)
        {
            Destroy(transform.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (
            other.gameObject.tag == "Player" &&
            this.gameObject.tag == "EnemyLaser"
        )
        {
            PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
            if (player != null)
            {
                _audioSource.Play();
                player.Damage();
            }
            Destroy(this.gameObject);
        }
    }
}
