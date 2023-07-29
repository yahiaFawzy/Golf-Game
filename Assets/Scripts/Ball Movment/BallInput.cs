using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class BallInput : MonoBehaviour
{

    [SerializeField] InputAction press;
    [SerializeField] InputAction screenPos;
    private Camera mainCamera;
    private Vector3 dragcurrentPositionScreenAxis;
    private bool isDragging;
    private Vector3 dragStartPositionWorld;

    public Action<Vector3> OnEndDragAction;
    public Action<Vector3> OnDragAction;



    //public GameObject follow;

    private void OnEnable()
    {

        press.Enable();
        screenPos.Enable();
        screenPos.performed += context => {  dragcurrentPositionScreenAxis = context.ReadValue<Vector2>(); };
        press.performed += _ => { if (isClickedOn) StartCoroutine(Drag()); };
        press.canceled += _ => { isDragging = false; };
    }

  

    private Vector3 ScreenToWorld(Vector3 screenPosition)
    {       
        var pos =   mainCamera.ScreenToWorldPoint(screenPosition);
        pos.y = ball.transform.position.y;
        return pos;
        
    }

    private bool isClickedOn
    {
        get
        {
            Ray ray = mainCamera.ScreenPointToRay(dragcurrentPositionScreenAxis);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                return hit.transform == transform;
            }
            return false;
        }
    }


    Vector3 worldPoint;
    [SerializeField] GameObject ball;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        transform.position = ball.transform.position+Vector3.up*0.5f;
    }

    private void OnDisable()
    {
        press.Disable();
        screenPos.Disable();
    }

    private IEnumerator Drag()
    {
        isDragging = true;
        dragStartPositionWorld = ball.transform.position;
        // grab
        while (isDragging)
        {
            // dragging
            OnDrag();
            yield return null;
        }
        // drop 
        OnEndDrag();
    }

    void OnDrag() {
        Ray ray = Camera.main.ScreenPointToRay(dragcurrentPositionScreenAxis);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject != this)
            {
                worldPoint = hit.point;
                worldPoint.y = ball.transform.position.y;
                OnDragAction.Invoke(worldPoint);
            }
            else
            {
                //drag canceled
            }
        }
    }

    void OnEndDrag() {
        Ray ray = Camera.main.ScreenPointToRay(dragcurrentPositionScreenAxis);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject != gameObject)
            {
                worldPoint = hit.point;
                worldPoint.y = ball.transform.position.y;
                OnEndDragAction.Invoke(worldPoint);
            }
            else { 
              //drag canceled
            }
        }

    }


    private void OnDrawGizmos()
    {

        if (mainCamera)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position,worldPoint);
        }
    }


}