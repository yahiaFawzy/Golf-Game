using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float rotateSpeed = 1f;
    public float zoomSpeed = 1f;
    public float panSpeed = 1f;
    private float distance;
    private float currentAngle = 0;

    private InputAction rotateAction;
    private InputAction zoomAction;
    private InputAction panAction;

    void Awake()
    {
        rotateAction = new InputAction(type: InputActionType.Value, binding: "<Pointer>/delta");
        zoomAction = new InputAction(type: InputActionType.Value, binding: "<Mouse>/scroll/y");
        panAction = new InputAction(type: InputActionType.Value, binding: "<Pointer>/delta");
    }

    private void OnEnable()
    {
        rotateAction.Enable();
        zoomAction.Enable();
        panAction.Enable();
    }

    private void OnDisable()
    {
        rotateAction.Disable();
        zoomAction.Disable();
        panAction.Disable();
    }

    void Start()
    {
        distance = (new Vector3(transform.position.x, 0, transform.position.z)).magnitude;
    }

    void Update()
    {
        // Rotate
        Vector2 rotateInput = rotateAction.ReadValue<Vector2>();
        currentAngle += rotateInput.x * rotateSpeed * Time.deltaTime;
        transform.position = new Vector3(Mathf.Sin(currentAngle) * distance, transform.position.y, Mathf.Cos(currentAngle) * distance);
        transform.LookAt(target);

        // Zoom
        float zoomInput = zoomAction.ReadValue<float>();
        distance -= zoomInput * zoomSpeed * Time.deltaTime;

        // Pan
        Vector2 panInput = panAction.ReadValue<Vector2>();
        transform.position += transform.right * panInput.x * panSpeed * Time.deltaTime;
        transform.position += transform.up * panInput.y * panSpeed * Time.deltaTime;
    }
}
