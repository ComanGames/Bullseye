using System;
using System.Linq;
using UnityEngine;
using static AimingSettings;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;
using Vector2 = System.Numerics.Vector2;

public class AimingLogic{

    private readonly AimingSettings _settings;

    private float _startTime;
    private AimZone[] _zones;

    public AimingLogic(AimingSettings settings,float time){
        _settings = settings;
        _zones = _settings.Zones;
       _startTime = time;
    }



    public Vector2 GetHitPoint(float time){
        //We look first for the zone index 
        AimState state = GetCurrentIimState(time);
        int i = state.ZoneIndex;

        float radius = Random.Range(_zones[i].MinDist, _zones[i + 1].MaxDist);
        float angle = Random.Range(0, Mathf.PI * 2f);

        float x = Mathf.Sin(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        return new Vector2(x,y);

    }

    public AimState GetCurrentIimState(float time){

        float chances = GetAimPercentage(time);
        float chanceEval = _settings.InidictorCurve.Evaluate(Math.Abs(chances));
        int zoneIndex = GetCurrentZoneIndex(chanceEval);
        var point = IndicatorPoint(chances, chanceEval);
        Color color = _zones[zoneIndex].Color;

        return new AimState(point,color,zoneIndex);
    }

    private static float IndicatorPoint(float chances, float chanceEval){
        return chances>=0?chanceEval/2: (1+chanceEval*.5f)+.5f;
    }

    private int GetCurrentZoneIndex(float chance){
        float totalChance = 0;
        chance = 1 - chance;
        for (int i = 0; i < _zones.Length-1; i++){
            totalChance += _zones[i].PercentageChance;
            if (chance <= totalChance)
                return i;
        }

        return _zones.Length - 2;

    }

    /// <summary>
    ///  Returns values from 0 to 1 if first half and from -1 to 0 if second half  
    /// </summary>
    private float GetAimPercentage(float time){
        float chances;
        float hTime = _settings.HalfTime;
        float relativeTime = (time-_startTime) % (hTime * 2);

        if (relativeTime < hTime)
            //For the first half on the indicator from 0 to 0.5
            chances = relativeTime / hTime;
        else
            //For the first half on the indicator from 0.5 to 1 
            chances = (1f - ((relativeTime % hTime))/hTime) * -1;
        return chances;
    }

}

public class AimState{
    /// <summary>
    /// Point on indicator in range [0,1]
    /// </summary>
    public readonly float Point;
    /// <summary>
    ///  Color representing current aiming state
    /// </summary>
    public readonly Color Color;
    /// <summary>
    ///  Color representing current aiming state
    /// </summary>
    public readonly int ZoneIndex;

    public AimState(float point, Color color,int index){
        Point = point;
       Color = color;
       ZoneIndex = index;
    }
}
