using SFML.Window;
using SFML.System;
using SFML.Graphics;

namespace breakout;
using SFML.System;
//TODO: Till nästa gång, fixa färger och hard bonus.
class Program
{
    public const int ScreenW = 500;
    public const int ScreenH = 700;
    static void Main(string[] args)
    {
        Ball ball = new Ball();
        Paddle paddle = new Paddle();
        Tile tiles = new Tile();
        PowerUp powerUp = new PowerUp(); //Skapar powerUp objekt, placeras utanför skärmen

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
                    resetGame(ball, paddle, tiles, powerUp); //När health blir 0 så anopas funktionene som ansvarar för att återställa spelet
                    
                }
                if (tiles.positions.Count <= 0) //När alla tiles är borta från listan så anopas funktionene som ansvarar för att återställa spelet
                {
                    resetGame(ball, paddle, tiles, powerUp);
                }
                paddle.Update(ball,dt, powerUp);
                ball.Update(dt, paddle);
                powerUp.Update(dt);
                tiles.Update(ball, powerUp, dt);
                
                window.Clear(new Color(131, 197, 235));
                paddle.Draw(window);
                ball.Draw(window);
                powerUp.Draw(window);
                tiles.Draw(window);
                window.Display();
            }
        }
    }

    public static void resetGame(Ball ball, Paddle paddle, Tile tiles, PowerUp powerUp)
    {
        ball.Health = 3;
        ball.Score = 0;
        ball.ballOnPaddle = true;
        ball.newPos = ball.spawnPosition;
        ball.direction = new Vector2f(Ball.RandomDirection(), 1) / MathF.Sqrt(2.0f);
        powerUp.newPos = powerUp.spawnPosition;
        powerUp.direction = new Vector2f(0, 0);
        paddle.sprite.Position = new Vector2f(ScreenW / 2, ScreenH - 20);
        tiles.positions.Clear();
        ball.sprite.Position = ball.spawnPosition;
        tiles.createTilePositions();
    }
    
    

}