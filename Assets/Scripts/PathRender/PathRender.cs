using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathRender : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int resolution = 10;

    public void DrawProjectilePath(Vector3 startPoint, Vector3 endPoint, float angle)
    {
        lineRenderer.enabled = true;

        if (angle == 0)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);
        }
        else
        {
            Vector3 velocity = ProjectilePath.CalculateProjectileVelocity(startPoint, endPoint, angle);
            float timeOfFlight = 2 * velocity.y / -Physics.gravity.y;
            float timeDelta = timeOfFlight / resolution;
            lineRenderer.positionCount = resolution + 1;
            lineRenderer.SetPosition(0, startPoint);

            for (int i = 1; i <= resolution; i++)
            {
                float time = i * timeDelta;
                Vector3 point = startPoint + velocity * time + 0.5f * Physics.gravity * time * time;
                lineRenderer.SetPosition(i, point);
            }
        }
    }


    

    internal void HidePath(Vector3 startPoint)
    {
      
            lineRenderer.enabled = false;
        
    }
}
