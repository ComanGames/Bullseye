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
        public Color Color;

        private float _realChance=-1f;

        public void Init(float totalChances){
            if(totalChances<=0)
                throw new ArgumentException("Total chances could not be less then 0");
            _realChance = Chance / totalChances;
        }
        public float GetChance(){
            if (_realChance == -1)
                throw new Exception("Didn't initialize properly AimZone");
            return _realChance;
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