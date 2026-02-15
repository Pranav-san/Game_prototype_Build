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
    [SerializeField] UI_YellowBar yellowBar;
    [SerializeField] float yellowBarTimer = 3;

    protected override void Awake()
    {
        base.Awake();
        character = GetComponentInParent<CharacterManager>();
        yellowBar = GetComponentInChildren<UI_YellowBar>();

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


        if(yellowBar != null)
        {
            yellowBar.gameObject.SetActive(false);// Trigger The OnEnable Method On The Yellow Bar
        }
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

        if(yellowBar != null)
        {
            if(newHealth < oldHealth)
            {
                yellowBar.slider.value =  Mathf.Max(yellowBar.slider.value, oldHealth);
                yellowBar.gameObject.SetActive(true);
                yellowBar.timer = yellowBarTimer; // Every Time We Hit We renew Timer
            }
        }

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

    public void ResetHPBar()
    {
        gameObject.SetActive(false);
    }




}
