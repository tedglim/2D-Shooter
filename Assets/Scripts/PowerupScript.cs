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

    [SerializeField] //0 - triple, 1 - speed, 2 - shield, 3 - health, 4 - ammo, 5 - missile
    private int powerupID;
    [SerializeField]
    private int negativeScore;
    [SerializeField]
    private GameObject _explosionAnim;
    private bool isSpecialMove;

    void Update()
    {
        if(!isSpecialMove)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        // else 
        // {
        //     Vector3.MoveTowards()
        // }
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
                    case 3:
                        player.AddHealth();
                        break;
                    case 4:
                        player.AddAmmo();
                        break;
                    case 5:
                        player.TurnOnMissileShot();
                        break;
                    case 6:
                        player.NegativePowerup(negativeScore);
                        break;
                }
            }
            Destroy(transform.gameObject);
        }
        if(other.gameObject.tag == "EnemyLaser")
        {
            // AudioSource.PlayClipAtPoint(_explosionClip, transform.position);
            StartCoroutine(DoExplosionAnim(transform.position));
            Destroy(this.gameObject);
            Destroy(other.gameObject);

        }
        //if tag is enemy laser
    }

    private void CleanupPowerup()
    {
        if (transform.position.y < _yMin)
        {
            Destroy(transform.gameObject);
        }
    }

    IEnumerator DoExplosionAnim(Vector3 pos)
    {
        GameObject gObj = Instantiate(_explosionAnim, pos, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        Destroy(gObj);
    }

    public void TurnOffNormalMove()
    {
        isSpecialMove = true;
    }
}
