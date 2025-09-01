using UnityEngine;

[CreateAssetMenu(menuName = "Items/Key Item")]
public class KeyItem : Item
{
    [Header("key ID ")]
    public int keyID = 0;//Must Match DoorKeyID 
    
}
