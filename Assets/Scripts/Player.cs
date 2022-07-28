using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpawnManager _spawnManager;

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

    [Header("TESTING")]
    [Range(0f, 5f)]
    public float blinkRate = 0.3f;

    [Range(0, 10)]
    public int numberofBlinks = 3;

    // Start is called before the first frame update
    void Start()
    {
        if (_spawnManager != null)
        {
            _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        }
        else if (_spawnManager == null)
        {
            Debug.LogWarning("Enemy_SpawnManager is Null!");
        }

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

        if (_player.position.y >= _boundaryY)
        {
            _player.position = new Vector3(_player.position.x, -_boundaryY, 0);
        }
        else if (_player.position.y <= -_boundaryY)
        {
            _player.position = new Vector3(_player.position.x, _boundaryY, 0);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(BlinkGameObject(gameObject, numberofBlinks, blinkRate));
        }
    }

    void Shoot()
    {
        Instantiate(_laserPrefab, _laserOffset.position, Quaternion.identity);

        _canFire = false;

        StartCoroutine(LaserCoolDownTimer());
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
        currentLives--;

        StartCoroutine(BlinkGameObject(gameObject, numberofBlinks, blinkRate));

        if (currentLives < 1)
        {
            if (_spawnManager != null)
            {
                _spawnManager.StopSpawning();
            }

            Destroy(gameObject);
        }
    }   
}
