using System;
using Logic;
using UnityEngine;

namespace Visuals{
    public class AimIndicator : MonoBehaviour{


        public Transform MovablePart;
        public Material ColorMat;
        public float MovingRange;

        public float ColorF=.1f;
    


        private Vector3 _center;
        private Color _currentColor;

        public void Start(){
            _center = MovablePart.position;
            _currentColor = ColorMat.color;
        }

        public void UpdateState(AimState state){

            MovablePart.position = _center + (Vector3.left * state.Point);

            _currentColor = Color.Lerp(_currentColor, state.Color, ColorF);
            ColorMat.color = _currentColor;

        }

        public void Reset(){
            throw new NotImplementedException();
        }
    }
}