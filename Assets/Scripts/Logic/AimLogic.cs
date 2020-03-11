using System;
using Settings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Logic{
    public class AimLogic{

        private readonly AimingSettings _settings;

        private float _startTime;
        private AimingSettings.AimZone[] _zones;

        public AimLogic(AimingSettings settings,float time){
            _settings = settings;
            _zones = _settings.Zones;
            _startTime = time;
        }



        public Vector3 GetHitPoint(float time){
            //We look first for the zone index 
            AimState state = GetCurrentAimState(time);
            int i = state.ZoneIndex;

            float radius = Random.Range(_zones[i].MinDist, _zones[i + 1].MaxDist);
            float angle = Random.Range(0, Mathf.PI * 2f);

            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            return new Vector3(x,y,0);

        }

        public AimState GetCurrentAimState(float time){

            float chances = GetAimPercentage(time);
            float chanceEval = _settings.InidictorCurve.Evaluate(Math.Abs(chances));
            int zoneIndex = GetCurrentZoneIndex(chances);
            var point = IndicatorPoint(chances,chanceEval); //should be from 1to0 from0 to -1/
            //JIC: IndicatorPoint(chances, chanceEval);

            Color color = _zones[zoneIndex].Color;
            var score = _zones[zoneIndex].Score;

            return new AimState(point,color,zoneIndex,score);
        }

        private static float IndicatorPoint(float chances, float chanceEval){
            return (chanceEval*2) -2f;
        }

        private int GetCurrentZoneIndex(float chance){
            float totalChance = 0;
            if(chance<1)
                chance = 1 - chance;
            else
                chance =  (chance-1);

            for (int i = 0; i < _zones.Length-1; i++){
                totalChance += _zones[i].PercentageChance;
                if (chance <= totalChance)
                    return i;
            }

            return _zones.Length - 1;

        }

        /// <summary>
        ///  Returns values from 0 to 1 if first half and from -1 to 0 if second half  
        /// </summary>
        private float GetAimPercentage(float time){
            float chances;
            float hTime = _settings.HalfTime;
            float relativeTime = (time-_startTime) % (hTime * 2);

            return relativeTime/hTime;
        }



        /// <summary>
        /// Give us trajectory from point 0,0,0 to dist 
        /// </summary>
        public Func<float, Vector3> Trajectory(Vector3 dist){

            return (x) => Vector3.Lerp(Vector3.zero, dist, x);

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
        public readonly int Score;
        public AimState(float point, Color color, int zoneIndex, int score){
            Point = point;
            Color = color;
            ZoneIndex = zoneIndex;
            Score = score;
        }

    }
}