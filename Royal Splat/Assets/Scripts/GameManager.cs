using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Transform boundryPrefab;
    public Transform playerPrefab;
    public GameObject canvas;
    private int _levelNumber = 0;
    void Start()
    {

    }
    void Update()
    {
        
    }
    public void SetLevel()
    {
        string levelName = EventSystem.current.currentSelectedGameObject.name;
        int.TryParse(levelName, out _levelNumber);
        canvas.SetActive(false);
        GameObject mapObject = new GameObject("Map");
        mapObject.AddComponent<MapGenerator>();
        mapObject.GetComponent<MapGenerator>().GenerateMap(tilePrefab, obstaclePrefab, boundryPrefab, playerPrefab, _levelNumber);
    }
}