using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour{

    //Mono Refs 
    [SerializeField] private InputButton _mainButton;
    [SerializeField] private AimIndicator _aimIndicatr;


    [SerializeField]
    private GameSettings _settings;
    private GameState _gameState;
    private AimingLogic _aimingLogic;


    public void Start(){
        _aimingLogic = new AimingLogic(_settings.aim,Time.time);
        StartCoroutine(LifeCycle());
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
        _mainButton.ChangeSate(state);

    }
}