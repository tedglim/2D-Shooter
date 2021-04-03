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

    // Start is called before the first frame update
    void Start()
    {
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
            Destroy(transform.gameObject);
        }
        else if (other.tag == "Laser")
        {
            Destroy(other.transform.gameObject);
            Destroy(transform.gameObject);
        }
    }
}
