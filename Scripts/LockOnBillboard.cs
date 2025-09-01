using UnityEngine;

public class LockOnBillboard : UI_StatBar
{
    [Header("Target")]
    public Transform targetBone; // Assign hips or chest bone

    [Header("Camera")]
    public Camera uiCamera; // Secondary camera that renders only UI

    [Header("Size")]
    public float fixedSize = 0.25f; // Adjust until it looks right

    public override void Start()
    {
        base.Start();

        uiCamera = PlayerCamera.instance.cameraObject;
    }

    void LateUpdate()
    {
        if (!targetBone || !uiCamera)
            return;

        // Position UI at the target's bone
        transform.position = targetBone.position;

        // Always face the camera
        transform.rotation = uiCamera.transform.rotation;

        // Keep constant size (no scaling with distance)
        transform.localScale = Vector3.one * fixedSize;
    }


}