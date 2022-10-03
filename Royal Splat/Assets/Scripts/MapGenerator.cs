using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    private int moveCount;
    public Vector2 mapSize;
    public int _seed = 0;

    List<Coord> moveableCoords = new List<Coord>();
    List<Coord> allTileCoords;
    List<Coord> keyTiles;
    [Range(0, 1)]
    private float percentageOfEmptyTiles;
    public void GenerateMap(Transform tilePrefab, Transform obstaclePrefab, Transform boundryPrefab, Transform playerPrefab, int levelNumber)
    {
        _seed = levelNumber;
        System.Random seed = new System.Random(_seed);
        percentageOfEmptyTiles = levelNumber * 0.001f + 0.1f;
        mapSize.x = seed.Next(10, (int)(levelNumber / 10) + 10);
        mapSize.y = seed.Next(10, (int)(levelNumber / 10) + 10);
        while ((moveableCoords.Count / (mapSize.x * mapSize.y)) < percentageOfEmptyTiles)
        {
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

            string mapHolderName = "Generated Map";
            string tileHolderName = "Tiles";
            string boundryHolderName = "Boundries";
            string obstacleHolderName = "Obstacles";

            if (transform.Find(mapHolderName))
            {
                DestroyImmediate(transform.Find(mapHolderName).gameObject);
            }
            Transform mapHolder = new GameObject(mapHolderName).transform;
            Transform tileHolder = new GameObject(tileHolderName).transform;
            Transform boundryHolder = new GameObject(boundryHolderName).transform;
            Transform obstacleHolder = new GameObject(obstacleHolderName).transform;
            mapHolder.parent = transform;
            tileHolder.parent = mapHolder;
            boundryHolder.parent = mapHolder;
            obstacleHolder.parent = mapHolder;
            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    //obstacles for out of bounds
                    if (x == 0)
                    {
                        Vector3 boundryPosition = new Vector3(-mapSize.x / 2 + 0.5f + x - 1, 0, -mapSize.y / 2 + 0.5f + y);
                        Transform newBoundry = Instantiate(boundryPrefab, boundryPosition + Vector3.up * .5f, Quaternion.identity) as Transform;
                        newBoundry.parent = boundryHolder;
                        newBoundry.name = "Boundry (" + (x - 1) + "," + y + ")";
                        Coord boundryCoords = new Coord(-1, y);
                        keyTiles.Add(boundryCoords);
                    }
                    if (y == 0)
                    {
                        Vector3 boundryPosition = new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y - 1);
                        Transform newBoundry = Instantiate(boundryPrefab, boundryPosition + Vector3.up * .5f, Quaternion.identity) as Transform;
                        newBoundry.parent = boundryHolder;
                        newBoundry.name = "Boundry (" + x + "," + (y - 1) + ")";
                        Coord boundryCoords = new Coord(x, -1);
                        keyTiles.Add(boundryCoords);
                    }
                    if (x == mapSize.x - 1)
                    {
                        Vector3 boundryPosition = new Vector3(-mapSize.x / 2 + 0.5f + x + 1, 0, -mapSize.y / 2 + 0.5f + y);
                        Transform newBoundry = Instantiate(boundryPrefab, boundryPosition + Vector3.up * .5f, Quaternion.identity) as Transform;
                        newBoundry.parent = boundryHolder;
                        newBoundry.name = "Boundry (" + (x + 1) + "," + y + ")";
                        Coord boundryCoords = new Coord((int)mapSize.x, y);
                        keyTiles.Add(boundryCoords);
                    }
                    if (y == mapSize.y - 1)
                    {
                        Vector3 boundryPosition = new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y + 1);
                        Transform newBoundry = Instantiate(boundryPrefab, boundryPosition + Vector3.up * .5f, Quaternion.identity) as Transform;
                        newBoundry.parent = boundryHolder;
                        newBoundry.name = "Boundry (" + x + "," + (y + 1) + ")";
                        Coord boundryCoords = new Coord(x, (int)mapSize.y);
                        keyTiles.Add(boundryCoords);
                    }
                }
            }
            Coord startCoord = new Coord(seed.Next(0, (int)mapSize.x), seed.Next(0, (int)mapSize.y));
            Vector3 playerPosition = new Vector3(-mapSize.x / 2 + 0.5f + startCoord.x, 0, -mapSize.y / 2 + 0.5f + startCoord.y);
            Transform newPlayer = Instantiate(playerPrefab, playerPosition + Vector3.up * .5f, Quaternion.identity) as Transform;
            newPlayer.parent = mapHolder;
            newPlayer.name = "Player Ball";
            Coord upCoord = new Coord(startCoord.x, startCoord.y + 1);
            Coord rightCoord = new Coord(startCoord.x + 1, startCoord.y);
            Coord downCoord = new Coord(startCoord.x, startCoord.y - 1);
            Coord leftCoord = new Coord(startCoord.x - 1, startCoord.y);
            moveableCoords.Add(startCoord);
            moveCount = seed.Next(10, 10 + levelNumber * 2);
            for (int i = 0; i < moveCount; i++)
            {
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
                if (ways.Count == 0)
                {
                    break;
                }
                int way = ways[seed.Next(0, ways.Count)];
                if (way == 1)
                {
                    if (i == 0)
                    {
                        keyTiles.Add(downCoord);
                        keyTiles.Add(leftCoord);
                        keyTiles.Add(rightCoord);
                    }
                    int k = 0;
                    Coord checkCoord = new Coord(startCoord.x, startCoord.y + 1);
                    while (!keyTiles.Contains(checkCoord))
                    {
                        checkCoord = new Coord(checkCoord.x, checkCoord.y + 1);
                        k++;
                    }
                    int moveDistance = seed.Next(1, k);
                    Coord ptrCoord = new Coord(startCoord.x, startCoord.y + moveDistance);
                    Coord ptrUp = new Coord(ptrCoord.x, ptrCoord.y + 1);
                    if (moveableCoords.Contains(ptrCoord))
                    {
                        if (ptrUp.y != mapSize.y && !moveableCoords.Contains(ptrUp))
                        {
                            moveDistance++;
                        }
                        else
                        {
                            moveDistance = 0;
                        }
                    }
                    else if (moveableCoords.Contains(ptrUp) || keyTiles.Contains(ptrCoord))
                    {
                        moveDistance--;
                    }
                    if (moveDistance != 0)
                    {
                        if (!keyTiles.Contains(downCoord))
                        {
                            keyTiles.Add(downCoord);
                        }
                        for (int j = 0; j < moveDistance; j++)
                        {
                            moveableCoords.Add(upCoord);
                            startCoord = upCoord;
                            upCoord = new Coord(upCoord.x, upCoord.y + 1);
                        }
                        if (!keyTiles.Contains(upCoord))
                        {
                            keyTiles.Add(upCoord);
                        }
                    }
                }
                if (way == 2)
                {
                    if (i == 0)
                    {
                        keyTiles.Add(downCoord);
                        keyTiles.Add(leftCoord);
                        keyTiles.Add(upCoord);
                    }
                    int k = 0;
                    Coord checkCoord = new Coord(startCoord.x + 1, startCoord.y);
                    while (!keyTiles.Contains(checkCoord))
                    {
                        checkCoord = new Coord(checkCoord.x + 1, checkCoord.y);
                        k++;
                    }
                    int moveDistance = seed.Next(1, k);
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
                    else if (moveableCoords.Contains(ptrRight) || keyTiles.Contains(ptrCoord))
                    {
                        moveDistance--;
                    }
                    if (moveDistance != 0)
                    {
                        if (!keyTiles.Contains(leftCoord))
                        {
                            keyTiles.Add(leftCoord);
                        }
                        for (int j = 0; j < moveDistance; j++)
                        {
                            moveableCoords.Add(rightCoord);
                            startCoord = rightCoord;
                            rightCoord = new Coord(rightCoord.x + 1, rightCoord.y);
                        }
                        if (!keyTiles.Contains(rightCoord))
                        {
                            keyTiles.Add(rightCoord);
                        }
                    }
                }
                if (way == 3)
                {
                    if (i == 0)
                    {
                        keyTiles.Add(upCoord);
                        keyTiles.Add(leftCoord);
                        keyTiles.Add(rightCoord);
                    }
                    int k = 0;
                    Coord checkCoord = new Coord(startCoord.x, startCoord.y - 1);
                    while (!keyTiles.Contains(checkCoord))
                    {
                        checkCoord = new Coord(checkCoord.x, checkCoord.y - 1);
                        k++;
                    }
                    int moveDistance = seed.Next(1, k);
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
                    else if (moveableCoords.Contains(ptrDown) || keyTiles.Contains(ptrCoord))
                    {
                        moveDistance--;
                    }
                    if (moveDistance != 0)
                    {
                        if (!keyTiles.Contains(upCoord))
                        {
                            keyTiles.Add(upCoord);
                        }
                        for (int j = 0; j < moveDistance; j++)
                        {
                            moveableCoords.Add(downCoord);
                            startCoord = downCoord;
                            downCoord = new Coord(downCoord.x, downCoord.y - 1);
                        }
                        if (!keyTiles.Contains(downCoord))
                        {
                            keyTiles.Add(downCoord);
                        }
                    }
                }
                if (way == 4)
                {
                    if (i == 0)
                    {
                        keyTiles.Add(downCoord);
                        keyTiles.Add(upCoord);
                        keyTiles.Add(rightCoord);
                    }
                    int k = 0;
                    Coord checkCoord = new Coord(startCoord.x - 1, startCoord.y);
                    while (!keyTiles.Contains(checkCoord))
                    {
                        checkCoord = new Coord(checkCoord.x - 1, checkCoord.y);
                        k++;
                    }
                    int moveDistance = seed.Next(1, k);
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
                    else if (moveableCoords.Contains(ptrLeft) || keyTiles.Contains(ptrCoord))
                    {
                        moveDistance--;
                    }
                    if (moveDistance != 0)
                    {
                        if (!keyTiles.Contains(rightCoord))
                        {
                            keyTiles.Add(rightCoord);
                        }
                        for (int j = 0; j < moveDistance; j++)
                        {
                            moveableCoords.Add(leftCoord);
                            startCoord = leftCoord;
                            leftCoord = new Coord(leftCoord.x - 1, leftCoord.y);
                        }
                        if (!keyTiles.Contains(leftCoord))
                        {
                            keyTiles.Add(leftCoord);
                        }
                    }
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
                        newObstacle.parent = obstacleHolder;
                        newObstacle.name = "Obstacle (" + x + "," + y + ")";
                    }
                    else
                    {
                        Vector3 tilePosition = new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
                        Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                        newTile.name = "Tile (" + x + "," + y + ")";
                        newTile.parent = tileHolder;
                    }
                }
            }
            _seed += 1;
        }
    }
    public bool IsPassed(Coord passedTile)
    {
        if (moveableCoords.Contains(passedTile) || keyTiles.Contains(passedTile))
        {
            return false;
        }
        else
        {
            return true;
        }
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