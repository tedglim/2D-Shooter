using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeScript : MonoBehaviour
{
    private Transform _camera;
    private Vector3 _origPos;
    private CinemachineImpulseSource _source;
    [SerializeField]
    private float _shakeTime = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        _source = transform.gameObject.GetComponent<CinemachineImpulseSource>();
        _camera = GameObject.Find("Main Camera").transform; 
        _origPos = _camera.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shake(int lives)
    {
        _source.GenerateImpulse();
        StartCoroutine(Recenter(lives));
    }

    IEnumerator Recenter(int lives)
    {
        yield return new WaitForSeconds(_shakeTime);        
        _camera.transform.position = _origPos;
    }
}
