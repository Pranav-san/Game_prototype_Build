using UnityEngine;

public class PlayerManager : CharacterManager
{
    public static PlayerManager instance;

    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;

    protected override void Awake()
    {
        base.Awake();

        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }



        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }
    protected override void Update()
    {
        base.Update();
        playerLocomotionManager.HandleAllMovement();


    }
}
