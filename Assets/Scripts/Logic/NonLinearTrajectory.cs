using System;
using UnityEngine;

namespace Logic{
    [Serializable]
    public class NonLinearTrajectory{

        [Range(0,10)]
        public float Height =2f;
        

        private const float G = -18;//Graviy const

        LaunchData CalculateLaunchData(Vector3 startPos,Vector3 targetPos)
        {
            float displacementY = targetPos.y - startPos.y;
            Vector3 displacementXZ = new Vector3(targetPos.x - startPos.x, 0, targetPos.z - startPos.z);
            float time = Mathf.Sqrt(-2 * Height / G) + Mathf.Sqrt(2 * (displacementY - Height) / G);
            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * G * Height);
            Vector3 velocityXZ = displacementXZ / time;

            return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(G), time);
        }

        public Func<float, Vector3> GetFunc(Vector3 startPos, Vector3 targetPos){

            LaunchData data = CalculateLaunchData(startPos,targetPos);

            var relative = DisplacementFync(data);

            return (f) => startPos + relative(f);


        }

        private Func<float,Vector3> DisplacementFync(LaunchData data){

            Func<float,Vector3> result = (f) => {
                var t = data.timeToTarget * f;
                var forward = data.initialVelocity * t;
                var up = Vector3.up * G * t * t / 2f;
                return forward + up;
            };
            return result;

        }
       public void DrawPath(Vector3 startPos, Vector3 targetPos)
        {

            LaunchData launchData = CalculateLaunchData(startPos,targetPos);

            Vector3 previousDrawPoint = startPos;

            int resolution = 30;
            for (int i = 1; i <= resolution; i++)
            {
                float f = i / (float)resolution ;

                var displacement = DisplacementFync(launchData);
                Vector3 drawPoint = startPos + displacement(f);

                Debug.DrawLine(previousDrawPoint, drawPoint, Color.blue);
                previousDrawPoint = drawPoint;
            }
        }

       private struct LaunchData
        {
            public readonly Vector3 initialVelocity;
            public readonly float timeToTarget;

            public LaunchData(Vector3 initialVelocity, float timeToTarget)
            {
                this.initialVelocity = initialVelocity;
                this.timeToTarget = timeToTarget;
            }

        }
    }
}