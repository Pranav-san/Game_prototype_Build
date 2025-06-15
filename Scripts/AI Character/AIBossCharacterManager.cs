using UnityEngine;

public class AIBossCharacterManager : AICharacterManager
{
    public int bossID = 1;
    [SerializeField] bool hasBeenDefeated = false;

    [Header("Test")]
    [SerializeField] bool defeatedBossDebug = false;



    protected override void Update()
    {
        base.Update();

        if (defeatedBossDebug)
        {
            defeatedBossDebug = false ;
            hasBeenDefeated= true ;

            if(WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true ) ;
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true ) ;

            }
            else
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
            }

            WorldSaveGameManager.instance.SaveGame();
        }


    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
       
    }

    

}
