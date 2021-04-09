using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _yMin;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        CleanupPowerup();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
            if (player != null)
            {
                player.TurnOnTripleShot();
            }
            Destroy(transform.gameObject);
        }
    }

    private void CleanupPowerup()
    {
        if (transform.position.y < _yMin)
        {
            Destroy(transform.gameObject);
        }
    }
}
