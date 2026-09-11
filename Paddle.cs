using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace breakout;

public class Paddle
{
    public Sprite sprite;
    public const float Diameter = 20.0f;
    public const float Radius = Diameter * .5f;
    public Vector2f size;
    public Vector2f direction = new Vector2f(1, 1) / MathF.Sqrt(2.0f);

    public Paddle()
    {
        sprite = new Sprite();
        sprite.Texture = new Texture("assets/paddle.png");
        sprite.Position = new Vector2f(Program.ScreenW / 2, Program.ScreenH - 20); // byt detta till spritens höjd
        Vector2f paddleTextureSize = (Vector2f)sprite.Texture.Size;
        sprite.Origin = 0.5f * paddleTextureSize;
        sprite.Scale = new Vector2f(
            Diameter / paddleTextureSize.Y,
            Diameter / paddleTextureSize.Y);
        size = new Vector2f(
            sprite.GetGlobalBounds().Width,
            sprite.GetGlobalBounds().Height);
    }

    public void Draw(RenderTarget target)
    {
        target.Draw(sprite);
    }

    public void Update(Ball ball, float dt)
    {
        var newPos = sprite.Position;
        if (Keyboard.IsKeyPressed(Keyboard.Key.Right) || Keyboard.IsKeyPressed(Keyboard.Key.D))
        {
            newPos.X += dt * 300.0f;
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.Left) || Keyboard.IsKeyPressed(Keyboard.Key.A))
        {
            newPos.X -= dt * 300.0f;

        }
        if (newPos.X > Program.ScreenW - size.X / 2) //TODO: Byt ut mot 45 mot ett värde från paddelns storlek
        {
            newPos.X = Program.ScreenW - size.X / 2;
        }
        if (newPos.X < 0 + size.X / 2) //TODO: Byt ut mot 45 mot ett värde från paddelns storlek
        {
            newPos.X = 0 + size.X / 2;
        }
        sprite.Position = newPos;

        if (Collision.CircleRectangle(
                ball.sprite.Position, Ball.Radius, this.sprite.Position, size, out Vector2f hit))
        {
            ball.sprite.Position += hit;
            ball.Reflect(hit.Normalized());
        }
    }
}