using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Transform boundryPrefab;
    public Transform playerPrefab;
    private int _levelNumber;
    private static int seed = 0;
    private int addedTime;
    MapGenerator generateMap = new MapGenerator();

    [Range(0, 1)]
    public float percentageOfEmptyTiles;
    void Start()
    {
        addedTime = 1;
        _levelNumber = 1;
        for(int i = 0; i < 5; i++)
        {
            System.Random _seed = new System.Random(seed);
            generateMap.GenerateMap(_seed, tilePrefab, obstaclePrefab, boundryPrefab, playerPrefab, _levelNumber);
        }
    }
    void Update()
    {
        if(_levelNumber == 1 + 5 * addedTime)
        {
            System.Random _seed = new System.Random(seed);
            generateMap.GenerateMap(_seed, tilePrefab, obstaclePrefab, boundryPrefab, playerPrefab, _levelNumber);
        }
    }
}
