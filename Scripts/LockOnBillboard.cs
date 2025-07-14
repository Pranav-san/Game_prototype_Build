using UnityEngine;

public class LockOnBillboard : UI_StatBar
{
    

 

    private void Update()
    {
        if (lockOnUI != null)
        {
            lockOnUI.transform.forward = Camera.main.transform.forward;
            

            Debug.Log("BillBoarding Lock on UI");
        }

    }
}
