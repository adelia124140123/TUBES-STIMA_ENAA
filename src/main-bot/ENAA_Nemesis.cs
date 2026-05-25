using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;
using System.IO;
using Microsoft.Extensions.Configuration;

public class ENAA_Nemesis : Bot
{
    private bool _hasTarget = false;

    private double _targetX;
    private double _targetY;

    private int _lockedBotId = -1;

    private int _lastScannedTurn = -1;

    private const int LOST_TARGET_TIMEOUT = 10; 

    static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("ENAA_Nemesis.json");

        var config = builder.Build();

        var botInfo = BotInfo.FromConfiguration(config);

        new ENAA_Nemesis(botInfo).Start();
    }

    private ENAA_Nemesis(BotInfo botInfo) : base(botInfo) { }

    public override void Run()
    {
        BodyColor   = Color.FromArgb(0xFF, 0xC2, 0x18, 0x5B); 
        TurretColor = Color.FromArgb(0xFF, 0x8B, 0x00, 0x38); 
        RadarColor  = Color.FromArgb(0xFF, 0xF0, 0x62, 0x92); 
        ScanColor   = Color.FromArgb(0xFF, 0xFF, 0x14, 0x93); 
        BulletColor = Color.FromArgb(0xFF, 0x3D, 0x0B, 0x1F); 
        TracksColor = Color.FromArgb(0xFF, 0x88, 0x00, 0x33); 
        GunColor    = Color.FromArgb(0xFF, 0xAD, 0x14, 0x57); 

        while (IsRunning)
        {
            SetTurnLeft(1.7);

            MaxSpeed = 6;

            SetForward(10_000);

            bool targetLost = _hasTarget &&
                              (_lastScannedTurn < 0 ||
                               TurnNumber - _lastScannedTurn > LOST_TARGET_TIMEOUT);

            if (targetLost)
            {
                _hasTarget = false;
                _lockedBotId = -1;
                _lastScannedTurn = -1;

                SetTurnRadarLeft(360);
            }

            else if (!_hasTarget)
            {
                SetTurnRadarLeft(360);
            }

            Go();
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        if (!_hasTarget)
        {
            _hasTarget = true;
            _lockedBotId = e.ScannedBotId;
        }

        if (e.ScannedBotId != _lockedBotId) return;

        _lastScannedTurn = TurnNumber;

        _targetX = e.X;
        _targetY = e.Y;

        double radarBearing = RadarBearingTo(e.X, e.Y);

        SetTurnRadarLeft(radarBearing * 1.5);

        double firePower   = CalcFirePower(DistanceTo(e.X, e.Y));

        double bulletSpeed = 20 - (3 * firePower);

        double ticks       = DistanceTo(e.X, e.Y) / bulletSpeed;

        double predictedX = e.X + Math.Sin(ToRad(e.Direction)) * e.Speed * ticks;

        double predictedY = e.Y + Math.Cos(ToRad(e.Direction)) * e.Speed * ticks;

        double gunBearing = GunBearingTo(predictedX, predictedY);

        SetTurnGunLeft(gunBearing);

        if (Math.Abs(gunBearing) <= 15 && GunHeat == 0)
            Fire(firePower);
    }

    public override void OnBotDeath(BotDeathEvent e)
    {
        if (e.VictimId == _lockedBotId)
        {
            _hasTarget = false;
            _lockedBotId = -1;
            _lastScannedTurn = -1;
        }
    }

    public override void OnHitByBullet(HitByBulletEvent e)
    {
        SetBack(30);
    }

    public override void OnHitWall(HitWallEvent e)
    {
        SetBack(40);
    }

    public override void OnHitBot(HitBotEvent e)
    {
        if (e.IsRammed) TurnLeft(10);
    }

    private double CalcFirePower(double distance)
    {
        
        if (distance < 150) return 3;

        if (distance < 400) return 2.5;

        return 2;
    }

    private double ToRad(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
}