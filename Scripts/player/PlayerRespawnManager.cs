using UnityEngine;

public class PlayerRespawnManager : MonoBehaviour
{
    private Transform lastGraceTransform;


    public void SetLastGraceTransform(Transform graceTransform)
    {
        lastGraceTransform = graceTransform;
    }

    public void RespawnPlayer(playerManager player)
    {
        
        if (lastGraceTransform == null)
        {
            Debug.LogWarning("No Site of Grace to respawn at, Respwaning At sitesOfGrace[0] ");


            WorldObjectManager.instance.sitesOfGrace[0].TeleportToSiteOfGrace();
            player.playerStatsManager.isDead = false;
            player.playerStatsManager.currentHealth = player.playerStatsManager.maxHealth;
            player.playerStatsManager.currentStamina = player.playerStatsManager.maxStamina;

            PlayerUIManager.instance.UpdateHealthBar(player.playerStatsManager.currentHealth);
            PlayerUIManager.instance.UpdateStaminaBar(Mathf.RoundToInt(player.playerStatsManager.currentStamina));
            WorldAIManager.instance.ResetAllCharacters();

            player.playerAnimatorManager.PlayTargetActionAnimation("Respwan", false);
            return;
        }
        else
        {
            player.playerStatsManager.isDead = false;
            player.transform.position = lastGraceTransform.position;
            player.transform.rotation = lastGraceTransform.rotation;

            player.playerStatsManager.currentHealth = player.playerStatsManager.maxHealth;
            player.playerStatsManager.currentStamina = player.playerStatsManager.maxStamina;

            PlayerUIManager.instance.UpdateHealthBar(player.playerStatsManager.currentHealth);
            PlayerUIManager.instance.UpdateStaminaBar(Mathf.RoundToInt(player.playerStatsManager.currentStamina));
            WorldAIManager.instance.ResetAllCharacters();

            player.playerAnimatorManager.PlayTargetActionAnimation("Respwan", false);

        }

        PlayerUIManager.instance.playerUILoadingScreenManager.DeactivateLoadingScreen();
        PlayerUIManager.instance.mobileControls.EnableMobileControls();





    }
}


