using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Snake : MonoBehaviour
{
    public GameObject SnakePreFab;
    public GameHandler GameHandlerPrefab;
    public FieldManager FieldManagerPrefab;
    private List<SnakeBodyBlock> _snakeBody = new List<SnakeBodyBlock>();
    private int _snakeBodyLength = 10;
    private float _timer = 0.0f;
    private float _interval = 0.5f;
    private float _deltaTime = 0.02f;


    void OnEnable()
    {
        Debug.Log("Snake.OnEnable");
        GameObject snakeHeadObj = new GameObject("SnakeHead");
        SnakeHeadBlock snakeHead = snakeHeadObj.AddComponent<SnakeHeadBlock>();
        snakeHead.Initialize(Directions.Up);
        _snakeBody.Add(snakeHead);
        Debug.Log("Snake.addHead");

        for (int i = 1; i < _snakeBodyLength; i++)
        {
            SnakeBodyBlock prevBlock = _snakeBody.Last();
            GameObject snakeBodyObj = new GameObject("SnakeBody" + i);
            SnakeBodyBlock curBlock = snakeBodyObj.AddComponent<SnakeBodyBlock>();
            curBlock.Initialize(prevBlock);
            _snakeBody.Add(curBlock);
            Debug.Log("Snake.addBody" + i);
        }

    }

    void Start()
    {
        ((SnakeHeadBlock)_snakeBody[0]).Restart(Directions.Up);
        _timer = 0.0f;
        Debug.Log("Snake.Start");
    }

    void OnDisable()
    {
        foreach (SnakeBodyBlock block in _snakeBody)
        {
            block.OnDisable();
        }
    }



    void Update()
    {
        if (!GameHandlerPrefab.IsGameActive)
        {
            return;
        }

        GameHandlerPrefab.HandleCollision(_snakeBody[0].transform.position);

        ((SnakeHeadBlock)_snakeBody[0]).updateMovement();
        _timer += _deltaTime;
        if (_timer > _interval)
        {
            FieldManagerPrefab.SetEmpty(_snakeBody[_snakeBodyLength - 1].transform.position.x,
             _snakeBody[_snakeBodyLength - 1].transform.position.y);
            FieldManagerPrefab.SetDeadly(_snakeBody[0].transform.position.x, _snakeBody[0].transform.position.y);
            for (int i = _snakeBodyLength - 1; i >= 0; i--)
            {
                _snakeBody[i].updatePosition();
            }

            _timer -= _interval;
        }

    }

}