using Unity.Mathematics;
using UnityEngine;

public class TitleSreenPlayerPreviewRotator : MonoBehaviour
{
    PlayerControls2 playerControls;

    [Header("Camera Input")]
    [SerializeField] private Vector2 cameraInput;
    [SerializeField] private float lookAngle;

    [Header("Rotation")]
    [SerializeField] private float horizontalInput;
    [SerializeField]private float rotationSpeed;

    private void Start()
    {
        transform.position =  WorldSaveGameManager.instance.player.defaultPlayerposition;
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
           
            
            playerControls = new PlayerControls2();
            playerControls.PlayerCamera.Movement.performed+=i => cameraInput =i.ReadValue<Vector2>();
        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        horizontalInput =cameraInput.x;

        lookAngle += (horizontalInput *rotationSpeed)*Time.deltaTime;
        Vector3 cameraRotation = Vector3.zero;
        cameraRotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;
        
    }

    

}
