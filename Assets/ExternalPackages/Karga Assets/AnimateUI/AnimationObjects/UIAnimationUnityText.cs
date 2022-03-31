using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIAnimationUnityText : UIAnimationText
{
    private Text textComponent;
    [Header("Movement Commands on UnityText Object")]
    [SerializeField]
    public List<UIUnityTextMovement> MovementCommands;
    
    public override float PlayCommandsSimple()
    {
         return PlayCommands(MovementCommands);
    }

    public float PlayCommands(List<UIUnityTextMovement> commandList)
    {
        float animationDuration = 0;
        foreach (UIUnityTextMovement movementCommand in commandList)
        {

            if (movementCommand.positionChangeEnabled)
            {
                changePosition(movementCommand.movementTargetPosition, movementCommand.movementDuration, movementCommand.movementType, animationDuration);
            }
            if (movementCommand.scaleChangeEnabled)
            {
                changeScale(movementCommand.targetScale, movementCommand.movementDuration, movementCommand.movementType, animationDuration);
            }
            if (movementCommand.rotationChangeEnabled)
            {
                changeRotation(movementCommand.movementTargetRotation, movementCommand.movementDuration, movementCommand.movementType, animationDuration);
            }
            if (movementCommand.colorChangeEnabled)
            {
                changeColor(movementCommand.targetColor, movementCommand.movementDuration, movementCommand.movementType, animationDuration);
            }
            if (movementCommand.textChangeEnabled)
            {
                changeText(movementCommand.targetText, animationDuration);
            }

            animationDuration += movementCommand.movementDuration + movementCommand.waitAfterComplete;
        }

        return animationDuration;
    }


    public override float PlayCommandsByGroup(UIMovementGroup animationGroup)
    {
        List<UIUnityTextMovement> groupCommandList = new List<UIUnityTextMovement>();
        foreach (UIUnityTextMovement movementCommand in MovementCommands)
        {
            if (movementCommand.movementGroup == animationGroup)
            {

                groupCommandList.Add(movementCommand);

            }
        }

        return PlayCommands(groupCommandList);
    }

    public override void changeText(string newValue, float delay)
    {

        if (textComponent == null)
        {
            textComponent = GetComponent<Text>();
        }

        if (delay > 0)
        {
            StartCoroutine(GeneralFunctions.executeAfterSec(() => { textComponent.text = newValue; }, delay));
        }
        else
        {
            textComponent.text = newValue;
        }
            
    }

    public void changeColor(Color newColor , float duration, LeanTweenType animtype, float delay)
    {
        LeanTween.colorText(GetRectTransform(), newColor, duration).setEase(animtype).setDelay(delay);
    }



}