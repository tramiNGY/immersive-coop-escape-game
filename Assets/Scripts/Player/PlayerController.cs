using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Mirror;


public class PlayerController : NetworkBehaviour
{
    public enum PlayerRole {Player1, Player2};
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSensitivity = 100f;
    private Vector2 _lookInput;
    private float _xPitch = 0f;
    private float _yYaw = 0f;
    [SyncVar] public PlayerRole role;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        HandleLook();

    }

    // Mirror Callbacks
    public override void OnStartLocalPlayer()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("current scene: " + currentScene);

        if (currentScene == "Room1_Sea")
        {
            if (isServer)
            {
                Debug.Log("This is the host Player1");
                role = PlayerRole.Player1;
            }

            else
            {
                Debug.Log("This is the client Player2");
                role = PlayerRole.Player2;
            }
        }
    }

    // Input System Events
    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();

    }

    // Input Logic
    private void HandleLook()
    {
        float deltaX = _lookInput.x * mouseSensitivity * Time.deltaTime;
        float deltaY = _lookInput.y * mouseSensitivity * Time.deltaTime;

        _xPitch -= deltaY;
        _xPitch = Mathf.Clamp(_xPitch, -80f, 80f);
        _yYaw += deltaX;
        _yYaw = Mathf.Clamp(_yYaw, -60f, 60f);

        playerCamera.localRotation = Quaternion.Euler(_xPitch, _yYaw, 0f);
    }
}
