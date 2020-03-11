using System;
using System.Collections;
using UnityEngine;

namespace Visuals{
    public class AimTargetorArrow:MonoBehaviour{
        [Header("Params")] 
        public float FlyTimeout;
        public float HitDelay=0.2f;

        [Header("Refs")]
        public Transform Arrow;
        public Transform Surface;
        public Transform Border;

        public event Action OnShooted;
        public event Action OnHit;

        private Vector3 _arrowInitPos;
        private Quaternion _arrowInitRot;

        private void Awake(){
            _arrowInitPos = Arrow.position;
            _arrowInitRot = Arrow.rotation;
        }

        public void Reset(){
            Arrow.position = _arrowInitPos;
            Arrow.rotation = _arrowInitRot;
        }


        public IEnumerator ArrowFly(float startTime, Func<float, Vector3> trajectory){
            float f = 0;

            if (OnShooted != null) 
                OnShooted.Invoke();

            while (f<1f){

                f = (Time.time - startTime) / FlyTimeout;
                var pos = trajectory(f);
                var future = trajectory(f + 0.1f);

                Arrow.position = pos+_arrowInitPos;

                yield return null;
            }

            if (OnHit != null) 
                OnHit.Invoke();

            yield return new WaitForSeconds(HitDelay);


        }

        public float Radius{
            get { return Vector3.Distance(Surface.position,Border.position); }
        }

        public Vector3 RelativePoint(Vector3 init){
            return ((Surface.rotation * init) + Surface.position) - _arrowInitPos;

        }
    }
}