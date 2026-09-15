using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace breakout;

public class Paddle
{
    public Sprite sprite;
    public const float Diameter = 20.0f;
    public Vector2f size;
    float timer;                    // Används för att hålla koll på paddelns power up status.
    private Vector2f paddleTextureSize;


    public Paddle()
    {
        sprite = new Sprite();
        sprite.Texture = new Texture("assets/paddle.png");
        sprite.Position = new Vector2f(Program.ScreenW / 2, Program.ScreenH - 20); // 20 kunde vara spritens höjd/2 men fungerar ändå.
        paddleTextureSize = (Vector2f)sprite.Texture.Size;
        sprite.Origin = 0.5f * paddleTextureSize;
        sprite.Scale = new Vector2f(
            Diameter / paddleTextureSize.Y,
            Diameter / paddleTextureSize.Y);
        size = new Vector2f(                        // Sätter hitboxen till spritens storlek, size = hitbox.
            sprite.GetGlobalBounds().Width,
            sprite.GetGlobalBounds().Height);
    }

    public void Draw(RenderTarget target)
    {
        target.Draw(sprite);
    }

    public void Update(Ball ball, float dt, PowerUp powerUp)
    {
        timer += dt;
        var newPos = sprite.Position;
        if (Keyboard.IsKeyPressed(Keyboard.Key.Right) || Keyboard.IsKeyPressed(Keyboard.Key.D))
        {
            newPos.X += dt * 300.0f;
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.Left) || Keyboard.IsKeyPressed(Keyboard.Key.A))
        {
            newPos.X -= dt * 300.0f;

        }
        if (newPos.X > Program.ScreenW - size.X / 2)  // Stoppar paddeln att åka utanför skärmens högersida
        {
            newPos.X = Program.ScreenW - size.X / 2;
        }
        if (newPos.X < 0 + size.X / 2)  // Stoppar paddeln att åka utanför skärmens vänstersida
        {
            newPos.X = 0 + size.X / 2;
        }
        sprite.Position = newPos; // Uppdaterar paddelns position med den senaste beräknade.

        if (Collision.CircleRectangle(              // Om kollision sker mellan boll och paddle, reflektera boll.
                ball.sprite.Position, Ball.Radius, this.sprite.Position, size, out Vector2f hit))
        {
            ball.sprite.Position += hit;
            ball.Reflect(hit.Normalized());
            ball.BonusScore = 0;
        }
        if(Collision.CircleRectangle( /// Om kollision sker mellan powerUp och paddle.
               powerUp.sprite.Position, PowerUp.Radius, this.sprite.Position, size, out Vector2f hit2))
        {
            timer = 0; // Sätter timer till 0 för att hålla koll på starttid av powerUp
            powerUp.sprite.Position += hit2;
            if (timer <= 4) // Under 4 sekunder blir paddle dubbelt så stor.
            {
                sprite.Scale = new Vector2f(
                    Diameter / paddleTextureSize.Y * 2,
                    Diameter / paddleTextureSize.Y * 2);
                size = new Vector2f(
                    sprite.GetGlobalBounds().Width,
                    sprite.GetGlobalBounds().Height);
               
            }
            powerUp.newPos = new Vector2f(-100f, -100f); // Återställer powerUp till en statisk position utanför skärmen
            powerUp.direction = new Vector2f(0, 0);
        }
        if (timer > 4) // När timern går över 4 sekunder återställs paddle till ursprungsvärden
        {
            sprite.Scale = new Vector2f(
                Diameter /paddleTextureSize.Y,
                Diameter / paddleTextureSize.Y);
            size = new Vector2f(
                sprite.GetGlobalBounds().Width,
                sprite.GetGlobalBounds().Height);
            timer = 0;
        }
    }
    
}