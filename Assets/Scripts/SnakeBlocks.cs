using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public static class Img2Sprite
{

    public static Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight)
    {
        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
        Texture2D SpriteTexture = LoadTexture(FilePath);
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);

        return NewSprite;
    }

    public static Texture2D LoadTexture(string FilePath)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }
}

public class Directions
{
    public static readonly Vector2 Up = new Vector2(0f, 1f);
    public static readonly Vector2 Down = new Vector2(0f, -1f);
    public static readonly Vector2 Right = new Vector2(1f, 0f);
    public static readonly Vector2 Left = new Vector2(-1f, 0f);
}

public class SnakeBodyBlock : MonoBehaviour
{
    protected SnakeBodyBlock _prevBodyBlock = null;
    protected int Size = 1;
    protected int PPU = 256;

    public virtual void OnEnable()
    { }

    public virtual void OnDisable()
    { }

    public void Initialize(SnakeBodyBlock prevBodyBlock)
    {
        _prevBodyBlock = prevBodyBlock;
        this.transform.position = prevBodyBlock.transform.position + new Vector3(0f, -1f, 0f) * Size;

        GameObject targetGameObject = this.gameObject;
        SpriteRenderer spriteRenderer = targetGameObject.AddComponent<SpriteRenderer>();
        Sprite bodySprite = Img2Sprite.LoadNewSprite("Assets/Textures/SnakeBody.png", PPU);
        if (bodySprite != null)
        {
            spriteRenderer.sprite = bodySprite;
            spriteRenderer.material.color = new Color(0f, 0.58f, 0.063f);
        }
        else
        {
            Debug.LogError("Sprite not found! Ensure it's in a Resources folder and the name is correct.");
        }
    }

    public virtual void updatePosition()
    {
        this.transform.position = _prevBodyBlock.transform.position;
    }
}

public class SnakeHeadBlock: SnakeBodyBlock
{
    public InputAction moveAction;
    private Vector2 _currentDirection = new Vector2(0f, 0f);

    public override void OnEnable()
    {
        moveAction = new InputAction("Move", binding: "<Gamepad>/rightStick");
        moveAction.AddCompositeBinding("Dpad")
        .With("Up", "<Keyboard>/w")
        .With("Down", "<Keyboard>/s")
        .With("Left", "<Keyboard>/a")
        .With("Right", "<Keyboard>/d");

        moveAction.Enable();
    }

    public override void OnDisable()
    {
        moveAction.Disable();
    }

    public void Restart(Vector2 currentMovementDirection)
    {
        _currentDirection = currentMovementDirection;
    }

    public void Initialize(Vector2 currentMovementDirection)
    {
        _prevBodyBlock = null;
        _currentDirection = currentMovementDirection;
        this.transform.position = new Vector3(0.0f, 0.0f, 0f);

        GameObject targetGameObject = this.gameObject;
        SpriteRenderer spriteRenderer = targetGameObject.AddComponent<SpriteRenderer>();
        Sprite bodySprite = Img2Sprite.LoadNewSprite("Assets/Textures/SnakeHead.png", PPU);
        if (bodySprite != null)
        {
            spriteRenderer.sprite = bodySprite;
            spriteRenderer.material.color = new Color(0f, 0.839f, 0.09f);
        }
        else
        {
            Debug.LogError("Sprite not found! Ensure it's in a Resources folder and the name is correct.");
        }
    }

    public void updateMovement()
    {
        if (moveAction.WasPressedThisFrame())
        {
            Vector2 newDirection = moveAction.ReadValue<Vector2>();
            if (newDirection == Directions.Up && _currentDirection != Directions.Down)
            {
                _currentDirection = Directions.Up;
            }
            if (newDirection == Directions.Down && _currentDirection != Directions.Up)
            {
                _currentDirection = Directions.Down;
            }
            if (newDirection == Directions.Left && _currentDirection != Directions.Right)
            {
                _currentDirection = Directions.Left;
            }
            if (newDirection == Directions.Right && _currentDirection != Directions.Left)
            {
                _currentDirection = Directions.Right;
            }
        }
    }

    public override void updatePosition()
    {
        this.transform.position += new Vector3(_currentDirection.x, _currentDirection.y, 0f) * Size;
    }

}
