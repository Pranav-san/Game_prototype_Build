using UnityEngine;

public class AICharacterInventoryManager : CharacterInventoryManager
{
    [Header("Projectiles")]
    public RangedProjectileItem mainProjectile;
    public RangedProjectileItem secondaryProjectile;
    public bool hasArrowNotched = false;
    public bool isHoldingArrow = false;
    public bool isAiming = false;
    public bool FireBullet = false;


    AICharacterManager aiCharacterManager;
    [Header("Loot Chance")]
    public int dropChance = 10;
    [SerializeField] Item[] droppableItems;


    protected override void Awake()
    {
        base.Awake();


        aiCharacterManager = GetComponent<AICharacterManager>();


    }
    public void DropItem()
    {

        //The Status Of If This Character Will Drop Item
        bool willDropItem = true;

        //Random Number Rolled From 0-100
        int itemChanceRoll = Random.Range(0, dropChance);

        //If The Number is Equal to or Lower Than The Item Drop Chance, We Pass The Check And Drop The Item
        if (itemChanceRoll <= dropChance)
            willDropItem = true;

        if (!willDropItem)
            return;

        Item generatedItem = droppableItems[Random.Range(0, droppableItems.Length)];

        if (generatedItem == null)
            return;

        GameObject itemPickUpInteractableGameObject = Instantiate(WorldItemDatabase.instance.pickUpItemPrefab);
        PickUpItemInteractable pickUpItemInteractable = itemPickUpInteractableGameObject.GetComponent<PickUpItemInteractable>();

        pickUpItemInteractable.itemID = generatedItem.itemID;
        pickUpItemInteractable.item = generatedItem;

        pickUpItemInteractable.SetItemPosition(transform.position);

    }



}
