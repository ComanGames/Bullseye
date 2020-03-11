using System.Collections;
using UnityEngine;
using Sounds = AudioManger.Sounds;

public class GameManager : MonoBehaviour{

    //Mono Refs 
    [SerializeField] private InputButton _mainButton;
    [SerializeField] private AimIndicator _aimIndicatr;


    [SerializeField]
    private GameSettings _settings;
    private GameState _gameState;
    private AimingLogic _aimingLogic;
    private AudioManger _audio;


    public void Start(){
        _settings.Init();
        AudioSubscriptions();
        _aimingLogic = new AimingLogic(_settings.aim,Time.time);
        StartCoroutine(LifeCycle());
        
    }

    private void AudioSubscriptions(){
        _mainButton.OnPushed += () => _audio.PlaySound(Sounds.Click);
    }

    public IEnumerator LifeCycle(){
        IPushable pushable = _mainButton;

        while (true){

            yield return new WaitForPushable(pushable);
            GoToNextState();
        }
    }

    private void GoToNextState(){
        if (_gameState == GameState.Final)
            _gameState = GameState.Aiming;
        else
            _gameState++;

        ChangeState(_gameState);

    }

    private void ChangeState(GameState state){

        _mainButton.ChangeState(_settings.visual.AimButState);
        

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