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

    public GameObject levelMenu;
    public GameObject congratMenu;
    public GameObject firstTouch;
    public GameObject settingsMenu;

    private int _levelNumber = 0;
    private bool isWon;
    void Awake()
    {
        firstTouch.SetActive(true);
    }
    void Update()
    {
        if(firstTouch.activeInHierarchy && Input.GetMouseButton(0))
        {
            FirstTouch();
        }
        if (CheckCompletion())
        {
            congratMenu.SetActive(true);
            isWon = true;
        }
    }
    public void SetLevel()
    {
        isWon = false;
        string levelName = EventSystem.current.currentSelectedGameObject.name;
        int.TryParse(levelName, out _levelNumber);
        levelMenu.SetActive(false);
        GameObject mapObject = new GameObject("Map");
        mapObject.AddComponent<MapGenerator>();
        mapObject.GetComponent<MapGenerator>().GenerateMap(tilePrefab, obstaclePrefab, boundryPrefab, playerPrefab, _levelNumber);
    }
    public bool CheckCompletion()
    {
        int x = 0;
        if (GameObject.Find("Tiles") && !isWon)
        {
            GameObject _tiles = GameObject.Find("Tiles");
            for(int i = 0; i < _tiles.transform.childCount; i++)
            {
                if(_tiles.transform.GetChild(i).GetComponent<GroundTile>().isColored == true)
                {
                    x++;
                }
            }
            if(x == _tiles.transform.childCount)
            {
                return true;
            }
        }
        return false;
    }
    public void NextLevelButton()
    {
        _levelNumber += 1;
        DestroyImmediate(GameObject.Find("Map").gameObject);
        GameObject mapObject = new GameObject("Map");
        mapObject.AddComponent<MapGenerator>();
        mapObject.GetComponent<MapGenerator>().GenerateMap(tilePrefab, obstaclePrefab, boundryPrefab, playerPrefab, _levelNumber);
        congratMenu.SetActive(false);
        isWon = false;
    }
    public void BacktoLevelMenu()
    {
        congratMenu.SetActive(false);
        DestroyImmediate(GameObject.Find("Map").gameObject);
        levelMenu.SetActive(true);
    }
    public void FirstTouch()
    {
        firstTouch.SetActive(false);
        levelMenu.SetActive(true);
    }
}