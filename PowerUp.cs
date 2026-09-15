using SFML.Graphics;
using SFML.System;

namespace breakout;

public class PowerUp
{
    public Sprite sprite;
    public const float Diameter = 20.0f;
    public const float Radius = Diameter * .5f;
    public Vector2f direction = new Vector2f(0,1) / MathF.Sqrt(4.0f);
    public Vector2f spawnPosition = new Vector2f(400, 600);
    public Vector2f newPos = new Vector2f(400, 600);
    
    public PowerUp()
    {
        sprite = new Sprite();
        sprite.Texture = new Texture("assets/ball.png");
        sprite.Color = Color.Red;
        sprite.Position = spawnPosition;
        Vector2f ballTextureSize = (Vector2f)sprite.Texture.Size;
        sprite.Origin = 0.5f * ballTextureSize;
        sprite.Scale = new Vector2f(
            Diameter / ballTextureSize.X,
            Diameter / ballTextureSize.Y);

    }

    public void Draw(RenderTarget target)
    {
        target.Draw(sprite);
    }

    public void Update(float dt)
    {
        newPos += direction * dt * 100.0f;
        sprite.Position = newPos;

        if (newPos.Y > Program.ScreenH - Radius)
        {
            newPos = new Vector2f(-100f, -100f);
        }
    }
}