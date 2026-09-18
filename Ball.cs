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
    public float speed = 3;
    public int Health = 3;
    public int Score;
    public int BonusScore;
    public Vector2f spawnPosition = new Vector2f(250, 400);
    public Text gui;
    public Vector2f newPos = new Vector2f(250, 400);
    public bool ballOnPaddle = true;

    public Ball()
    {
        sprite = new Sprite();
        sprite.Texture = new Texture("assets/ball.png");
        sprite.Position = spawnPosition;
        Vector2f ballTextureSize = (Vector2f)sprite.Texture.Size; //Får storleken på texturen från storleken på "ball.png"
        sprite.Origin = 0.5f * ballTextureSize; //Ändrar spritens referenspunkt till mitten av texturen
        sprite.Scale = new Vector2f(   //Bestämmer skalan på objeketet/spriten med hjälp av texture size
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
        newPos += direction * dt * 100.0f * speed;
        if (newPos.X > Program.ScreenW - Radius)
        {
            newPos.X = Program.ScreenW - Radius;
            Reflect(new Vector2f(-1, 0)); // Vid träff på högersidan anropas Reflect() -> bollen studsar ifrån sidan
        }
        if (newPos.X < 0 + Radius)
        {
            newPos.X = 0 + Radius;
            Reflect(new Vector2f(1, 0)); // Vid träff på vänstersidan anropas Reflect() -> bollen studsar ifrån sidan
        }
        if (newPos.Y < 0 + Radius)
        {
            newPos.Y = 0 + Radius;
            Reflect(new Vector2f(0, 1)); // Vid träff i taket anropas Reflect() -> bollen studsar ifrån sidan
        }
        if (newPos.Y > Program.ScreenH - Radius) // När bollen passerar nederkanten av skärmen minskas Health med 1 och boolean ballOnPaddle sätts till true.
        {
            Health--;
            ballOnPaddle = true;
            BonusScore = 0; // Nollställer bonus ifall boll når botten
        }

        if (ballOnPaddle) // När ballOnPaddle = true återställs positionen av ball till precis ovanför paddeln.
        {
            newPos.Y = paddle.sprite.Position.Y - paddle.size.Y / 2 - Radius - 5;
            newPos.X = paddle.sprite.Position.X;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space)) // När man trycker på space skickas bollen iväg med en slumpmässig riktning.
            {
                direction = new Vector2f(RandomDirection(), -1) / MathF.Sqrt(2.0f);
                ballOnPaddle = false;
            }
        }
        sprite.Position = newPos;
    }

    public static int RandomDirection() // Returnar -1 eller 1 vilket representerar vinkeln som bollen skickas iväg med.
    {
        
        int angle = new Random().Next(0,2);
        if (angle == 0)
        {
            return -1;
        }
        return 1;
    }
    
    public void Reflect(Vector2f normal) // Hanterar bollens direction vid studs med objekt/sida.
    {
        direction -= normal * (2 * (
            direction.X * normal.X +
            direction.Y * normal.Y));
    }
}