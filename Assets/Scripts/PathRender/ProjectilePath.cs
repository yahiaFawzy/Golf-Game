using UnityEngine;
using System.Collections;

public class ProjectilePath 
{
	

 //	public static LaunchData CalculateLaunchData(Vector3 ball, Vector3 target, float maxHeigh)
	//{
		

	//		Vector3 displacementXZ = new Vector3(target.x - ball.x, 0, target.z - ball.z);

		
	//		float displacementY = target.y - ball.y;
	//		float gravity = Physics.gravity.y;

	//		//time for getting up
	//		float time = Mathf.Sqrt(-2 * maxHeigh / gravity) + Mathf.Sqrt(2 * (displacementY - maxHeigh) / gravity);

	//		Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * maxHeigh);
	//		Vector3 velocityXZ = displacementXZ / time;

	//		return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
		
		
		

	//}

	public static Vector3 CalculateProjectileVelocity(Vector3 startPoint, Vector3 endPoint, float angle)
	{
		if (angle == 0)
		{
			Vector3 direction = endPoint - startPoint;
			direction.y = 0;
			float distance = direction.magnitude;
			float velocity = distance;
			return velocity * direction.normalized;
		}
		else
		{
            Vector3 direction = endPoint - startPoint;
            direction.y = 0;
            float distance = direction.magnitude;
            float radianAngle = angle * Mathf.Deg2Rad;
            direction.y = distance * Mathf.Tan(radianAngle);
            float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * radianAngle));
            return velocity * direction.normalized;
          

        }
	}



	//public static void DrawPath(Vector3 startPoint,Vector3 targetPoint,float curve)
	//{
	//	LaunchData launchData = CalculateLaunchData(startPoint,targetPoint,curve);
	//	Vector3 previousDrawPoint = startPoint;

	//	int resolution = 30;
	//	for (int i = 1; i <= resolution; i++)
	//	{
	//		float simulationTime = i / (float)resolution * launchData.timeToTarget;
	//		Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * Physics.gravity.y * simulationTime * simulationTime / 2f;
	//		Vector3 drawPoint = startPoint + displacement;
	//		Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
	//		previousDrawPoint = drawPoint;
	//	}
	//}

	public struct LaunchData
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
