using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Character_HP_Bar : UI_StatBar
{
    private CharacterManager character;
    private AICharacterManager aiCharacter;

    [SerializeField] bool displayCharacterNameOnDamage = false;
    [SerializeField] float defaultTimeBeforeBarHides = 3f;
    private float hideTimer = 0;
    [SerializeField] TextMeshProUGUI characterName;
    [SerializeField] TextMeshProUGUI characterDamage;

    protected override void Awake()
    {
        base.Awake();
        character = GetComponentInParent<CharacterManager>();

        if (character != null)
        {
            aiCharacter = character as AICharacterManager;
        }

        // Ensure the slider is initialized with the correct max value
        if (character != null)
        {
            slider.maxValue = character.characterStatsManager.maxHealth;
            slider.value = character.characterStatsManager.currentHealth;
        }

        gameObject.SetActive(false);  // Hide by default
    }

    private void Update()
    {
        // Make sure the HP bar always faces the camera
        transform.LookAt(transform.position + Camera.main.transform.forward);

        // Hide the HP bar after the timer runs out
        if (hideTimer > 0)
        {
            hideTimer -= Time.deltaTime;
        }
        else if (hideTimer <= 0)
        {
            gameObject.SetActive(false);  // Hide the bar when the timer is up
        }
    }

    public override void SetStat(int newValue)
    {
        base.SetStat(newValue);  // Update the slider value

        // Show the bar when the character takes damage
        gameObject.SetActive(true);

        // Reset hide timer whenever damage is taken
        hideTimer = defaultTimeBeforeBarHides;

        if (displayCharacterNameOnDamage && aiCharacter != null)
        {
            characterName.text = aiCharacter.characterName;
            characterName.enabled = true;
        }

        // Display the damage dealt or healed
        int damageDealt = character.characterStatsManager.currentHealth - newValue;
        if (damageDealt < 0)
        {
            characterDamage.text = "+" + Mathf.Abs(damageDealt).ToString();
        }
        else
        {
            characterDamage.text = "-" + damageDealt.ToString();
        }

        characterDamage.enabled = true;  // Enable the damage text
    }
}
