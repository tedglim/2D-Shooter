using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _yMin;

    [SerializeField] //0 - triple, 1 - speed, 2 - shield
    private int powerupID;

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
                switch(powerupID)
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
