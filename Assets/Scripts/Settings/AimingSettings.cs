using System;
using System.Linq;
using UnityEngine;

namespace Settings{
    [Serializable]
    public partial class AimingSettings{
        public AimZone[] Zones;
        public float HalfTime;
        public AnimationCurve InidictorCurve;

        public void Init(float radius){
            //This one Could also be done by for.
            //But my slogan is "shorter is better";

            float prevDist = 0;

            float totalChance = Zones.Select(x => x.Chance).Sum();

            foreach (var zone in Zones){
                zone.Init(totalChance,prevDist);
                prevDist += radius*zone.Chance;
            } 
        }
    }
}