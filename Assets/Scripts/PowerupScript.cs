using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _yMin;

    [SerializeField]
    private AudioClip _powerupClip;

    [SerializeField] //0 - triple, 1 - speed, 2 - shield
    private int powerupID;

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
                AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
                switch (powerupID)
                {
                    default:
                        Debug.Log("Default");
                        break;
                    case 0:
                        player.TurnOnTripleShot();
                        break;
                    case 1:
                        player.RaiseSpeed();
                        break;
                    case 2:
                        player.TurnOnShields();
                        break;
                }
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
