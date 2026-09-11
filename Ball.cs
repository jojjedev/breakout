using System.Net.Http.Headers;
using System.Numerics;
using SFML.Graphics;
using SFML.System;

namespace breakout;

public class Ball
{
    public Sprite sprite;
    public const float Diameter = 20.0f;
    public const float Radius = Diameter * .5f;
    public Vector2f direction = new Vector2f(1, 1) / MathF.Sqrt(2.0f);
    public int Health = 3;
    public int Score;
    public Text gui;

    public Ball()
    {
        sprite = new Sprite();
        sprite.Texture = new Texture("assets/ball.png");
        sprite.Position = new Vector2f(250, 300);
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

    public void Update(float dt)
    {
        var newPos = sprite.Position;
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
            newPos.X = 250;
            newPos.Y = 300;
            direction = new Vector2f(1, 1) / MathF.Sqrt(2.0f);
            Health--;
        }
        sprite.Position = newPos;
    }

    public void Reflect(Vector2f normal)
    {
        direction -= normal * (2 * (
            direction.X * normal.X +
            direction.Y * normal.Y));
    }
    
    
}