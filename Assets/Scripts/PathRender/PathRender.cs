using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathRender : MonoBehaviour
{
    [SerializeField] GameObject headOfLine;
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
            headOfLine.SetActive(false);
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

            DrawHeadOfLine(lineRenderer.GetPosition(resolution-2),lineRenderer.GetPosition(resolution-1));          

        }
    }

    

    private void DrawHeadOfLine(Vector3 startPoint,Vector3 endPoint)
    {
        headOfLine.SetActive(true);

        RaycastHit raycastHit;
        Vector3 direction  = endPoint - startPoint;
        bool hit = Physics.Raycast(endPoint-direction*10 , direction , out raycastHit,200);

        if (hit)
        {
            
            headOfLine.transform.position = raycastHit.point;
            Debug.Log("hit");
        }
        else
        {
            Debug.Log("no hit");
            headOfLine.transform.position = endPoint;
        }
    }

    internal void HidePath()
    {
        lineRenderer.enabled = false;        
    }
}
