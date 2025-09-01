using UnityEngine;


[System.Serializable]
public class SerializableWeapon : ISerializationCallbackReceiver
{
    [SerializeField] public int itemID;


    public WeaponItem GetWeapon()
    {
        WeaponItem weapon = WorldItemDatabase.instance.GetWeaponFromSerialzedData(this);
        return weapon;
    }
    public void OnAfterDeserialize()
    {
       
    }

    public void OnBeforeSerialize()
    {
        
    }
}
