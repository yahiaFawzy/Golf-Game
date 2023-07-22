using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [SerializeField] int MaxHeigh = 10;
    BallInput _ballInput;
    Rigidbody _rigidbody;
    PathRender _pathRender;
    [SerializeField] Transform debugCube;
    [SerializeField] PhysicMaterial physicMaterial;

    Vector3 prevuiosPoint;
    Vector3 target;
    float distanceToTarget;
    public float threshold = 1;
    public float decelerationRate = 0.5f;

    void Awake()
    {
        Physics.gravity = new Vector3(0,-18f,0); 

        _ballInput = GetComponent<BallInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _pathRender = GetComponent<PathRender>();

        _ballInput.OnDragAction += OnDrag;
        _ballInput.OnEndDragAction += OnDragEnd;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, prevuiosPoint) > distanceToTarget)
        {            
            _rigidbody.angularDrag = Mathf.Lerp(_rigidbody.angularDrag,20,0.0125f);
        }
        else {
            _rigidbody.angularDrag = 0;
        }
    }


    private void OnDragEnd(Vector3 dragPoint)
    {
        prevuiosPoint = transform.position;
      
        target = (transform.position - dragPoint) * 2 + transform.position;

        distanceToTarget = Vector2.Distance(transform.position,target);

        var vel = ProjectilePath.CalculateProjectileVelocity(transform.position, target, (int)RocetManager.Instance.rocet);

        _rigidbody.velocity = vel;

        _pathRender.HidePath(transform.position);
    }


    private void OnDrag(Vector3 dragPoint)
    {
        target = (transform.position - dragPoint) * 2 +transform.position;
        debugCube.position = target;
        _pathRender.DrawProjectilePath(transform.position, target, (int)RocetManager.Instance.rocet);
    }

 

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "targetPoint")
        {
         //   _rigidbody.velocity = Vector3.zero;
        }
        else if(other.tag == "fallingArea") {
            _rigidbody.position = prevuiosPoint;
            _rigidbody.velocity = Vector3.zero;
        }
    }


}
