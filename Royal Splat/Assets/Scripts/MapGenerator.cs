using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Transform boundryPrefab;
    public Vector2 mapSize;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;

    public int seed = 10;
    void Start()
    {
        GenerateMap();
    }
    public void GenerateMap()
    {
        allTileCoords = new List<Coord>();
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                allTileCoords.Add(new Coord(x, y));
            }
        }
        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), seed));
        string holderName = "Generated Map";
        if(transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }
        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;
        for(int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.name = "(" + x + "," + y + ")";
                newTile.parent = mapHolder;
                if(x == 0)
                {
                    Vector3 boundryPosition = new Vector3(-mapSize.x / 2 + 0.5f + x - 1, 0, -mapSize.y / 2 + 0.5f + y);
                    Transform newBoundry = Instantiate(boundryPrefab, boundryPosition + Vector3.up * .5f, Quaternion.identity) as Transform;
                    newBoundry.parent = mapHolder;
                }
                if(y == 0)
                {
                    Vector3 boundryPosition = new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y - 1);
                    Transform newBoundry = Instantiate(boundryPrefab, boundryPosition + Vector3.up * .5f, Quaternion.identity) as Transform;
                    newBoundry.parent = mapHolder;
                }
                if(x == mapSize.x - 1)
                {
                    Vector3 boundryPosition = new Vector3(-mapSize.x / 2 + 0.5f + x + 1, 0, -mapSize.y / 2 + 0.5f + y);
                    Transform newBoundry = Instantiate(boundryPrefab, boundryPosition + Vector3.up * .5f, Quaternion.identity) as Transform;
                    newBoundry.parent = mapHolder;
                }
                if(y == mapSize.y - 1)
                {
                    Vector3 boundryPosition = new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y + 1);
                    Transform newBoundry = Instantiate(boundryPrefab, boundryPosition + Vector3.up * .5f, Quaternion.identity) as Transform;
                    newBoundry.parent = mapHolder;
                }
            }
        }
        int obstacleCount = 10;
        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);
            Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * .5f, Quaternion.identity) as Transform;
            newObstacle.parent = mapHolder;
        }
    }
    Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
    }

    public Coord GetRandomCoord()
    {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
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
