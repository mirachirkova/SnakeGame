using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class FoodManager : MonoBehaviour

{
    public GameObject foodPrefab;
    public GameHandler GameHandlerPrefab;
    public Dictionary<Vector2, GameObject> FoodCoord = new Dictionary<Vector2, GameObject>();

    private void Start() {
        SpawnFood();
        Debug.Log("FoodManager.Start");
    }

    private void SpawnFood()
    {
        int x = (int)Random.Range(1, GameHandlerPrefab.FieldManagerPrefab.getWidth() - 2);
        int y = (int)Random.Range(1, GameHandlerPrefab.FieldManagerPrefab.getHeight() - 2);
        GameObject curObject = Instantiate(foodPrefab,
            GameHandlerPrefab.FieldManagerPrefab.PositiveToReal(new Vector2(x, y)), Quaternion.identity);
        Vector2 realValues = GameHandlerPrefab.FieldManagerPrefab.PositiveToReal(new Vector2(x, y));
        Debug.Log("FoodManager.SpawnFood: " + (float)x + ", " + (float)y + " or " + realValues.x + ", " + realValues.y);
        FoodCoord.Add(new Vector2((float)x, (float)y), curObject);
        GameHandlerPrefab.FieldManagerPrefab.FieldState[x, y] = BlockState.Eatble;
    }

    public void DeleteFood(Vector2 coord)
    {
        GameObject toDelete = FoodCoord[coord];
        if (toDelete != null)
        {
            Destroy(toDelete);
        }
        FoodCoord.Remove(coord);
    }

    void Update()
    {
        if (!GameHandlerPrefab.IsGameActive)
        {
            return;
        }
        if (Random.Range(0, 1000) < 2)
        {
            SpawnFood();
        }
    }

}
