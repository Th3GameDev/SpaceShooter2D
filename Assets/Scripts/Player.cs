using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Transform _player;

    [SerializeField]
    private float _speed = 3.5f;
    private float _boundaryX = 9.4f;
    private float _boundaryY = 5.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = transform; 

        _player.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal") * _speed * Time.deltaTime;
        float verticalInput = Input.GetAxis("Vertical") * _speed * Time.deltaTime;

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        _player.Translate(direction);

        if (_player.position.x >= _boundaryX)
        {
            _player.position = new Vector3(-_boundaryX, _player.position.y, 0);
        }
        else if (_player.position.x <= -_boundaryX)
        {
            _player.position = new Vector3(_boundaryX, _player.position.y, 0);
        }

        if (_player.position.y >= _boundaryY)
        {
            _player.position = new Vector3(_player.position.x, -_boundaryY, 0);
        }
        else if (_player.position.y <= -_boundaryY)
        {
            _player.position = new Vector3(_player.position.x, _boundaryY, 0);
        }
    }
}
