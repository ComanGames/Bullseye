using System.Collections;
using UnityEngine;
using UnityStandardAssets.CinematicEffects;

namespace Visuals{
    public class CameraVis : MonoBehaviour{
        public float FollowSmooth=.2f;

        public ShakeTransformEventData ShakeData;
        public ShakeTransform Shaker;
        public MotionBlur Blur;

        private Vector3 _startPos;
        private Quaternion _startRot;

        public Transform ScorePoint;
        public Vector3 FollowOffset;
        public Vector3 FollowAngle;

        private Transform _target;


        public void Awake(){
            _startPos = transform.position;
            _startRot = transform.rotation;
            Blur.enabled = false;
        }

        public void Update(){
            if (_target != null)
                FollowObject(_target.position); 
        }

        private Vector3 _preShake;

        public void ShakeCamera(){
            Shaker.AddShakeEvent(ShakeData);
        }


        public void FollowObject(Vector3 target){
            Blur.enabled = true;
            transform.position = Vector3.Lerp(transform.position, target + FollowOffset, FollowSmooth);
        }

        public void Reset(){
            _target = null;
            transform.position = _startPos;
            transform.rotation = _startRot;
            Blur.enabled = false;
        }

        public void StartFollowing(Transform target){
            _target = target;
        }

        public void GoToScorePoint(){
            _target = null;
            transform.position =  ScorePoint.position;
            transform.rotation = ScorePoint.rotation;
        }
    }
}