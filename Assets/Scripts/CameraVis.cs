using UnityEngine;

public class CameraVis : MonoBehaviour{
    public float FollowSmooth=.2f;

    private Vector3 _startPos;
    private Quaternion _startRot;

    public Transform ScorePoint;
    public Vector3 FollowOffset;
    public Vector3 FollowAngle;

    private Transform _target;


    public void Update(){
        if (_target != null)
           FollowObject(_target.position); 



    }

    public void FollowObject(Vector3 target){
        transform.position = Vector3.Lerp(transform.position, target + FollowOffset, FollowSmooth);
    }
    public void Reset(){
        _target = null;
        transform.position = _startPos;
        transform.rotation = _startRot;
    }

    public void StartFollowing(Transform target){
        _target = target;
    }

    public void GoToScorePoint(){
        _target = null;
        transform.position =  ScorePoint.position;
        transform.rotation = ScorePoint.rotation;
    }
    public void Awake(){
        _startPos = transform.position;
        _startRot = transform.rotation;
    }
}