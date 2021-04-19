using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    [SerializeField]
    private AudioClip _explosionClip;

    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = transform.GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.Log("audio Source is null");
        }
        else
        {
            _audioSource.clip = _explosionClip;
            _audioSource.Play();
        }
        Destroy(transform.gameObject, 1.5f);
    }
}
