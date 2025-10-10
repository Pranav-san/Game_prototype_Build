using UnityEngine;

public class LadderInteractable : Interactable
{
    [Header("Ladder Settings")]
    public Transform ladderTopPoint;
    public Transform ladderTopStartPoint;
    public Transform ladderBottomPoint;
    public float climbSpeed = 3f;
    public bool isTopPoint = false;
    private Vector3 ladderAxis;
    private float ladderLength;
    private float ladderProgress;


    public override void Interact(playerManager player)
    {

        // Enable climbing mode
        player.playerLocomotionManager.EnterLadder(this);
       
    }

    public override void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<playerManager>();
        if (player == null) return;

        if (!player.playerLocomotionManager.isClimbingLadder)
        {
            // this is entry
            base.OnTriggerEnter(other);
        }
        else
        {
            // this must be exit 
        }


    }

    //private void OnDrawGizmos()
    //{
    //    if (ladderTopPoint != null && ladderBottomPoint != null)
    //    {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawLine(ladderBottomPoint.position, ladderTopPoint.position);
    //        Gizmos.DrawSphere(ladderTopPoint.position, 0.2f);
    //        Gizmos.DrawSphere(ladderBottomPoint.position, 0.2f);
    //    }
    //}


}
