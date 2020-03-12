using System;
using System.Linq;
using UnityEngine;

namespace Settings{
    [Serializable]
    public partial class AimingSettings{
        public AimZone[] Zones;
        public float HalfTime;
        public AnimationCurve InidictorCurve;
        public float Radius;

        public float ZoneSize{
            get {
                var size = Radius / (float)(Zones.Length-1);
                return size;
            }
        }

        public void Init(float radius){

            Radius = radius;
            float prevDist = 0;

            float totalChance = Zones.Select(x => x.Chance).Sum();

            foreach (var zone in Zones){
                zone.Init(totalChance,prevDist);
                prevDist += ZoneSize;
            } 
        }
    }
}