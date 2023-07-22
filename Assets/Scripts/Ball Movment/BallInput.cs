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
        pos.y = transform.position.y;
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

    private void Awake()
    {
        mainCamera = Camera.main;
    }


    private void OnDisable()
    {
        press.Disable();
        screenPos.Disable();
    }

    private IEnumerator Drag()
    {
        isDragging = true;
        dragStartPositionWorld = transform.position;
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
            worldPoint = hit.point;
            worldPoint.y = transform.position.y;
            OnDragAction.Invoke(worldPoint);           
        }
    }

    void OnEndDrag() {
        Ray ray = Camera.main.ScreenPointToRay(dragcurrentPositionScreenAxis);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            worldPoint = hit.point;
            worldPoint.y = transform.position.y;
            OnEndDragAction.Invoke(worldPoint);
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