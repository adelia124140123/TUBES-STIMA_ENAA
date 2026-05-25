using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class ENAA_Gaia : Bot
{
    private bool peek;
    private double moveAmount;
    private bool dodging = false; 
    private readonly Random rng = new Random();

    static void Main(string[] args) => new ENAA_Gaia().Start();
    ENAA_Gaia() : base(BotInfo.FromFile("ENAA_Gaia.json")) { }

    public override void Run()
    {
        BodyColor   = Color.FromArgb(143, 188, 143);
        TurretColor = Color.FromArgb(112, 128, 144); 
        RadarColor  = Color.FromArgb(176, 224, 230); 
        ScanColor   = Color.FromArgb(200, 230, 200); 
        BulletColor = Color.FromArgb(210, 180, 140); 
        TracksColor = Color.FromArgb(105, 105, 105);
        GunColor    = Color.FromArgb(119, 136, 153); 

        moveAmount = Math.Max(ArenaWidth, ArenaHeight);
        peek = false;

        TurnRight(Direction % 90);
        Forward(moveAmount);

        peek = true;
        TurnGunRight(90);
        TurnRight(90);

        while (IsRunning)
        {
            peek = true;
            Forward(moveAmount); 
            peek = false;
            TurnRight(90);         
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        double distance    = DistanceTo(e.X, e.Y);
        double firePower   = GetFirePower();
        double bulletSpeed = 20.0 - 3.0 * firePower;
        double travelTime  = distance / bulletSpeed;

        double predX = e.X + Math.Sin(e.Direction * Math.PI / 180.0) * e.Speed * travelTime;
        double predY = e.Y + Math.Cos(e.Direction * Math.PI / 180.0) * e.Speed * travelTime;

        double gunTurn = GunBearingTo(predX, predY);
        TurnGunRight(gunTurn);

        Fire(firePower);

        if (peek) Rescan();
    }

    public override void OnHitByBullet(HitByBulletEvent e)
    {
        if (dodging) return;
        dodging = true;

        Forward(rng.Next(30, 80));
        TurnRight(90);
        Forward(rng.Next(50, 150));

        dodging = false;
    }

    public override void OnHitBot(HitBotEvent e)
    {
        var bearing = BearingTo(e.X, e.Y);
        if (bearing > -90 && bearing < 90)
            Back(100);
        else
            Forward(100);
    }

    public override void OnHitWall(HitWallEvent e)
    {
        Back(20);
    }

    private double GetFirePower()
    {
        if (Energy < 10) return 0.5;
        if (Energy < 20) return 1.5;
        return 3.0; 
    }
}
