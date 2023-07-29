using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class InputArea : MonoBehaviour
{
    private Input input;

    [SerializeField] float dragScale;
    [SerializeField] float drawScale;
    [SerializeField] float minDragDistance;

    [SerializeField] PathRender projectilePath;
    [SerializeField] MeshRenderer circleInput;
    public Action<Vector3> OnRealseBall;


    private void Awake()
    {
        input = GetComponent<Input>();
    }


    private void Start()
    {
        input.OnDrag += OnDrag;
        input.OnDragEndAction += OnDragEnd;
    }

    private void OnDragEnd(Vector3 startDragWorldPos, Vector3 endDragWorldPos)
    {
        projectilePath.HidePath();
        circleInput.enabled = true;

        Vector3 dragDirection = startDragWorldPos - endDragWorldPos;
        Vector3 dragAreaOffest = minDragDistance * dragScale / drawScale * dragDirection.normalized;

        Vector3 targetRealsePoint = transform.position + dragDirection * dragScale-dragAreaOffest;
        Vector3 drawPoint = transform.position + dragDirection * drawScale * -1;
        float inputDistance = Vector3.Distance(transform.position, drawPoint);


        if (inputDistance > minDragDistance)
        {
             //relase ball              
           OnRealseBall?.Invoke(targetRealsePoint); 
        }
       

    }

    

    private void OnDrag(Vector3 startDragWorldPos, Vector3 endDragWorldPos)
    {
        
        Vector3 dragDirection = startDragWorldPos - endDragWorldPos;

        
        Vector3 drawPoint = transform.position + dragDirection * drawScale * -1;

        float drawDistance = Vector3.Distance(transform.position, drawPoint);

        Vector3 dragAreaOffest = minDragDistance * dragScale / drawScale * dragDirection.normalized;
        Vector3 targetRealsePoint = transform.position + dragDirection * dragScale - dragAreaOffest;      



        Debug.DrawLine(transform.position, transform.position +  minDragDistance * dragDirection.normalized, Color.cyan);
        if (drawDistance > minDragDistance)
        {
            Debug.DrawLine(transform.position, transform.position + dragDirection * -1 * drawScale, Color.blue);
          //  Debug.DrawLine(transform.position, targetRealsePoint, Color.red);
            projectilePath.DrawProjectilePath(transform.position, targetRealsePoint, (int)RocetManager.Instance.rocet);
            circleInput.enabled = false;
        }
        else { 
            Debug.DrawLine(transform.position, transform.position + dragDirection * -1 * drawScale, Color.red);        
            projectilePath.HidePath();
            circleInput.enabled = true;
        }

    }


}
