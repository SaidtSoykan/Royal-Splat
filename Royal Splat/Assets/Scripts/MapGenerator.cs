using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Transform boundryPrefab;
    public Vector2 mapSize;
    public Coord startPoint;

    public int moveCount;
    List<Coord> moveableCoords;
    List<Coord> allTileCoords;
    List<Coord> keyTiles;
    void Start()
    {
        GenerateMap();
    }
    public void GenerateMap()
    {
        startPoint = new Coord(0, 0);
        allTileCoords = new List<Coord>();
        moveableCoords = new List<Coord>();
        keyTiles = new List<Coord>();
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                allTileCoords.Add(new Coord(x, y));
            }
        }
        string holderName = "Generated Map";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }
        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.name = "(" + x + "," + y + ")";
                newTile.parent = mapHolder;
                //obstacles for out of bounds
                if (x == 0)
                {
                    Vector3 boundryPosition = new Vector3(-mapSize.x / 2 + 0.5f + x - 1, 0, -mapSize.y / 2 + 0.5f + y);
                    Transform newBoundry = Instantiate(boundryPrefab, boundryPosition + Vector3.up * .5f, Quaternion.identity) as Transform;
                    newBoundry.parent = mapHolder;
                    Coord boundryCoords = new Coord(-1, y);
                    keyTiles.Add(boundryCoords);
                }
                if (y == 0)
                {
                    Vector3 boundryPosition = new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y - 1);
                    Transform newBoundry = Instantiate(boundryPrefab, boundryPosition + Vector3.up * .5f, Quaternion.identity) as Transform;
                    newBoundry.parent = mapHolder;
                    Coord boundryCoords = new Coord(x, -1);
                    keyTiles.Add(boundryCoords);
                }
                if (x == mapSize.x - 1)
                {
                    Vector3 boundryPosition = new Vector3(-mapSize.x / 2 + 0.5f + x + 1, 0, -mapSize.y / 2 + 0.5f + y);
                    Transform newBoundry = Instantiate(boundryPrefab, boundryPosition + Vector3.up * .5f, Quaternion.identity) as Transform;
                    newBoundry.parent = mapHolder;
                    Coord boundryCoords = new Coord((int)mapSize.x, y);
                    keyTiles.Add(boundryCoords);
                }
                if (y == mapSize.y - 1)
                {
                    Vector3 boundryPosition = new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y + 1);
                    Transform newBoundry = Instantiate(boundryPrefab, boundryPosition + Vector3.up * .5f, Quaternion.identity) as Transform;
                    newBoundry.parent = mapHolder;
                    Coord boundryCoords = new Coord(x, (int)mapSize.y);
                    keyTiles.Add(boundryCoords);
                }
            }
        }
        Coord startCoord = new Coord((int)Random.Range(0,mapSize.x),(int)Random.Range(0,mapSize.y));
        Debug.Log("Start point: (" + startCoord.x + "," + startCoord.y + ")");
        Coord upCoord = new Coord(startCoord.x, startCoord.y + 1);
        Coord rightCoord = new Coord(startCoord.x + 1, startCoord.y);
        Coord downCoord = new Coord(startCoord.x, startCoord.y - 1);
        Coord leftCoord = new Coord(startCoord.x - 1, startCoord.y);
        moveableCoords.Add(startCoord);
        for(int i = 0; i < moveCount; i++)
        {
            Debug.Log("Move Count: " + (i+1));
            List<int> ways = new List<int>();
            if (IsPassed(upCoord))
            {
                ways.Add(1);
            }
            if (IsPassed(rightCoord))
            {
                ways.Add(2);
            }
            if (IsPassed(downCoord))
            {
                ways.Add(3);
            }
            if (IsPassed(leftCoord))
            {
                ways.Add(4);
            }
            int way = ways[Random.Range(0, ways.Count)];
            if (way == 1)
            {
                Debug.Log("Up Chosen");
                int moveDistance = Random.Range(1, (int)mapSize.y - startCoord.y - 1);
                Coord ptrCoord = new Coord(startCoord.x, startCoord.y + moveDistance);
                Coord ptrUp = new Coord(ptrCoord.x, ptrCoord.y + 1);
                if (moveableCoords.Contains(ptrCoord))
                {
                    if(ptrUp.y != mapSize.y && !moveableCoords.Contains(ptrUp))
                    {
                        moveDistance++;
                    }
                    else
                    {
                        moveDistance = 0;
                    }
                }
                else if (moveableCoords.Contains(ptrUp))
                {
                    moveDistance--;
                }
                if(moveDistance != 0)
                {
                    keyTiles.Add(downCoord);
                    for (int j = 0; j < moveDistance; j++)
                    {
                        moveableCoords.Add(upCoord);
                        Debug.Log("Added Coord: (" + upCoord.x + "," + upCoord.y + ")");
                        startCoord = upCoord;
                        upCoord = new Coord(upCoord.x, upCoord.y + 1);
                    }
                    keyTiles.Add(upCoord);
                }
            }
            if (way == 2)
            {
                Debug.Log("Right Chosen");
                int moveDistance = Random.Range(1, (int)mapSize.x - startCoord.x - 1);
                Coord ptrCoord = new Coord(startCoord.x + moveDistance, startCoord.y);
                Coord ptrRight = new Coord(ptrCoord.x + 1, ptrCoord.y);
                if (moveableCoords.Contains(ptrCoord))
                {
                    if (ptrRight.x != mapSize.x && !moveableCoords.Contains(ptrRight))
                    {
                        moveDistance++;
                    }
                    else
                    {
                        moveDistance = 0;
                    }
                }
                else if (moveableCoords.Contains(ptrRight))
                {
                    moveDistance--;
                }
                if (moveDistance != 0)
                {
                    keyTiles.Add(leftCoord);
                    for (int j = 0; j < moveDistance; j++)
                    {
                        moveableCoords.Add(rightCoord);
                        Debug.Log("Added Coord: (" + rightCoord.x + "," + rightCoord.y + ")");
                        startCoord = rightCoord;
                        rightCoord = new Coord(rightCoord.x + 1, rightCoord.y);
                    }
                    keyTiles.Add(rightCoord);
                }
            }
            if (way == 3)
            {
                Debug.Log("Down Chosen");
                int moveDistance = Random.Range(1, startCoord.y + 1);
                Coord ptrCoord = new Coord(startCoord.x, startCoord.y - moveDistance);
                Coord ptrDown = new Coord(ptrCoord.x, ptrCoord.y - 1);
                if (moveableCoords.Contains(ptrCoord))
                {
                    if (ptrDown.y != 0 && !moveableCoords.Contains(ptrDown))
                    {
                        moveDistance++;
                    }
                    else
                    {
                        moveDistance = 0;
                    }
                }
                else if (moveableCoords.Contains(ptrDown))
                {
                    moveDistance--;
                }
                if (moveDistance != 0)
                {
                    keyTiles.Add(upCoord);
                    for (int j = 0; j < moveDistance; j++)
                    {
                        moveableCoords.Add(downCoord);
                        Debug.Log("Added Coord: (" + downCoord.x + "," + downCoord.y + ")");
                        startCoord = downCoord;
                        downCoord = new Coord(downCoord.x, upCoord.y - 1);
                    }
                    keyTiles.Add(downCoord);
                }
            }
            if(way == 4)
            {
                Debug.Log("Left Chosen");
                int moveDistance = Random.Range(1, startCoord.x + 1);
                Coord ptrCoord = new Coord(startCoord.x - moveDistance, startCoord.y);
                Coord ptrLeft = new Coord(ptrCoord.x - 1, ptrCoord.y);
                if (moveableCoords.Contains(ptrCoord))
                {
                    if (ptrLeft.x != 0 && !moveableCoords.Contains(ptrLeft))
                    {
                        moveDistance++;
                    }
                    else
                    {
                        moveDistance = 0;
                    }
                }
                else if (moveableCoords.Contains(ptrLeft))
                {
                    moveDistance--;
                }
                if (moveDistance != 0)
                {
                    keyTiles.Add(rightCoord);
                    for (int j = 0; j < moveDistance; j++)
                    {
                        moveableCoords.Add(leftCoord);
                        Debug.Log("Added Coord: (" + leftCoord.x + "," + leftCoord.y + ")");
                        startCoord = leftCoord;
                        leftCoord = new Coord(leftCoord.x, leftCoord.y + 1);
                    }
                    keyTiles.Add(leftCoord);
                }
            }
            if(way == 0)
            {
                break;
            }
            ways.Clear();
            upCoord = new Coord(startCoord.x, startCoord.y + 1);
            rightCoord = new Coord(startCoord.x + 1, startCoord.y);
            downCoord = new Coord(startCoord.x, startCoord.y - 1);
            leftCoord = new Coord(startCoord.x - 1, startCoord.y);
        }

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Coord comp = new Coord(x, y);
                if (!moveableCoords.Contains(comp))
                {
                    Coord _add = new Coord(comp.x, comp.y);
                    Vector3 obstaclePosition = new Vector3(-mapSize.x / 2 + 0.5f + _add.x, 0, -mapSize.y / 2 + 0.5f + _add.y);
                    Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * .5f, Quaternion.identity) as Transform;
                    newObstacle.parent = mapHolder;
                }
                
            }
        }
    }
    public bool IsPassed(Coord passedTile)
    {
        if (moveableCoords.Contains(passedTile) && keyTiles.Contains(passedTile))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
    }

    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }
}
