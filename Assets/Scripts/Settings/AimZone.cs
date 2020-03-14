using System;
using UnityEngine;

namespace Settings{
    public partial class AimingSettings{
        [Serializable]
        public class AimZone{

            public float Chance;
            public int Score = 10;

            public Color Color;

            private float _percentageChange=-1f;
            private bool _wasInit = false;

            public float PercentageChance
            {
                get {
                    CheckInit();
                    return _percentageChange;
                }
            }

            public void Init(float totalChance){
                _wasInit = true;
                if(totalChance<=0f)
                    throw new ArgumentException("Total chances could not be less then 0");
                _percentageChange = Chance / totalChance;

            }


            private void CheckInit(){
                if (!_wasInit)
                    throw new Exception("Didn't initialize properly AimZon2e");
            }
        }
    }
}