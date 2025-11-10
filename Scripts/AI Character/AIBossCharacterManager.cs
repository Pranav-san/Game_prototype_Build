using NUnit.Framework;
using UnityEngine;
using UnityEngine.TextCore.Text;
using System.Collections.Generic;
using System.Collections;

public class AIBossCharacterManager : AICharacterManager
{
    public int bossID = 1;


    [Header("Status")]
    [SerializeField] bool hasBeenDefeated = false;
    [SerializeField] bool hasBeenAwakened = false;
    [SerializeField] string sleepAnimation;
    [SerializeField] string awakenedAnimation;
    public bool BossFightIsActive = false;

    [Header("Fog Walls")]
    [SerializeField] int fogWallID = 0;
    [SerializeField] List<FogWallInteractable> fogWalls;

    [Header("States")]
    [SerializeField] BossSleepState sleepState;

    [Header("Phase Shift")]
    [SerializeField] string phaseShiftAnimation;
    [SerializeField] CombatStanceState phase02CombatStanceState;

    [Header("Test")]
    [SerializeField] bool defeatedBossDebug = false;


    protected override void Awake()
    {
        base.Awake();

        sleepState = Instantiate(sleepState);
        currentState = sleepState;
    }

    private void Start()
    {
        //IF our Save Data Does Not Contain Info On This boss, Add It Now 

        if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
        {
            WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add (bossID, false);
            WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add (bossID, false);

        }
        //Otherwise Load the Data That Already Exists
        else
        {
            hasBeenDefeated = WorldSaveGameManager.instance.currentCharacterData.bossesDefeated[bossID];
            hasBeenAwakened = WorldSaveGameManager.instance.currentCharacterData.bossesAwakened[bossID];

            

        }



        //Locate FogWall
        fogWalls = new List<FogWallInteractable>();

        //Method 1
        //Use Boss ID to Fetch Fog Wall
        foreach(var fogwall in WorldObjectManager.instance.fogWalls)
        {
            if(fogwall.fogWallID == bossID)
            {
                fogWalls.Add(fogwall);
            }
        }

        //Method 2 Fog Wall ID
        //Use Fog Wall id Instead of Boss ID


        //foreach (var fogwall in WorldObjectManager.instance.fogWalls)
        //{
        //    if (fogwall.fogWallID == fogWallID)
        //    {
        //        fogWalls.Add(fogwall);
        //    }
        //}



        
        // If Boss Has Been Awakened Enable The Fog Walls
        
        if (hasBeenAwakened)
        {
            for(int i = 0; i< fogWalls.Count; i++)
            {
                fogWalls[i].SetActive(true);
            }
        }


        //If Boss Has Been Defeated Disable The Fog Walls
        if (hasBeenDefeated)
        {
            for (int i = 0; i< fogWalls.Count; i++)
            {
                fogWalls[i].SetActive(false);
            }
            gameObject.SetActive(false);


        }



    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
       
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        characterStatsManager.currentHealth =0;
        characterStatsManager.isDead = true;

        PlayerUIManager.instance.playerUIPopUPManager.SendBossDefeatedPopUp("Victory Achieved");


        if (!manuallySelectDeathAnimation)
        {
            characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);

            playerManager killer = characterStatsManager.lastAttacker as playerManager;


            PlayerInputManager.Instance.player.playerCombatManager.isLockedOn = false;
            PlayerInputManager.Instance.player.playerCombatManager.currentTarget = null;
            //PlayerCamera.instance.SetLockCameraHeight();

            if (killer != null)
            {
                aiCharacterCombatManager.AwardRunesOnDeath(killer);
            }


            PlayerUIManager.instance.SetLockOnTarget(null);

            hasBeenDefeated = true;
            
            //Set BossFightIsActive Bool to False, And Destroy Boss HP Bars
            OnBossFightIsActiveChanged(true, false);

            //Disable Fog Walls
            for (int i = 0; i< fogWalls.Count; i++)
            {
                fogWalls[i].SetActive(false);
            }

            //If Our Save Data Does Not Contain Details On this Boss, Add It now
            if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {

                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);

            }
            //OtherWise, Load The Data That Already Exists On This boss
            else
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);



            }




            WorldSaveGameManager.instance.SaveGame();


        }

        yield return new WaitForSeconds(5);

    }

    public void Wakeboss()
    {
        if(hasBeenDefeated)
            return;

        if (!hasBeenAwakened)
        {
            characterAnimatorManager.PlayTargetActionAnimation(awakenedAnimation, true);

        }

        hasBeenAwakened = true ;
        currentState = idle;
        

        if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
        {
            WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID , true ) ;
        }
        else
        {
            WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
            WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID,true);
        }

        for (int i = 0;i< fogWalls.Count;i++)
        {
            fogWalls [i].SetActive(true);
        }

        //Set BossFightIsActive Bool to True, And Enable Boss HP Bars
        OnBossFightIsActiveChanged(false,true);
      

       
    }

    public void OnBossFightIsActiveChanged(bool oldStats, bool newStats)
    {
        //Create A HP Bar For Boss fight If Its Active 

        if (newStats)
        {
            if (BossFightIsActive)
            return;

            BossFightIsActive = true;


            GameObject bossHealthBar = Instantiate(PlayerUIManager.instance.playerUIHUDManager.BossHealthBar, PlayerUIManager.instance.playerUIHUDManager.BossHealthBarParent);

            UI_Boss_HP_Bar boss_HP_Bar = bossHealthBar.GetComponent<UI_Boss_HP_Bar>();
            boss_HP_Bar.EnableBossHpBar(this);
            PlayerUIManager.instance.playerUIHUDManager.currentBossHealthBar = boss_HP_Bar;
            LockOnBillboard LockOnUI = GetComponentInChildren<LockOnBillboard>(true);

            





            

            characterStatsManager.uI_Character_HP_Bar = boss_HP_Bar;
            characterStatsManager.AIHealthUI = boss_HP_Bar;

        }
        else
        {
            if(!BossFightIsActive)
                return;

            BossFightIsActive = false;

            if (characterStatsManager.uI_Character_HP_Bar is UI_Boss_HP_Bar boss_HP_Bar)
            {
                boss_HP_Bar.RemoveHPBar(1.5f); // Remove instantly
                characterStatsManager.uI_Character_HP_Bar = null;
            }
        }



            
        
       

      



        

        


    }


    protected void PhaseShift()
    {
        characterAnimatorManager.PlayTargetActionAnimation(phaseShiftAnimation, true);
        combatStance = Instantiate(phase02CombatStanceState);
        currentState = combatStance;
    }

    

}
