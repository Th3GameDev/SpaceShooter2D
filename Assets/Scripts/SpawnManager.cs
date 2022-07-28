using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    private bool _stopSpawning = false;


    [Header("Testing")]
    public Vector3[] _positions;
    private int _positionSelector;
    private int _lastSelectedPosition;
    private int _initialSpawnPositionCount = 9;
    Vector3 posTemp;
    Vector3 _lastPos;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine()); 
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Pos selected is" + RandomPos());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        //Vector3 randomPos = new Vector3(Random.Range(-8f, 8f), 7f, 0f);

        while (_stopSpawning == false)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, RandomPos(), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5f);
        }
    }

    public void StopSpawning()
    {
        //Debug.Log("StopSpawning");
        _stopSpawning = true;
    }

    Vector3 RandomPos()
    {
        Vector3 randomPos = Vector3.zero;

        _positionSelector = Random.Range(0, _positions.Length);

        for (int i = 0; i < _initialSpawnPositionCount; i++)
        {
            if (_positionSelector == _lastSelectedPosition)
            {
                while (_positionSelector == _lastSelectedPosition)
                {
                    _positionSelector = Random.Range(0, _positions.Length);
                }
            }

            _lastSelectedPosition = _positionSelector;

            randomPos = _positions[_positionSelector]; //Instantiate(_roadPrefabs[_roadSelector], i * _roadOffset, transform.rotation);

            _lastPos = randomPos;

            //Debug.Log(randomPos);

            //transform.position = randomPos;

            //canSpawnRoad = false;
        }

        return randomPos;
    }
}
