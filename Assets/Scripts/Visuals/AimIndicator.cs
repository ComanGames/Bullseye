using System;
using Logic;
using UnityEngine;
using static UnityEngine.Color;

namespace Visuals{
    public class AimIndicator : MonoBehaviour{


        public Transform MovablePart;
        public Material ColorMat;
        public float MovingRange;
        public float ColorF=1/3f;
        [SerializeField]
        public IndicatorType _type;

    


        private Vector3 _center;
        private Color _currentColor;

        public void Start(){
            _center = MovablePart.position;
            _currentColor = ColorMat.color;
        }

        public void UpdateState(AimState state)
        {
            if(_type == IndicatorType.Linear)
                LinearMovment(state);
            else
                NonLinearMovment(state);
        }

        private void LinearMovment(AimState state)
        {
            float f = (state.Point*2);
            Vector3 offset = Vector3.left * f;
            Vector3 start = _center +Vector3.right;
            MovablePart.position = start + offset;

            _currentColor = Lerp(_currentColor, state.Color, ColorF);
            ColorMat.color = _currentColor;
        }
        private void NonLinearMovment(AimState state)
        {
            float f=state.Point;
            if (state.Cycles%2 == 1)
                f = (1f - f)-1;




            MovablePart.position = _center + f*Vector3.left;
            _currentColor = Lerp(_currentColor, state.Color, ColorF);
            ColorMat.color = _currentColor;
        }

        public void Reset(){

        }
    }

    public enum IndicatorType
    {
        Linear,
        BiLinear
    }
}