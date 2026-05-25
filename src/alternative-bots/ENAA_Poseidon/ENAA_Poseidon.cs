using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class ENAA_Poseidon : Bot
{
    private readonly Random rng = new Random();

    static void Main(string[] args) => new ENAA_Poseidon().Start();
    ENAA_Poseidon() : base(BotInfo.FromFile("ENAA_Poseidon.json")) { }

    public override void Run()
    {
        BodyColor   = Color.MidnightBlue;
        TurretColor = Color.DarkBlue;
        RadarColor  = Color.Cyan;
        ScanColor   = Color.Aquamarine;
        BulletColor = Color.LightBlue;
        TracksColor = Color.Black;
        GunColor    = Color.DeepSkyBlue;

        while (IsRunning)
        {
            SetTurnLeft(10_000); 
            MaxSpeed = 5;
            Forward(10_000);
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        Fire(3);
    }

    public override void OnHitByBullet(HitByBulletEvent e)
    {
        int distance = rng.Next(50, 150);
        if (rng.Next(2) == 0)
            Forward(distance);
        else
            Back(distance);
    }

    public override void OnHitBot(HitBotEvent e)
    {
        var bearing = BearingTo(e.X, e.Y);
        if (bearing > -10 && bearing < 10)
        {
            Fire(3);
        }
        if (e.IsRammed)
        {
            TurnLeft(10);
        }
    }

    public override void OnHitWall(HitWallEvent e)
    {
        Back(rng.Next(50, 100));
    }
}
