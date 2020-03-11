﻿using System;
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
                var time = Time.time;
                var logic = new AimLogic(_settings.aim, Time.time);
                yield return StartCoroutine(AimPhase(button, logic));
                time = Time.time;
                GoToNextState();

                //Anim
                yield return StartCoroutine(AnimPhase(logic,time));
            
                //Score
                yield return StartCoroutine(ShowScorePhase(logic,time));

                //Reset 
                Reset(); 
                yield return null;

                GoToNextState();

            }
        }

        private IEnumerator ShowScorePhase(AimLogic logic, float time){

            _visuals.Camera.GoToScorePoint();
            AimState state= logic.GetCurrentAimState(time);
            yield return StartCoroutine(_visuals.Scores.ShowScore(state.Score));
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

        private IEnumerator AnimPhase(AimLogic logic,float time){

            _visuals.Indicator.enabled = false;
            _visuals.MainButton.enabled = false;

            Vector3 init = logic.GetHitPoint(time);

            Vector3 relative = _visuals.AimTarget.RelativePoint(init);
            Func<float, Vector3> trajectory = logic.Trajectory(relative);

            _visuals.Camera.StartFollowing(_visuals.AimTarget.Arrow);

            var flyPhase = _visuals.AimTarget.ArrowFly(Time.time,trajectory);
            yield return StartCoroutine(flyPhase);
        }


        private IEnumerator AimPhase(IPushable pushable, AimLogic aimLogic){

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