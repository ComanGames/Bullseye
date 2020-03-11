using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.XR;

public class GameManager : MonoBehaviour{

    //Mono Refs 
    [SerializeField] private InputButton _mainButton;
    [SerializeField] private AimIndicator _aimIndicatr;


    public GameSettings Settings;
    private GameState _gameState;


    public void Start(){
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
        if (_gameState == GameState.Shooting)
            _gameState = GameState.Aiming;
        else
            _gameState++;

        ChangeState(_gameState);

    }

    private void ChangeState(GameState state){
        _mainButton.ChangeSate(state);

    }
}