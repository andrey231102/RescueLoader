using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : ObjectPool
{
    [SerializeField] private int _height;
    [SerializeField] private int _width;
    [SerializeField] private int _cellSize;
    [SerializeField] private int _minAmountOfObstacles;
    [SerializeField] private int _maxAmountOfObstacles;
    [SerializeField] private GameObject _obstacle;

    private Grid _grid;
    private Vector3 _position;

    private void Start()
    {
        int amountOfObstacles = Random.Range(_minAmountOfObstacles, _maxAmountOfObstacles);
        _position = transform.position;
        _grid = new Grid(_height, _width, _cellSize, _position);

        Initialize(_obstacle);

        PlaceObstacles(amountOfObstacles);
    }

    private void PlaceObstacles(int amountOfObstacles)
    {
        for (int i = 0; i < amountOfObstacles + 1; i++)
        {
            if (TryGetObject(out GameObject obstacle))
                SetObstacle(obstacle);
        }
    }

    private void SetObstacle(GameObject obstacle)
    {
        obstacle.SetActive(true);
        //Vector3 position = _grid.GridToWorldPosition(Random.Range(0, _width), Random.Range(0, _height));
        obstacle.transform.position = _grid.GridToWorldPosition(Random.Range(0, _width), Random.Range(0, _height));
    }
}

public class Grid
{
    private int _height;
    private int _width;
    private int _cellSize;

    private Vector3 _originPosition;
    private int[,] _grid;

    public int[,] Gridf => _grid;

    public Grid(int height, int width, int cellSize, Vector3 originPosition)
    {
        if (height < 0)
            _height = 1;
        else
            _height = height;

        if (width < 0)
            _width = 1;
        else
            _width = width;

        if (cellSize < 0)
            _cellSize = 1;
        else
            _cellSize = cellSize;

        _originPosition = originPosition;
        _grid = new int[_width, _height];

        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int z = 0; z < _grid.GetLength(1); z++)
            {
                Debug.DrawLine(GridToWorldPosition(x, z), GridToWorldPosition(x, z + 1), Color.white, 100f);
                Debug.DrawLine(GridToWorldPosition(x, z), GridToWorldPosition(x + 1, z), Color.white, 100f);
            }
        }
        Debug.DrawLine(GridToWorldPosition(0, _height), GridToWorldPosition(_width, _height), Color.white, 100f);
        Debug.DrawLine(GridToWorldPosition(_width, 0), GridToWorldPosition(_width, _height), Color.white, 100f);
    }

    public Vector3 GridToWorldPosition(int x, int z)
    {
        return (_originPosition + new Vector3(x, 0, z) * _cellSize);
    }

    private void WorldToGridPosition(Vector3 cellWorldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((cellWorldPosition - _originPosition).x / _cellSize);
        z = Mathf.FloorToInt((cellWorldPosition - _originPosition).z / _cellSize);
    }
}
