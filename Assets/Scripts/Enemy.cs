using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 5f)]
    private float _movementSpeed = 4f;

    private float _bottomBarrier = -7f;

    // Start is called before the first frame update
    void Start()
    {
        float startXPos = Random.Range(-8f, 8f);

        transform.position = new Vector3(startXPos, 10f, 0f);
    }

    // Update is called once per frame
    void Update()
    {       
        transform.Translate(Vector3.down * _movementSpeed * Time.deltaTime);
        
        if (transform.position.y <= _bottomBarrier)
        {
            float newXPos = Random.Range(-8f, 8f);
            transform.position = new Vector3(newXPos, 7f, 0f);       
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);

            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
           
        }
        else if (other.tag == "Laser")
        {           
            Destroy(gameObject);

            Destroy(other.gameObject);
        }
    }
}
