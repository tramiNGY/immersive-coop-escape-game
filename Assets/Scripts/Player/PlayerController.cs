using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;


public class PlayerController : NetworkBehaviour
{
    // Action Move
    [SerializeField] private float _moveSpeed = 2f;
    private Vector2 _moveInput;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _rotationSpeed = 360f;

    // Action Look
    [SerializeField] private Camera _playerCamera; // HeadCamera of Local player
    [SerializeField] private Transform _headTarget; // Multi-aim constraint playerCamera following headTarget
    [SerializeField] private float _headTargetDistance = 2f; // distance of headTarget in front of playerCamera
    [SerializeField] private float _mouseSensitivity = 5f; // speed move for head camera
    private Vector2 _mouseDelta; // Mouse movement
    private float _xPitch = 0f; // Vertical rotation around x axis
    private float _yYaw = 0f; // Horizontal rotation around y axis

    // Actions HoldLeftArm and HoldRightArm
    private bool _isLeftArmActive = false;
    private bool _isRightArmActive = false;
    [SerializeField] private Transform _bodyTransform; // player's body reference
    [SerializeField] private Transform _leftArmIKTarget; // Two Bone IK constraint target which the arm follows
    [SerializeField] private Transform _rightArmIKTarget;
    [SerializeField] private float _armDistance = 1.5f; // arm's base distance from torso
    private Vector3 _leftArmPosition = Vector3.zero;
    private Vector3 _rightArmPosition = Vector3.zero;
    [SerializeField] private float _armMouseSensitibity = 0.5f; // speed move for arms

    // Action Grab
    private bool _isGrabbed = false;
    [SerializeField] private HandGrabDetector _leftHandGrabDetector;
    [SerializeField] private HandGrabDetector _rightHandGrabDetector;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None; // mouse pointer locked to center of view constrained in window, invisible
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (_isLeftArmActive) // Left mouse click hold -> Move Left Arm
        {
            HandleLeftArmPerformed();
        }

        else if (_isRightArmActive) // Right mouse click hold -> Move Right Arm
        {
            HandleRightArmPerformed();
        }

