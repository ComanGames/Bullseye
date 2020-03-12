using System;
using Logic;
using Visuals;

namespace Settings{
    [Serializable]
    public class VisualSettings{
        public float ButtonStateTransitionTime = 0.5f;
        public ButtonVisuals InitButState;
        public ButtonVisuals AimButState;
        public ButtonVisuals FinalButState;
    
        public AimTarget AimTarget;
        public InputButton MainButton;
        public AimIndicator Indicator;
        public ScorePanel Scores;

        public CameraVis Camera;

        public void Init(){
            MainButton.ChangeState(InitButState);
        }
    }
}