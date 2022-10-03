using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject _map;

    void Update()
    {
        if (GameObject.Find("Map"))
        {
            SetMapObject();
            Vector3 cameraPos = new Vector3(0, (int)(_map.GetComponent<MapGenerator>().mapSize.y) + 20, -7);
            transform.position = cameraPos;
        }
    }
    void SetMapObject()
    {
        _map = GameObject.Find("Map").gameObject;
    }
}
