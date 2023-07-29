using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [SerializeField] BallInput _ballInput;
    Rigidbody _rigidbody;
    PathRender _pathRender;
    [SerializeField] Transform debugCube;
    [SerializeField] PhysicMaterial physicMaterial;
    [SerializeField] float shootScale=3;
    Vector3 prevuiosPoint;
    Vector3 target;
    float distanceToTarget;
    public float threshold = 1;
    public float decelerationRate = 0.5f;

    void Awake()
    {
        Physics.gravity = new Vector3(0,-25,0); 

        _rigidbody = GetComponent<Rigidbody>();
        _pathRender = GetComponent<PathRender>();

        _ballInput.OnDragAction += OnDrag;
        _ballInput.OnEndDragAction += OnDragEnd;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, prevuiosPoint) > distanceToTarget)
        {            
            _rigidbody.angularDrag = Mathf.Lerp(_rigidbody.angularDrag,20,0.0125f*3);
        }
        else {
            _rigidbody.angularDrag = 0;
        }

        if (_rigidbody.velocity.magnitude < 1 && _rigidbody.angularVelocity.magnitude < 1)
        {
            if (!_ballInput.gameObject.activeInHierarchy) {
                _ballInput.gameObject.SetActive(true);
            } 
        }
        else {
            if (_ballInput.gameObject.activeInHierarchy)
            {
              _ballInput.gameObject.SetActive(false);        
            }
        }

    }


    private void OnDragEnd(Vector3 dragPoint)
    {
        prevuiosPoint = transform.position;

        Vector3 pos = transform.position;
        pos.y = dragPoint.y;
        target = (pos - dragPoint) * shootScale + pos;
        RaycastHit hit;
        if (Physics.Raycast(target + Vector3.up * 5, Vector3.down, out hit, 15))
        {

            target.y = hit.point.y;

        }
        distanceToTarget = Vector2.Distance(transform.position,target);

        var vel = ProjectilePath.CalculateProjectileVelocity(transform.position, target, (int)RocetManager.Instance.rocet);

        _rigidbody.velocity = vel;

        _pathRender.HidePath();
    }


    private void OnDrag(Vector3 dragPoint)
    {
        Vector3 position = transform.position;
        target = (position - dragPoint) * shootScale + position;

        RaycastHit hit;
        if (Physics.Raycast(target+Vector3.up*5,Vector3.down, out hit,15))
        {

            target.y = hit.point.y;
           
        }

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
