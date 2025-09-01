using UnityEngine;

public class EventTriggerBossFight : MonoBehaviour
{
    [SerializeField] int bossID;
    [SerializeField] int[] fogWallIDs;
    private void OnTriggerEnter(Collider other)
    {
        AIBossCharacterManager boss = WorldAIManager.instance.GetBossByID(bossID);

        if (boss != null)
        {
            boss.Wakeboss();

            foreach (int id in fogWallIDs)
            {
                var fogWall = WorldObjectManager.instance.fogWalls.Find(w => w.fogWallID == id);
                if (fogWall != null) fogWall.SetActive(true);
            }
        }
        
    }
}
