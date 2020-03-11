using System;
using System.Linq;
using UnityEditor;

public class AimingLogic{
    private AimingSettings _settings;

    public AimingLogic(AimingSettings settings){
        _settings = settings;
       InitChances(settings);
    }


    private void InitChances(AimingSettings settings){
        //This one Could also be done by for.
        //But my slogan is "shorter is better";
        var zones = _settings.Zones;
        float totalChance = zones 
            .Select(x => x.Chance).Sum();

        float totalDistance = 0;
        foreach (var zone in zones){
            zone.Init(totalChance,totalDistance);
            totalDistance += zone.Distance;

        }

    }
}