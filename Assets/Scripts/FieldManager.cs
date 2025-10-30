using UnityEngine;
using System;

public enum BlockState
{
    Empty,
    Eatble,
    Deadly,
    Head
}

public class FieldManager : MonoBehaviour
{
    public const int BlockSize = 1;
    public const int FieldWidth = 100 / BlockSize;
    public const int FieldHeight = 50 / BlockSize;
    public static readonly Vector3 FieldCentrum = new Vector3(0f, 0f, 0f);
    public BlockState[,] FieldState = new BlockState[FieldWidth, FieldHeight];

    public Vector2 RealToPositive(Vector2 coordinates)
    {
        return new Vector2((int)Math.Floor(coordinates.x) / BlockSize + FieldWidth / 2, (int)Math.Floor(coordinates.y) / BlockSize + FieldHeight / 2);
    }
    public Vector3 PositiveToReal(Vector2 coordinates)
    {
        return new Vector3((int)coordinates.x * BlockSize - FieldWidth / 2 + 0.5f
            , (int)coordinates.y * BlockSize - FieldHeight / 2 + 0.5f, 0);
    }
    public int getWidth()
    {
        return FieldWidth;
    }

    public int getHeight()
    {
        return FieldHeight;
    }

    public void SetEmpty(float x, float y)
    {
        Vector2 posCoord = RealToPositive(new Vector2(x, y));
        FieldState[(int)posCoord.x, (int)posCoord.y] = BlockState.Empty;
    }

    public void SetDeadly(float x, float y)
    {
        Vector2 posCoord = RealToPositive(new Vector2(x, y));
        FieldState[(int)posCoord.x, (int)posCoord.y] = BlockState.Deadly;
        Debug.Log("FieldManager.SetDeadly: " + posCoord.x + posCoord.y);
    }


    void OnEnable()
    {
        for (int x = 0; x < FieldWidth; ++x)
        {
            for (int y = 0; y < FieldHeight; ++y)
            {
                FieldState[x, y] = BlockState.Empty;
                if (y == FieldHeight - 1)
                {
                    FieldState[x, y] = BlockState.Deadly;
                }
                if (y == 0)
                {
                    FieldState[x, y] = BlockState.Deadly;
                }
                if (x == FieldWidth - 1)
                {
                    FieldState[x, y] = BlockState.Deadly;
                }
                if (x == 0)
                {
                    FieldState[x, y] = BlockState.Deadly;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
