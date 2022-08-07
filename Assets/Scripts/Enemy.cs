﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 5f)]
    private float _movementSpeed = 4f;

    private float _bottomBarrier = -7f;

    private Player _player;

    /*
    public Vector3[] _positions;
    private int _positionSelector;
    private int _lastSelectedEnemy;
    private int _initialSpawnPositionCount = 1;
    Vector3 posTemp;
    Vector3 _lastPos;
    */

    /*
    private void Awake()
    {
        _positionSelector = Random.Range(0, _positions.Length + 1);

        for (int i = 0; i < _initialSpawnPositionCount; i++)
        {
            if (_positionSelector == _lastSelectedEnemy)
            {
                while (_positionSelector == _lastSelectedEnemy)
                {
                    _positionSelector = Random.Range(0, _positions.Length);
                }
            }

            _lastSelectedEnemy = _positionSelector;

            Vector3 _pos = _positions[_positionSelector]; //Instantiate(_roadPrefabs[_roadSelector], i * _roadOffset, transform.rotation);

            _lastPos = _pos;

            Debug.Log(_pos);

            transform.position = _pos;

            //canSpawnRoad = false;
        }
    }
    */
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        //float startXPos = Random.Range(-8f, 8f);

        //transform.position = new Vector3(startXPos, 10f, 0f);      
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);

            if (_player != null)
            {
                _player.Damage();
            }

        }
        else if (other.tag == "Laser")
        {
            Destroy(gameObject);

            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }
        }
    }
}
