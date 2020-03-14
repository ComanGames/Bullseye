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



        public Vector3 GetHitPoint(int zoneIndex ){
            //We look first for the zone index 

            int i = zoneIndex;

            float radius = (i * _settings.ZoneSize)+Random.Range(0,_settings.ZoneSize);
            float angle = Random.Range(0, Mathf.PI * 2f);

            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;

            return new Vector3(x,y,0);

        }

        public AimState GetCurrentAimState(float time)
        {

            int cycles;
            float chances = GetAimPercentage(time,out cycles);
            float point = _settings.InidictorCurve.Evaluate(chances);
            int zoneIndex = GetCurrentZoneIndex(chances);


            bool missed = _zones[zoneIndex].Score<=1;
            Color color = _zones[zoneIndex].Color;
            var score = _zones[zoneIndex].Score;

            return new AimState(point,color,zoneIndex,score,missed,cycles);
        }


        private int GetCurrentZoneIndex(float chance){
            float totalChance = 0;
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
        private float GetAimPercentage(float time, out int cycles)
        {
            var hTime = _settings.HalfTime;
            var fullCycle = hTime * 2;
            var rTime = time - _startTime;
            var relativeTime = rTime % fullCycle;
             cycles = (int) (rTime / fullCycle);

             float f = relativeTime / hTime;
             return f<1?f:(2-f);
        }



        /// <summary>
        /// Give us trajectory from point 0,0,0 to dist 
        /// </summary>
        public Func<float, Vector3> DirectTraject(Vector3 start, Vector3 dist){

            return (x) => Vector3.Lerp(start, dist, x);
            
        }
        /// <summary>
        /// Give us trajectory from point 0,0,0 to dist 
        /// </summary>
        public Func<float, Vector3> KinematicTraject(Vector3 start, Vector3 dist){

            return _settings.NonLiner.GetFunc(start,dist);
            
        }

        public Vector3 GetMissPoint(){
            throw new NotImplementedException();
        }
    }

    public class AimInfo{
        public AimState state;

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
        public readonly bool Missed;
        public readonly int Cycles;

        public AimState(float point, Color color, int zoneIndex, int score,bool missed,int cycle){
            Point = point;
            Color = color;
            ZoneIndex = zoneIndex;
            Score = score;
            Missed = missed;
            Cycles = cycle;

        }

    }
}