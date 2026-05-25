using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class ENAA_Predator : Bot
{
    private int moveDir = 1;
    private int tick = 0;
    private readonly Random rng = new Random();

    static void Main(string[] args) => new ENAA_Predator().Start();
    ENAA_Predator() : base(BotInfo.FromFile("ENAA_Predator.json")) { }

    public override void Run()
    {
        BodyColor   = Color.FromArgb(15, 0, 0);     
        TurretColor = Color.FromArgb(180, 0, 0);     
        RadarColor  = Color.FromArgb(255, 50, 0);    
        ScanColor   = Color.FromArgb(255, 100, 0);  
        BulletColor = Color.FromArgb(255, 200, 0); 
        TracksColor = Color.FromArgb(60, 0, 0);   
        GunColor    = Color.FromArgb(220, 30, 0);

        AdjustGunForBodyTurn   = true;
        AdjustRadarForBodyTurn = true;
        AdjustRadarForGunTurn  = false;

        GunTurnRate = 20;

        while (IsRunning)
        {
            tick++;

            if (tick % 25 == 0)
            {
                moveDir *= -1;
                TurnRate = (rng.NextDouble() * 20 + 5) * (rng.Next(2) == 0 ? 1 : -1);
            }

            MaxSpeed    = 8;
            TargetSpeed = 8 * moveDir;

            Go();
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        double gunBearing = GunBearingTo(e.X, e.Y);
        GunTurnRate = gunBearing; // koreksi ke musuh

        if (GunHeat == 0)
        {
            Fire(GetFirePower());
        }

        Rescan();
    }

    public override void OnHitBot(HitBotEvent e)
    {
        var bearing = BearingTo(e.X, e.Y);
        if (bearing > -10 && bearing < 10)
            Fire(3);
        if (e.IsRammed)
            TurnRate = 10;
    }

    public override void OnHitByBullet(HitByBulletEvent e)
    {
        moveDir *= -1;
        tick = 0;
        TurnRate = 90 * (rng.Next(2) == 0 ? 1 : -1);
    }

    public override void OnHitWall(HitWallEvent e)
    {
        moveDir *= -1;
        tick = 0;
    }

    public override void OnWonRound(WonRoundEvent e)
    {
        TurnRate = 10;
    }

    private double GetFirePower()
    {
        if (Energy < 10) return 0.5;
        if (Energy < 20) return 1.5;
        return 3.0;
    }
}
