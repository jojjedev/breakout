using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace breakout;

public class Tile
{
    public Sprite sprite;
    public const float Diameter = 30.0f;
    public Vector2f size;
    public List<Vector2f> positions;
    
    public Tile()
    {
        sprite = new Sprite();
        sprite.Texture = new Texture("assets/tileBlue.png");
        Vector2f tileTextureSize = (Vector2f)sprite.Texture.Size;
        sprite.Origin = 0.5f * tileTextureSize;
        sprite.Scale = new Vector2f(
            Diameter / tileTextureSize.Y,
            Diameter / tileTextureSize.Y);
        size = new Vector2f(
            sprite.GetGlobalBounds().Width,
            sprite.GetGlobalBounds().Height);
        positions = new List<Vector2f>();
        createTiles();
        
    }

    public Tile(string color)
    {
        sprite = new Sprite();
        sprite.Texture = new Texture(color);
        Vector2f tileTextureSize = (Vector2f)sprite.Texture.Size;
        sprite.Origin = 0.5f * tileTextureSize;
        sprite.Scale = new Vector2f(
            Diameter / tileTextureSize.Y,
            Diameter / tileTextureSize.Y);
        size = new Vector2f(
            sprite.GetGlobalBounds().Width,
            sprite.GetGlobalBounds().Height);
        positions = new List<Vector2f>();
        createTiles();
        
    }

    public void Draw(RenderTarget target)
    {
        for (int i = 0; i < positions.Count; i++)
        {
                switch (i % 3)
                {
                    case 0:
                        sprite.Texture = new Texture("assets/tileBlue.png");
                        break;
                    case 1:
                        sprite.Texture = new Texture("assets/tileGreen.png");
                        break;
                    case 2:
                        sprite.Texture = new Texture("assets/tilePink.png");
                        break;
                }
            
            sprite.Position = positions[i];
            target.Draw(sprite);
        }
    }

    public void Update(Ball ball, float dt)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            var pos = positions[i];
            if(Collision.CircleRectangle(ball.sprite.Position, 
                   Ball.Radius, pos, size, out Vector2f hit))
            {
                ball.sprite.Position += hit;
                ball.Reflect(hit.Normalized());
                positions.RemoveAt(i);
                ball.Score += 100 + ball.BonusScore;
                ball.BonusScore += 10;
                i = 0;
            }
        }
    }

    public void createTiles()
    {
        for (int i = -2; i <= 2; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
                var pos = new Vector2f(
                    Program.ScreenW * 0.5f + i * 96.0f,
                    Program.ScreenH * 0.3f + j * 48.0f);
                    positions.Add(pos);
            }
            
        }
    }

    public static string setTileColor()
    {
        Random random = new Random();
        int index = random.Next(0, 3);
        string[] colorList = {
            "assets/tileBlue.png", 
            "assets/tileGreen.png", 
            "assets/tilePink.png"
        };
        return colorList[index];
    }
}