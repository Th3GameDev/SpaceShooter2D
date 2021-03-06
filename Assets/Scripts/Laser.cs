using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _LaserSpeed = 8f;
    private float _boundaryX = 9.4f;
    private float _boundaryY = 5.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _LaserSpeed * Time.deltaTime);
        
        if (transform.position.x >= _boundaryX)
        {
            Destroy(gameObject);
        }
        else if (transform.position.y >= _boundaryY)
        {
            Destroy(gameObject);   
        }      
    }
}
