using System.Net.Http.Headers;
using System.Numerics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace breakout;

public class Ball
{
    public Sprite sprite;
    public const float Diameter = 20.0f;
    public const float Radius = Diameter * .5f;
    public Vector2f direction = new Vector2f(RandomDirection(),1) / MathF.Sqrt(2.0f);
    public int Health = 3;
    public int Score;
    public int BonusScore;
    public Vector2f spawnPosition = new Vector2f(250, 400);
    public Text gui;
    private Vector2f newPos = new Vector2f(250, 400);
    private bool ballOnPaddle = false;

    public Ball()
    {
        sprite = new Sprite();
        sprite.Texture = new Texture("assets/ball.png");
        sprite.Position = spawnPosition;
        Vector2f ballTextureSize = (Vector2f)sprite.Texture.Size;
        sprite.Origin = 0.5f * ballTextureSize;
        sprite.Scale = new Vector2f(
            Diameter / ballTextureSize.X,
            Diameter / ballTextureSize.Y);
        gui = new Text();
        gui.CharacterSize = 24;
        gui.Font = new Font("assets/future.ttf");
    }

    public void Draw(RenderTarget target)
    {
        // Ritar ball
        target.Draw(sprite);
    
        // Ritar Health
        gui.DisplayedString = $"Health: {Health}";
        gui.Position = new Vector2f(12, 8);
        target.Draw(gui);
        
        //Ritar Score
        gui.DisplayedString = $"Score: {Score}";
        gui.Position = new Vector2f(Program.ScreenW - gui.GetGlobalBounds().Width - 12, 8);
        target.Draw(gui);
    }

    public void Update(float dt, Paddle paddle)
    {
       
        //newPos = sprite.Position;
        newPos += direction * dt * 100.0f;
        if (newPos.X > Program.ScreenW - Radius)
        {
            newPos.X = Program.ScreenW - Radius;
            Reflect(new Vector2f(-1, 0)); // Högersidan
        }
        if (newPos.X < 0 + Radius)
        {
            newPos.X = 0 + Radius;
            Reflect(new Vector2f(1, 0)); // Vänstersidan
        }
        if (newPos.Y < 0 + Radius)
        {
            newPos.Y = 0 + Radius;
            Reflect(new Vector2f(0, 1)); // Taket
        }
        if (newPos.Y > Program.ScreenH - Radius)
        {
            newPos.Y = paddle.sprite.Position.Y - paddle.size.Y / 2 - Radius - 5;
            newPos.X = paddle.sprite.Position.X;
            direction = new Vector2f(0, 0);
            ballOnPaddle = true;
            Health--;
            BonusScore = 0; // Nollställer bonus ifall boll når botten
        }

        if (ballOnPaddle)//newPos.Y == paddle.sprite.Position.Y - paddle.size.Y / 2 - Radius - 5)
        {
            newPos.Y = paddle.sprite.Position.Y - paddle.size.Y / 2 - Radius - 5;
            newPos.X = paddle.sprite.Position.X;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            {
                direction = new Vector2f(RandomDirection(), -1) / MathF.Sqrt(2.0f);
                ballOnPaddle = false;
            }
        }
        sprite.Position = newPos;
    }

    public static int RandomDirection()
    {
        
        int angle = new Random().Next(0,2);
        if (angle == 0)
        {
            return -1;
        }
        return 1;
    }
    
    public void Reflect(Vector2f normal)
    {
        direction -= normal * (2 * (
            direction.X * normal.X +
            direction.Y * normal.Y));
    }
}