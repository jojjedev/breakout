using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace breakout;

public class Tile
{
    public Sprite sprite;
    public const float Diameter = 30.0f;
    public Vector2f size;
    public List<Vector2f> positions; // Skapar en tom lista som ska hålla alla koordinater där det ska vara en tile.
    
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
        createTilePositions(); // Anropar funktionen som fyller listan med koordinater.
        
    }

    public void Draw(RenderTarget target)
    {
        for (int i = 0; i < positions.Count; i++) // Går igenom listan av koordinater
        {
            sprite.Position = positions[i]; // Sätter en sprite på en koordinat i listan enligt index.
            switch (positions[i].Y) // Beroende på y-koordinaten sätts texturen för den specifika koordinaten.
            {
                case < 115:
                    sprite.Texture = new Texture("assets/tilePink.png");
                    break;
                case > 115 and < 165:
                    sprite.Texture = new Texture("assets/tileBlue.png");
                    break;
                case > 165 and < 215:
                    sprite.Texture = new Texture("assets/tileGreen.png");
                    break;
                case > 215 and < 260:
                    sprite.Texture = new Texture("assets/tilePink.png");
                    break;
                case > 260:
                    sprite.Texture = new Texture("assets/tileBlue.png");
                    break;
            }
            target.Draw(sprite);
        }
    }

    public void Update(Ball ball, PowerUp powerUp, float dt)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            var pos = positions[i];  // pos = koordinaten för det specifika indexet i.
            if(Collision.CircleRectangle(ball.sprite.Position, // Kontrollerar kollision mellan ball och specifika tile vid koordinaten
                   Ball.Radius, pos, size, out Vector2f hit))
            {
                ball.sprite.Position += hit;
                ball.Reflect(hit.Normalized());
                
                if (powerUp.direction.Y == 0)  // Om powerUps direction är 0 så går vi in i denna if-sats och slumpar ett värde mellan 0-9
                {
                    Random random = new Random();
                    int chance = random.Next(0, 10);
                    if (chance == 0) // Om det slumpade värdet är 0, så sätts powerUps position till koordinaten för tilen som träffades
                    {
                        powerUp.newPos = positions[i];
                        powerUp.direction = new Vector2f(0, 1) / MathF.Sqrt(4.0f); //PowerUp direction sätts till rakt ner med farten 1/sqrt(4)
                    }
                   
                }
                
                positions.RemoveAt(i);
                ball.Score += 100 + ball.BonusScore;
                ball.BonusScore += 10;
                i = 0;
            }
        }
    }

    public void createTilePositions()
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
}