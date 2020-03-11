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
    public float AimingCycleTime;
    public AnimationCurve InidictorSpeedCurve;


    public class AimZone{
        public float Chance;
        public float Distance = 1f;
        public int Score = 10;

        public Color Color;

        private float _realChance=-1f;
        private float _realsDistance=-1f;
        private bool _wasInit = false;
        public float StartDistance{
            get{
                CheckInit();
                return _realsDistance;
            }
        }
        public float RealChance {
            get {
                CheckInit();
                return _realChance;
            }
        }

        public void Init(float totalChance, float totalDistance){
            _wasInit = true;
            if(totalChance<=0f)
                throw new ArgumentException("Total chances could not be less then 0");
            _realChance = Chance / totalChance;

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