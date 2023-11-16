using UnityEngine;

class VariableSpeedFPSWalker : MonoBehaviour
{
    [SerializeField]
    private float forwardSpeed = 10;
    [SerializeField]
    private float backwardSpeed = 5;
    [SerializeField]
    private float strafeSpeed = 8;
    [SerializeField]
    private float jumpForce = 5000;
    private float m_RayDistance;
    private RaycastHit m_Hit;


    private float m_characterHeight;
    private new Rigidbody rigidbody;

    private readonly float m_mouseSensitivity = 3.0f;

    private readonly float m_upDownRange = 55.0f;
    float rotY = 0;
    float rotX = 0;
    CursorLockMode wantedMode;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
        m_characterHeight = GetComponent<Collider>().bounds.max.y - transform.position.y;


        m_RayDistance = m_characterHeight * 1.1f;


        wantedMode = CursorLockMode.Locked;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            wantedMode = CursorLockMode.None;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        FPSMouseLook();
        RigidbodyMovement();

        SetCursorState();

    }

    private void RigidbodyMovement()
    {

        // Step1: Get your input values
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // Step 2: Limit the movement on angles and then multiply those results
        // by their appropriate speed variables
        float percentofpercent = Mathf.Abs(horizontal) + Mathf.Abs(vertical) - 1;
        if (percentofpercent > 0.1f)
        {
            // if we're here, then we're not moving in a straight line
            // my math here might be kinda confusing and sloppy...so don't look!
            percentofpercent = percentofpercent * 10000;
            percentofpercent = Mathf.Sqrt(percentofpercent);
            percentofpercent = percentofpercent / 100;
            float finalMultiplier = percentofpercent * .25f;
            horizontal = horizontal - (horizontal * finalMultiplier);
            vertical = vertical - (vertical * finalMultiplier);
        }

        if (vertical > 0)
            vertical = vertical * forwardSpeed;
        else if (vertical < 0)
            vertical = vertical * backwardSpeed;

        horizontal = horizontal * strafeSpeed;

        // Step 3: Derive a vector on which to travel, based on the combined
        // influence of BOTH axes (ignoring any y movement)
        Vector3 tubeFinalVector = transform.TransformDirection(new Vector3(horizontal, rigidbody.velocity.y, vertical));

        // Step 4: Apply the final movement in world space
        rigidbody.velocity = tubeFinalVector;
    }

    private void Jump()
    {
        Physics.Raycast(transform.position, -Vector3.up, out m_Hit, m_RayDistance);
        
        if(m_Hit.transform)
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FPSMouseLook()
    {

        rotY -= Input.GetAxis("Mouse Y") * m_mouseSensitivity;
        rotX = Input.GetAxis("Mouse X") * m_mouseSensitivity;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            transform.Rotate(0, rotX, 0);

            rotY = Mathf.Clamp(rotY, -m_upDownRange, m_upDownRange);
            Camera.main.transform.localRotation = Quaternion.Euler(rotY, 0, 0);
        }
    }

    // Apply requested cursor state
    void SetCursorState()
    {
        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != wantedMode);
    }
}