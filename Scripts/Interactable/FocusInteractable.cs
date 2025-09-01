using UnityEngine;

public class FocusInteractable : Interactable
{


    public override void Interact(playerManager player)
    {
        if (!PlayerCamera.instance.isInFocusMode)
        {
            PlayerCamera.instance.snapCameraToFocusPoint(focusPoint);

        }
        else
        {
            PlayerCamera.instance.ResetCameraFromFocus();
        }
        



        
    }

}
