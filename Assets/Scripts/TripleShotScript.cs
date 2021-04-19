using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotScript : MonoBehaviour
{
    [SerializeField]
    private float _yUpper;

    void Update()
    {
        if(transform.position.y > _yUpper || transform.childCount == 0)
        {
            Destroy(transform.gameObject);
        }
    }
}
