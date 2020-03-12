using System;
using System.Collections;
using Settings;
using UnityEngine;

namespace Logic{
    public class GameManager : MonoBehaviour{


        [SerializeField] private GameSettings _settings;
        private GameState _gameState;
        private VisualSettings _visuals;


        public void Start(){
            _settings.Init();
            AudioSubscriptions();
            StartCoroutine(LifeCycle());
        
        }

        private void AudioSubscriptions(){

            AudioManger audio = new AudioManger(_settings.audio);
            _visuals = _settings.visual;
            _visuals.MainButton.OnPushed += () => audio.PlaySound(AudioManger.Sounds.Click);
            _visuals.AimTarget.OnShooted += () => audio.PlaySound(AudioManger.Sounds.Fly);
            _visuals.AimTarget.OnHit += () => audio.PlaySound(AudioManger.Sounds.Bang);
        }

    

        public IEnumerator LifeCycle(){
            IPushable button =_visuals.MainButton;

            while (true){


                //Start
                yield return new WaitForPushable(button);
                GoToNextState();

                //Aiming
                var logic = new AimLogic(_settings.aim, Time.time);

                yield return StartCoroutine(AimPhase(button, logic));
                var state = logic.GetCurrentAimState(Time.time);
                var trajectory = GetTrajectory(logic, state.ZoneIndex);

                GoToNextState();

                //Anim
                yield return StartCoroutine(AnimPhase(trajectory));
            
                //Score
                yield return StartCoroutine(ShowScorePhase(state));

                //Reset 
                Reset(); 
                yield return null;

                GoToNextState();

            }
        }

        
        private Func<float, Vector3> GetTrajectory(AimLogic logic,int index){
            Vector3 init = logic.GetHitPoint(index);
            Vector3 start = _visuals.AimTarget.Arrow.position;
            Vector3 end = _visuals.AimTarget.RelativePoint(init);
            Func<float, Vector3> trajectory = logic.Trajectory(start,end);
            return trajectory;
        }


        private IEnumerator ShowScorePhase(AimState state ){

            _visuals.Camera.GoToScorePoint();

            if (state.Score>1){
                yield return StartCoroutine(_visuals.Scores.ShowScore(state.Score));
            }
            else{
                _settings.visual.AimTarget.Reset();
                yield return StartCoroutine(_visuals.Scores.YouMissed());
            }

            _visuals.MainButton.enabled = true;
        }


        private void Reset(){

            _visuals.Indicator.enabled = false;
            _visuals.MainButton.enabled = true;

            _visuals.AimTarget.Reset();
            _visuals.Indicator.Reset();
            _visuals.Scores.Reset();
            _visuals.Camera.Reset();
        }

        private IEnumerator AnimPhase(Func<float,Vector3> t){

            _visuals.Camera.StartFollowing(_visuals.AimTarget.Arrow);

            var flyPhase = _visuals.AimTarget.ArrowFly(Time.time,t);
            yield return StartCoroutine(flyPhase);
        }


        private IEnumerator AimPhase(IPushable pushable, AimLogic aimLogic ){

            _visuals.Indicator.enabled = true;
            while (!pushable.IsPushed()){
                AimState state = aimLogic.GetCurrentAimState(Time.time);

                _visuals.Indicator.UpdateState(state);
                yield return null;
            }

            pushable.IsPushed();
            yield return null;
        }

        private void GoToNextState(){
            if (_gameState == GameState.Final)
                _gameState = GameState.Aiming;
            else
                _gameState++;

            ChangeState(_gameState);

        }

        private void ChangeState(GameState state){
            var visual = _visuals;
            _visuals.MainButton.ChangeState(visual.AimButState);
        }
    }
}