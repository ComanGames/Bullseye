using System;
using System.Collections;
using Logic;
using UnityEditorInternal;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Visuals{
    public class AimTarget:MonoBehaviour{
        [Header("Params")] 
        public float FlyTimeout;
        public float HitDelay=0.2f;
        public NonLinearTrajectory NonLinTra;


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

        public Transform MissZone;
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

        public void Update(){
            NonLinTra.DrawPath(Arrow.position,Surface.position);

        }

        public IEnumerator ArrowFly(float startTime, Func<float, Vector3> trajectory){
            bool hitted = false;
            float f = 0;

            if (OnShooted != null) 
                OnShooted.Invoke();
            HideBow();

            while (f<1f){

                f = (Time.time - startTime) / FlyTimeout;
                var pos = trajectory(f);
                var future = trajectory(f + Time.deltaTime);
                var rel = future - pos;
                Arrow.position = pos;
                Arrow.rotation = Quaternion.LookRotation(rel);





                //Having hit half second before
                if (f > 0.95f && OnHit != null && !hitted){
                    OnHit.Invoke();
                    hitted = true;
                }

                yield return null;
            }

            

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

        public Vector3 RandomMissPoint(){
            var zone = MissZone.localScale;
            float x = Random.Range(zone.x * -.5f, zone.x * .5f);
            float z = Random.Range(zone.z * -.5f, zone.z * .5f);
            var point = MissZone.position + new Vector3(x,0,z);
            return point;
        }
    }

}