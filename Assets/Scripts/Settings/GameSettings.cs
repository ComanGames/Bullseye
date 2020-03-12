using System;

namespace Settings{
    [Serializable]
    public class GameSettings{
        public AimingSettings aim;
        public VisualSettings visual;
        public SoundSettings audio;

        public void Init(){
            aim.Init(visual.AimTarget.Radius);
            visual.Init();
        }

    }
}