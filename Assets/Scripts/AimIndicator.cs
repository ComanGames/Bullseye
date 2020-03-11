using System;
using UnityEngine;

internal class AimIndicator : MonoBehaviour{

    public Transform MovablePart;
    public Material ColorMat;
    public float MovingRange;

    [Range(0,.5f)]
    public float ColorFloatSpeed=.1f;
    


    private Vector3 _center;

    public void Start(){
        _center = MovablePart.position;
    }


    public void UpdateState(AimState state){

    }
}