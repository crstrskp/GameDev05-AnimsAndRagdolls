using UnityEngine;

public class WowCamera : MonoBehaviour
{
    public Transform Target { get; set; }

    [SerializeField] private float targetHeight = 1.7f;
    [SerializeField] private float distance = 5.0f;

    [SerializeField] private float maxDistance = 20;
    [SerializeField] private float minDistance = .6f;
    [SerializeField] private float speedDistance = 5;

    [SerializeField] private float xSpeed = 200.0f;
    [SerializeField] private float ySpeed = 200.0f;

    [SerializeField] private int yMinLimit = -40;
    [SerializeField] private int yMaxLimit = 80;

    [SerializeField] private int zoomRate = 40;

    [SerializeField] private float rotationDampening = 3.0f;
    [SerializeField] private float zoomDampening = 5.0f;

    [SerializeField] private LayerMask collisionLayers = -1;

    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private float correctedDistance;
    private bool m_rightClicking;
    private bool m_freeLookMode;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        Target = transform.parent;
        Vector3 angles = transform.eulerAngles;
        xDeg = angles.x;
        yDeg = angles.y;

        currentDistance = distance;
        desiredDistance = distance;
        correctedDistance = distance;

        m_rightClicking = true;

        // Make the rigid body not change rotation
        if (this.gameObject.GetComponent<Rigidbody>())
            this.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
    }

    private void Update()
    {
        m_freeLookMode = Input.GetKey(KeyCode.LeftAlt);
    }

    /**
     * Camera logic on LateUpdate to only update after all character movement logic has been handled.
     */
    void LateUpdate()
    {
        Vector3 vTargetOffset;

        if (!Target) return;

        if (GUIUtility.hotControl == 0)
        {
            // Free Look
            xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            // Center
            float targetRotationAngle = Target.eulerAngles.y;
            float currentRotationAngle = transform.eulerAngles.y;
            xDeg = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
        }

        // calculate the desired distance
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance) * speedDistance;
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);

        yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);

        // set camera rotation
        Quaternion rotation = Quaternion.Euler(yDeg, xDeg, 0);
        correctedDistance = desiredDistance;

        // calculate desired camera position
        vTargetOffset = new Vector3(0, -targetHeight, 0);
        Vector3 position = Target.position - (rotation * Vector3.forward * desiredDistance + vTargetOffset);

        // keep within legal limits
        currentDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);

        // recalculate position based on the new currentDistance
        position = Target.position - (rotation * Vector3.forward * currentDistance + vTargetOffset);

        transform.rotation = rotation;
        transform.position = position;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}