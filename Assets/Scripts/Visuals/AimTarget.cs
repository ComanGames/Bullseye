using System;
using System.Collections;
using UnityEngine;

namespace Visuals{
    public class AimTarget:MonoBehaviour{
        [Header("Params")] 
        public float FlyTimeout;
        public float HitDelay=0.2f;

        public float Radius{
            get {
                var start = Surface.position;
                var end = Border.position;

                float distance = Vector3.Distance(start, end);

                return distance;
            }
        } 

        [Header("Refs")]
        public Transform Arrow;
        public Transform Surface;
        public Transform Border;
        public GameObject Bow;

        public event Action OnShooted;
        public event Action OnHit;

        private Vector3 _arrowInitPos;
        private Quaternion _arrowInitRot;

        private void Awake() {
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
            HideBow();

            while (f<1f){

                f = (Time.time - startTime) / FlyTimeout;
                var pos = trajectory(f);
                var future = trajectory(f + 0.1f);

                Arrow.position = pos;

                yield return null;
            }

            if (OnHit != null) 
                OnHit.Invoke();

            yield return new WaitForSeconds(HitDelay);
            ShowBow();


        }

        public Vector3 RelativePoint(Vector3 init){
            return ((init) + Surface.position);

        }

        public void HideBow(){

            Bow.SetActive(false);
        }
        public void ShowBow(){

            Bow.SetActive(true);
        }
    }

}