        else // No mouse click hold -> Move HeadTarget -> Head Camera follows
        {
            HandleLook();
            _headTarget.position = _playerCamera.transform.position + _playerCamera.transform.forward * _headTargetDistance; // Places headTarget position in front of the new playerCamera direction
            _headTarget.rotation = _playerCamera.transform.rotation;
        }
    }

    void FixedUpdate()
    {
        HandleMove(); // FixedUpdate called in sync with physics, avoid updating Rigidbody in Update() unstable
    }

    // Mirror Callbacks
    public override void OnStartLocalPlayer()
    {
        DontDestroyOnLoad(gameObject); // Make Player persistant when changing scenes

        _playerCamera.enabled = true; // Only enable localPlayer Camera per client
        _playerCamera.GetComponent<AudioListener>().enabled = true;

        if (!isLocalPlayer)
        {
            _playerCamera.enabled = false; // Disable other client's camera
            _playerCamera.GetComponent<AudioListener>().enabled = false; // Cannot have 2 active audio listeners in the scene

        }
    }

    // Input System Events
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        Debug.Log("Move input .x: " + _moveInput.x + "Move input .y: " + _moveInput.y);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnHoldLeftArm(InputAction.CallbackContext context)
    {
        if (context.started) // Mouse button clicked
        {
            _isLeftArmActive = true;
            HandleLeftArmStart();
        }

        if (context.performed) // Mouse button held long enough
        {
            _isLeftArmActive = true;
        }

        if (context.canceled) // Mouse button unpressed
        {
            _isLeftArmActive = false;
            HandleLeftArmCanceled();
        }

    }

    public void OnHoldRightArm(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isRightArmActive = true;
            HandleRightArmStart();
        }

        else if (context.performed)
        {
            _isRightArmActive = true;
        }

        else if (context.canceled)
        {
            _isRightArmActive = false;
            HandleRightArmCanceled();
        }

    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HandleGrab();
        }
    }

    // Input Logic
    private void HandleMove()
    {
        Vector3 localDirection = new Vector3(_moveInput.x, 0, _moveInput.y); // .x Horizontal (left/right), .y Vertical (up/down), .z Depth (forward/backward)

        if (localDirection.sqrMagnitude > 0f)
        {
            Vector3 direction = localDirection.normalized; // normalize vector to 1 to keep consistent speed in all directions (not faster in diagonal)
            Vector3 worldDirection = transform.TransformDirection(direction); // converts local direction to world space (based on player orientation)
            Debug.Log($"[Move] Input: {_moveInput}, MoveWorld: {worldDirection}");

            // Use Rigidbody movement  and rotation to respect physics and collisions (avoid transform.position/.rotation which skips physics)
            // Movement
            Vector3 newPosition = _rigidbody.position + worldDirection * _moveSpeed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(newPosition);

            // Rotation
            Quaternion targetRotation = Quaternion.LookRotation(worldDirection);
            Quaternion newRotation = Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
            _rigidbody.MoveRotation(newRotation);
        }
    }

    private void HandleLook()
    {
        float deltaX = _mouseDelta.x * _mouseSensitivity * Time.deltaTime; // deltaTime to make movement independant from frame rate FPS
        float deltaY = _mouseDelta.y * _mouseSensitivity * Time.deltaTime;

        _xPitch -= deltaY;
        _xPitch = Mathf.Clamp(_xPitch, -30f, 80f); // Limit Head camera realistic rotation
        _yYaw += deltaX;
        _yYaw = Mathf.Clamp(_yYaw, -60f, 60f);

        _playerCamera.transform.localRotation = Quaternion.Euler(_xPitch, _yYaw, 0f); // Camera local rotation from euler's angles
    }

    private void HandleLeftArmStart()
    {

    }

    private void HandleLeftArmPerformed()
    {
        Vector3 leftArmDelta = new Vector3(_mouseDelta.x * _armMouseSensitibity, _mouseDelta.y * _armMouseSensitibity, 0f); // Mouse movement

        _leftArmPosition -= _bodyTransform.right * leftArmDelta.x; // Arm's position based on mouse movement
        _leftArmPosition.x = Mathf.Clamp(_leftArmPosition.x, -1.5f, 1f); // Limit arm's position left/right
        _leftArmPosition += _bodyTransform.up * leftArmDelta.y;
        _leftArmPosition.y = Mathf.Clamp(_leftArmPosition.y, -1f, 1.5f); // Limit arm's position down/up
        _leftArmPosition.z = Mathf.Clamp(_leftArmPosition.z, 0f, 1f); // Limit arm's position backward/forward

        Vector3 baseLeftArmPosition = _bodyTransform.position - _bodyTransform.forward * _armDistance; // Base arm's position when click in front of torso

        Vector3 leftTargetPosition = _leftArmPosition + baseLeftArmPosition;

        _leftArmIKTarget.position = Vector3.Lerp(_leftArmIKTarget.position, leftTargetPosition, 0.4f * Time.deltaTime); // Lerp Linear interpolation between 2 frames arm's position to smooth movement
    }

    private void HandleLeftArmCanceled()
    {

    }

    private void HandleRightArmStart()
    {

    }

    private void HandleRightArmPerformed()
    {
        Vector3 rightArmDelta = new Vector3(_mouseDelta.x * _armMouseSensitibity, _mouseDelta.y *_armMouseSensitibity, 0f);

        _rightArmPosition -= _bodyTransform.right * rightArmDelta.x;
        _rightArmPosition.x = Mathf.Clamp(_rightArmPosition.x, -1.5f, 1f);
        _rightArmPosition += _bodyTransform.up * rightArmDelta.y;
        _rightArmPosition.y = Mathf.Clamp(_rightArmPosition.y, -1f, 1.5f);
        _rightArmPosition.z = Mathf.Clamp(_leftArmPosition.z, 0f, 1f);

        Vector3 baseRightArmPosition = _bodyTransform.position - _bodyTransform.forward * _armDistance;

        Vector3 rightTargetPosition = _rightArmPosition + baseRightArmPosition;

        _rightArmIKTarget.position = Vector3.Lerp(_rightArmIKTarget.position, rightTargetPosition, 0.4f * Time.deltaTime);

    }

    private void HandleRightArmCanceled()
    {

    }

    private void HandleGrab()
    {
        Debug.Log("Grab action triggered");
        if (!_isGrabbed)
            StartGrab();
        else
            StopGrab();
    }

    private void StartGrab()
    {
        HandGrabDetector activeHandGrab;
        if (_isLeftArmActive)
        {
            activeHandGrab = _leftHandGrabDetector;
        }
        else
        {
            activeHandGrab = _rightHandGrabDetector;
        }

        GameObject objectToGrab = activeHandGrab.currentGrabableObject; // Fetch grabbable object detected
        if (objectToGrab != null) // if null then the object is not grabbable
        {
            GrabbableObject grabbable = objectToGrab.GetComponent<GrabbableObject>();
            if (grabbable != null && grabbable.grabPoint != null)
            {
                activeHandGrab.transform.position = grabbable.grabPoint.position; // Put hand to grabPoint position
                activeHandGrab.transform.rotation = grabbable.grabPoint.rotation;
            }

            objectToGrab.transform.SetParent(activeHandGrab.transform); // Parent hand moves -> object moves

            Rigidbody rb = objectToGrab.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = true; // prevents object from falling with gravity when hold by hand
            _isGrabbed = true;
            Debug.Log("Started grabbing: " + objectToGrab.name);
        }

    }

    private void StopGrab()
    {
        HandGrabDetector activeHandGrab;
        if (_isLeftArmActive)
            activeHandGrab = _leftHandGrabDetector;
        else
            activeHandGrab = _rightHandGrabDetector;

        GameObject objectToGrab = activeHandGrab.currentGrabableObject;
        if (objectToGrab != null)
        {
            objectToGrab.transform.SetParent(null);
            Rigidbody rb = objectToGrab.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            activeHandGrab.currentGrabableObject = null;
            _isGrabbed = false;
        }
    }
}
