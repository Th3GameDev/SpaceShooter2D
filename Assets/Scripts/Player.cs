using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpawnManager _spawnManager;

    [Header("Player Settings")]

    private Transform _player;

    [SerializeField]
    private int _maxLives = 3;

    [SerializeField]
    private int currentLives;

    [SerializeField]
    [Range(0f, 10f)]
    private float _movementSpeed = 5f;

    private float _boundaryX = 9.4f;
    private float _boundaryY = 3.9f;

    [Header("Shooting Settings")]
    [SerializeField]
    [Range(0f, 1f)]
    private float _fireRate = 0.2f;

    [SerializeField]
    private GameObject _laserPrefab, tripleShotLaserPrefab;

    [SerializeField]
    private Transform _laserOffset;
    private bool _canFire;

    [Header("PowerUps")]

    [SerializeField]
    private GameObject _playerShieldVisualizer;

    [SerializeField]
    private bool _isTripleShotActive, _isShieldActive;

    [SerializeField]
    [Range(0f, 5f)]
    private float _tripleShotTimeActive = 5f, _SpeedBoostTimeActive = 5f;

    [SerializeField]
    [Range(0f, 5f)]
    private float _speedMultiplier = 3.5f;

    [Header("TESTING")]
    [Range(0f, 5f)]
    public float blinkRate = 0.3f;

    [Range(0, 10)]
    public int numberofBlinks = 3;

    // Start is called before the first frame update
    void Start()
    {
        currentLives = _maxLives;

        _player = transform;

        _player.position = Vector3.zero;

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

        _player.position = new Vector3(_player.position.x, Mathf.Clamp(_player.position.y, -_boundaryY, _boundaryY), 0);

        /*
        if (_player.position.y >= _boundaryY)
        {
            //_player.position = new Vector3(_player.position.x, -_boundaryY, 0);
            _player.position = new Vector3(_player.position.x, _boundaryY, 0);
        }
        else if (_player.position.y <= -_boundaryY)
        {
            //_player.position = new Vector3(_player.position.x, _boundaryY, 0);
            _player.position = new Vector3(_player.position.x, -_boundaryY, 0);
        }
        */

        if (Input.GetKeyDown(KeyCode.T))
        {
            //StartCoroutine(BlinkGameObject(gameObject, numberofBlinks, blinkRate));
            ActivateSpeedBoost();
            _isTripleShotActive = true;
        }

    }

    void Shoot()
    {
        if (_isTripleShotActive)
        {
            _fireRate = 0.5f;
            Instantiate(tripleShotLaserPrefab, transform.position, Quaternion.identity);
            _canFire = false;
            StartCoroutine(LaserCoolDownTimer());
        }
        else
        {
            _fireRate = 0.3f;
            Instantiate(_laserPrefab, _laserOffset.position, Quaternion.identity);
            _canFire = false;
            StartCoroutine(LaserCoolDownTimer());
        }
    }

    IEnumerator LaserCoolDownTimer()
    {
        yield return new WaitForSeconds(_fireRate);
        _canFire = true;
    }

    public IEnumerator BlinkGameObject(GameObject gameObject, int numBlinks, float seconds)
    {
        // In this method it is assumed that your game object has a SpriteRenderer component attached to it
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();

        //disable animation if any animation is attached to the game object
        //Animator animator = gameObject.GetComponent<Animator>();
        //animator.enabled = false; // stop animation for a while
        for (int i = 0; i < numBlinks * 2; i++)
        {
            //toggle renderer
            renderer.enabled = !renderer.enabled;
            //wait for a bit
            yield return new WaitForSeconds(seconds);
        }

        //make sure renderer is enabled when we exit
        renderer.enabled = true;
        //animator.enabled = true; // enable animation again, if it was disabled before
    }
    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _playerShieldVisualizer.SetActive(false);
            return;
        }

        currentLives--;

        StartCoroutine(BlinkGameObject(gameObject, numberofBlinks, blinkRate));

        if (currentLives < 1)
        {
            _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

            if (_spawnManager != null)
            {
                _spawnManager.StopSpawning();
            }

            Destroy(gameObject);
        }
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerUpTimer());
    }

    public void ActivateSpeedBoost()
    {
        _movementSpeed += _speedMultiplier;
        StartCoroutine(SpeedBoostTimer());
    }

    public void ActivateShield()
    {
        _isShieldActive = true;
        _playerShieldVisualizer.SetActive(true);
    }

    IEnumerator TripleShotPowerUpTimer()
    {
        yield return new WaitForSeconds(_tripleShotTimeActive);
        _isTripleShotActive = false;
    }

    IEnumerator SpeedBoostTimer()
    {
        yield return new WaitForSeconds(_SpeedBoostTimeActive);
        _movementSpeed = 5f;
    }
}
