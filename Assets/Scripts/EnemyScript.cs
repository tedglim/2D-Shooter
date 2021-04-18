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

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _animator = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < _minYScreenHeight)
        {
            float randXPos = Random.Range(_minXScreenWidth, _maxXScreenWidth);
            transform.position =
                new Vector3(randXPos, _maxYScreenHeight, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerScript player = other.transform.GetComponent<PlayerScript>();
            if (player != null)
            {
                player.Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            Destroy(transform.gameObject, 1f);
        }
        else if (other.tag == "Laser")
        {
            Destroy(other.transform.gameObject);
            if (_player != null)
            {
                _player.AddToScore (_pointValue);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            Destroy(transform.gameObject, 1f);
        }
    }
}
