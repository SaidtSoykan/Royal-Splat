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
        if (CheckCompletion())
        {
            Debug.Log("Tebrikler");
            DestroyImmediate(GameObject.Find("Map").gameObject);
            canvas.SetActive(true);
        }
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
    public bool CheckCompletion()
    {
        int x = 0;
        if (GameObject.Find("Tiles"))
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
}