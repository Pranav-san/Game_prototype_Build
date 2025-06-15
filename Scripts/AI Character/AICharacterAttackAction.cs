using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Action/ Attack Action")]

public class AICharacterAttackAction : ScriptableObject
{

    [Header("Attack")]
    [SerializeField] private string attackAnimation;

    [Header("ComBoAction")]
    public AICharacterAttackAction comboAction;//Combo Action Of Thi Attack 

    [Header("Attack Values")]
    public int attackWeight = 50;
    [SerializeField]AttackType attackType;

    public float actionRecoveryTime = 1.5f;//The time befor the character can make another action
    public float minimumAttackAngle = -35;
    public float maximumAttackAngle = 35;
    public float minimumAttackDistance = 0;
    public float maximumAttackDistance = 2;

    public void AttemptToPerformAction(AICharacterManager aiCharacter)
    {
        //If Ai character Attack animation is Purely Equipment/Weapon Based, If so use PlayTargetAttackActionAnimation()
        //aiCharacter.characterAnimatorManager.PlayTargetAttackActionAnimation(attackType, attackAnimation,true);

        //If Ai character Attack is Purely Animation based, & not Equipment/Weapon Based, If so use PlayTargetActionAnimation()
        aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(attackAnimation, true);


    }



    






}
