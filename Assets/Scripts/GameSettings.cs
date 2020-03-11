using System;
using UnityEngine;

[Serializable]
public class GameSettings{
    public float TimeOut;
    public AimingSettings aim;
    public VisualSettings visual;
    public SoundSettings audio;
}

[Serializable]
public class AimingSettings{
    public AimZone[] Zones;
    public float HalfTime;
    public AnimationCurve InidictorCurve;


    public class AimZone{

        public float Chance;
        public float Distance = 1f;
        public int Score = 10;

        public Color Color;

        private float _percentageChange=-1f;
        private float _prevDistance=-1f;
        private bool _wasInit = false;
        public float MinDist{
            get{
                CheckInit();
                return _prevDistance;
            }
        }
        public float MaxDist{
            get{
                CheckInit();
                return _prevDistance+Distance;
            }
        }
        public float PercentageChance
        {
            get {
                CheckInit();
                return _percentageChange;
            }
        }

        public void Init(float totalChance, float prevDistance){
            _wasInit = true;
            if(totalChance<=0f)
                throw new ArgumentException("Total chances could not be less then 0");
            _percentageChange = Chance / totalChance;

            _prevDistance = prevDistance;

        }


        private void CheckInit(){
            if (!_wasInit)
                throw new Exception("Didn't initialize properly AimZone");
        }
    }
}

[Serializable]
public class VisualSettings{
    public float ButtonStateTransitionTime = 0.5f;
    public ButtonVisualState InitButState;
    public ButtonVisualState AimButState;
    public ButtonVisualState FinalButState;


}

public class ButtonVisualState{
    public Color Color;
    public string Text;
}


[Serializable]
public class SoundSettings{
    public AudioClip _clickSound;
    public AudioClip _arrowFlySound;
    public AudioClip _arrowHitSound;
    public AudioClip _gameScoreSound;
    

}