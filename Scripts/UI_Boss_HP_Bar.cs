using UnityEngine;
using TMPro;

public class UI_Boss_HP_Bar : UI_StatBar, AIHealthUI
{
    [SerializeField] AIBossCharacterManager bossCharacter;
   public void EnableBossHpBar(AIBossCharacterManager aIBoss)
    {

        bossCharacter = aIBoss;
        //bossCharacter.characterStatsManager.currentHealth += OnBossHpChanged;
        SetMaxStat(aIBoss.characterStatsManager.maxHealth);
        SetStat(aIBoss.characterStatsManager.currentHealth);
       
        GetComponentInChildren<TextMeshProUGUI>().text = bossCharacter.characterName;
        


    }

    public void OnHealthChanged(int oldValue, int newValue)
    {
        SetStat(newValue);

        if (newValue <= 0)
        {
            RemoveHPBar(2.5f);
        }
    }

    public void RemoveHPBar(float timer)
    {
        Destroy(gameObject, timer);
    }
}
