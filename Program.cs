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
                ball.Update(dt);
                paddle.Update(dt);
                window.Clear(new Color(131, 197, 235));
                ball.Draw(window);
                paddle.Draw(window);
                window.Display();
            }
        }

    }
}