using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _maxLives = 3;

    [SerializeField]
    private int currentLives;

    [SerializeField]
    [Range(0f, 5f)]
    private float _movementSpeed = 3.5f;

    private float _boundaryX = 9.4f;
    private float _boundaryY = 5.5f;
    private Transform _player;

    [SerializeField]
    [Range(0f, 1f)]
    private float _fireRate = 0.2f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private Transform _laserOffset;
    private bool _canFire;
    

    // Start is called before the first frame update
    void Start()
    {
        currentLives = _maxLives;

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
        Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.7f, 0), Quaternion.identity);

        _canFire = false;

        StartCoroutine(LaserCoolDownTimer());
    }

    IEnumerator LaserCoolDownTimer()
    {
        yield return new WaitForSeconds(_fireRate);
        _canFire = true;
    }

    public void Damage()
    {
        currentLives--;

        if (currentLives < 1)
        {
            Destroy(gameObject);
        }
    }
}
