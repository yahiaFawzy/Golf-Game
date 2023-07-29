using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{

    [SerializeField] InputAction press;
    [SerializeField] InputAction screenPos;
    [SerializeField] LayerMask Layer;
    private Camera mainCamera;

   [SerializeField] Vector3 startDragScreenPos, dragScreenPosition;


    public bool isDrag = false;
    Coroutine Drag;
    public Action<Vector3, Vector3> OnDragEndAction;
    public Action<Vector3, Vector3> OnDrag;



    private void Awake()
    {
        mainCamera = Camera.main;
    }



    private void OnEnable()
    {
        //press input 
        press.Enable();

        press.started += _ =>
        {
            startDragScreenPos = dragScreenPosition = Vector3.zero;

        };


            press.performed += _ =>
        {
            if (gameObject.activeInHierarchy&&isDrag==false)
            {
                startDragScreenPos = dragScreenPosition;
                if (isClickedOnMe)
                {
                    isDrag = true; 
                    Drag = StartCoroutine(DragCorotuine());
                }
            }
           
        };

     
        press.canceled += _ => {
            if (gameObject.activeInHierarchy&&isDrag)
            {

                Debug.Log("press canceld");
                isDrag = false; 
                StopCoroutine(Drag); 
                OnDragEnd();
                startDragScreenPos = dragScreenPosition = Vector3.zero;

            }
        };

        //screen position input
        screenPos.Enable();
        //screenPos.started += context => {
        //    Debug.Log("screen started");
        //    startDragScreenPos = context.ReadValue<Vector2>();
        //    startDragScreenPos = new Vector3(startDragScreenPos.x, startDragScreenPos.y, Camera.main.nearClipPlane);
        //};

        screenPos.performed += context => {
            Debug.Log("screen performed");
            dragScreenPosition = context.ReadValue<Vector2>();
            dragScreenPosition = new Vector3(dragScreenPosition.x, dragScreenPosition.y, Camera.main.nearClipPlane);
        };

        
        
    }


    private bool isClickedOnMe
    {
        get
        {
                Debug.Log("here on click");
            Ray ray = mainCamera.ScreenPointToRay(startDragScreenPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,1000,Layer))
            {
                Debug.Log("here ");
                return hit.transform == transform;
            }
            return false;
        }
    }

    void OnDragEnd()
    {
        Vector3 startDragWorldPos = Camera.main.ScreenToWorldPoint(startDragScreenPos);
        Vector3 endDragWorldPos = Camera.main.ScreenToWorldPoint(dragScreenPosition);

        startDragWorldPos.y = 0;
        endDragWorldPos.y = 0;

        OnDragEndAction?.Invoke(startDragWorldPos, endDragWorldPos);

        

    }



    IEnumerator DragCorotuine()
    {
        while (isDrag) {
            Vector3 startDragWorldPos = Camera.main.ScreenToWorldPoint(startDragScreenPos);
            Vector3 endDragWorldPos   = Camera.main.ScreenToWorldPoint(dragScreenPosition);
           
            startDragWorldPos.y = 0;
            endDragWorldPos.y   = 0;
            OnDrag?.Invoke(startDragWorldPos, endDragWorldPos);

            yield return null;
        }
    }

  



}
