using SFML.Window;
using SFML.System;
using SFML.Graphics;

namespace breakout;
using SFML.System;

class Program
{
    public const int ScreenW = 500;
    public const int ScreenH = 700;
    static void Main(string[] args)
    {
        Ball ball = new Ball();
        Paddle paddle = new Paddle();
        Tile tiles = new Tile();

        using (var window = new RenderWindow(
                   new VideoMode(ScreenW, ScreenH), "breakout"))
        {
            window.Closed += (o, e) => window.Close();
            Clock clock = new Clock();
            while (window.IsOpen)
            {
                float dt =
                    clock.Restart().AsSeconds();
                window.DispatchEvents();
                if (ball.Health <= 0)
                {
                    resetGame(ball, paddle, tiles);
                }
                if (tiles.positions.Count <= 0)
                {
                    resetGame(ball, paddle, tiles);
                }
                paddle.Update(ball, dt);
                ball.Update(dt, paddle);
                tiles.Update(ball, dt);
                window.Clear(new Color(131, 197, 235));
                paddle.Draw(window);
                ball.Draw(window);
                tiles.Draw(window);
                window.Display();
            }
        }
    }

    public static void resetGame(Ball ball, Paddle paddle, Tile tiles)
    {
        ball.Health = 3;
        ball.Score = 0;
        paddle.sprite.Position = new Vector2f(ScreenW / 2, ScreenH - 20);
        tiles.positions.Clear();
        ball.sprite.Position = ball.spawnPosition;
        tiles.createTiles();
    }
    
    

}