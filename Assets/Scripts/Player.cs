using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 3.5f;
    private float _boundaryX = 9.4f;
    private float _boundaryY = 5.5f;
    private Transform _player;

    [SerializeField]
    private float _fireRate = 0.5f;

    [SerializeField]
    private GameObject laserPrefab;

    [SerializeField]
    private Transform _laserOffset;

    private bool _canFire;
    private IEnumerator laserCooldownTimer;


    // Start is called before the first frame update
    void Start()
    {
        _player = transform; 
        _player.position = new Vector3(0, 0, 0);

        _canFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.Space) && _canFire)
        {
            Shoot();
        }
    }

    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal") * _movementSpeed * Time.deltaTime;
        float verticalInput = Input.GetAxis("Vertical") * _movementSpeed * Time.deltaTime;

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

    void Shoot()
    {  
        Instantiate(laserPrefab, _laserOffset.position, Quaternion.identity);
        _canFire = false;
        StartCoroutine(LaserCoolDownTimer());
    }

    IEnumerator LaserCoolDownTimer()
    {
        yield return new WaitForSeconds(_fireRate);
        _canFire = true;
    }
}
