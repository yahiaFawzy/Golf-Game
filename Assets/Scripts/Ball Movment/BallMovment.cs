using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovment : MonoBehaviour
{
    Rigidbody _rigidbody;
    [SerializeField] InputArea _inputArea;
    [SerializeField] Transform debugCube;

    [SerializeField] GameObject targetCube;

    private Vector3 prevuiosPoint;
    private float   distanceToTarget;


    [Header("anguler drag")]
    [SerializeField] float maxDragValue = 20;
    [SerializeField] float smoothTime = 3;
    [SerializeField] float anguleDrag;

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;
         Physics.gravity = new Vector3(0, -25, 0);
        _rigidbody = GetComponent<Rigidbody>();
        _inputArea.OnRealseBall += OnRealseBall;
    }

    private void OnRealseBall(Vector3 targetPoint)
    {
        prevuiosPoint = transform.position;
        distanceToTarget = Vector2.Distance(transform.position, targetPoint);
        targetCube.transform.position = targetPoint; 
        _rigidbody.velocity = ProjectilePath.CalculateProjectileVelocity(transform.position, targetPoint, (int)RocetManager.Instance.rocet);
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, prevuiosPoint) > distanceToTarget)
        {
            _rigidbody.angularDrag = Mathf.Lerp(_rigidbody.angularDrag, maxDragValue, 0.0125f*smoothTime);
        }
        else
        {
            _rigidbody.angularDrag = 0;
        }


        if (_rigidbody.velocity.magnitude < 1 && _rigidbody.angularVelocity.magnitude < 1)
        {
            ActiveInputArea();
        }
        else
        {
            DeactiveInputArea();
        }

        anguleDrag = _rigidbody.angularDrag;
    }



    void ActiveInputArea() {
        if (!_inputArea.gameObject.activeInHierarchy)
        {
            _inputArea.gameObject.SetActive(true);
            _inputArea.transform.position = transform.position;
        }
    }

    void DeactiveInputArea() {
        if (_inputArea.gameObject.activeInHierarchy)
        {
            _inputArea.gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "fallingArea")
        {
            _rigidbody.position = prevuiosPoint;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            if (!_inputArea.gameObject.activeInHierarchy)
            {
                _inputArea.gameObject.SetActive(true);
                _inputArea.transform.position = prevuiosPoint;
            }
        }
    }
}
