using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.IO;
using TMPro;

public class GameHandler : MonoBehaviour
{
    public GameObject gameOverPanelPrefab;
    public FieldManager FieldManagerPrefab;
    public FoodManager foodManagerPrefab;
    public TMP_Text scoreText;
    public Snake SnakePrefab;
    public InputAction restartAction;
    public bool IsGameActive = true;
    private int _score = 0;

    public void OnEnable()
    {
        restartAction = new InputAction("Restart", binding: "<Keyboard>/space");
        restartAction.Enable();
    }

    public void OnDisable()
    {
        restartAction.Disable();
    }
    public void HandleCollision(Vector3 headCoord)
    {
        Vector2 positiveHeadCoord = FieldManagerPrefab.RealToPositive(new Vector2(headCoord.x, headCoord.y));
        //Debug.Log("GameHandler.Currently: " + positiveHeadCoord.x + ", " + positiveHeadCoord.y);
        BlockState currentBlockState = FieldManagerPrefab.FieldState[(int)positiveHeadCoord.x, (int)positiveHeadCoord.y];
        //Debug.Log("FoodManager.SpawnFood: " + currentBlockState);
        if (currentBlockState == BlockState.Deadly)
        {
            Debug.Log("GameHandler.GameOver: " + _score + ". Coordinates:" + positiveHeadCoord.x + ", " + positiveHeadCoord.y);
            EndGame();
        }
        if (currentBlockState == BlockState.Eatble)
        {
            Debug.Log("GameHandler.Ate: " + _score + ". Coordinates:" + positiveHeadCoord.x + ", " + positiveHeadCoord.y);
            foodManagerPrefab.DeleteFood(positiveHeadCoord);
            _score++;
            scoreText.text = "Score: " + _score.ToString();
            FieldManagerPrefab.FieldState[(int)positiveHeadCoord.x, (int)positiveHeadCoord.y] = BlockState.Empty;
        }
    }

    public void EndGame()
    {
        Instantiate(gameOverPanelPrefab);
        IsGameActive = false;
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        IsGameActive = true;
        _score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Start() {
        Debug.Log("GameHandler.Start");
        scoreText.text = "Score: " + _score.ToString();
    }

    void Update() {
        if (restartAction.WasPressedThisFrame() && !IsGameActive)
        {
            RestartGame();
        }
    }
}