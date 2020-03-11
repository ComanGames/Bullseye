using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Sounds = AudioManger.Sounds;
using Vector2 = System.Numerics.Vector2;

public class GameManager : MonoBehaviour{

    //Mono Refs 


    [SerializeField] private GameSettings _settings;
    private GameState _gameState;
    private VisualSettings _visuals;
    private AudioManger _audio;


    public void Start(){
        _settings.Init();
        AudioSubscriptions();
        StartCoroutine(LifeCycle());
        
    }

    private void AudioSubscriptions(){
        _visuals = _settings.visual;
        _visuals.MainButton.OnPushed += () => _audio.PlaySound(Sounds.Click);
        _visuals.AimTarget.OnShooted += () => _audio.PlaySound(Sounds.Fly);
        _visuals.AimTarget.OnHit += () => _audio.PlaySound(Sounds.Bang);
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
            yield return StartCoroutine(ShowScorePhase(logic,time,button));

            //Reset 
            Reset(); 
            yield return null;

            GoToNextState();

        }
    }

    private IEnumerator ShowScorePhase(AimLogic logic, float time, IPushable button){

        _visuals.Camera.GoToScorePoint();
        AimState state= logic.GetCurrentAimState(time);
         _visuals.Scores.ShowScore(state.Score);
         _visuals.MainButton.enabled = true;
        yield return new WaitForPushable(button);
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



public class AudioManger{


    private SoundSettings _settings;

    public AudioManger(SoundSettings settings){
        _settings = settings;
    }

    public enum Sounds{
        Click,
        Fly,
        Bang,
        Congrats,
    }

    public void PlaySound(Sounds sound){

    }

}