using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupDetectorScript : MonoBehaviour
{
    private Transform _enemy;
    [SerializeField]
    private GameObject _laser;
    [SerializeField]
    private Vector3 _laserOffset;
    // Start is called before the first frame update
    void Awake() {
        _enemy = this.transform.parent.transform;
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Powerup")
        {
            Instantiate(_laser, _enemy.position + _laserOffset, Quaternion.Euler(0,0,180));
        }  
        if(other.gameObject.tag == "Player")
        {
            return;
        } 
        if(other.gameObject.tag == "Enemy")
        {
            return;
        }
    }
}
