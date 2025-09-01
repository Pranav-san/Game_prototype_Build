using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Character_HP_Bar : UI_StatBar, AIHealthUI
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

    public void OnHealthChanged(int oldHealth, int newHealth)
    {
        base.SetStat(newHealth); // Update the slider

        gameObject.SetActive(true);
        hideTimer = defaultTimeBeforeBarHides;

        int delta = oldHealth - newHealth;

        if (displayCharacterNameOnDamage && aiCharacter != null)
        {
            characterName.text = aiCharacter.characterName;
            characterName.enabled = true;
        }

        characterDamage.text = delta < 0 ? "+" + Mathf.Abs(delta) : "-" + delta;
        characterDamage.enabled = true;
    }




}
