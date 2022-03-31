using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIAnimationTMProText : UIAnimationText
{
    private TextMeshProUGUI textComponent;
    [Header("Movement Commands on TMProText Object")]
    [SerializeField]
    public List<UITMProMovement> MovementCommands;

    void Start()
    {

    }

    public override float PlayCommandsSimple()
    {
       return PlayCommands(MovementCommands);
    }

    public float PlayCommands(List<UITMProMovement> commandList)
    {
        float animationDuration = 0;
        foreach (UITMProMovement movementCommand in commandList)
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
            if (movementCommand.faceColorChangeEnabled)
            {
                changeColorFace(movementCommand.targetFaceColor, movementCommand.movementDuration, movementCommand.movementType, animationDuration);
            }
            if (movementCommand.outlineColorChangeEnabled)
            {
                changeColorOutLine(movementCommand.targetOutlineColor, movementCommand.movementDuration, movementCommand.movementType, animationDuration);
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
        List<UITMProMovement> groupCommandList = new List<UITMProMovement>();
        foreach (UITMProMovement movementCommand in MovementCommands)
        {
            if (movementCommand.movementGroup == animationGroup)
            {

                groupCommandList.Add(movementCommand);

            }
        }

       return PlayCommands(groupCommandList);
    }


    public override void changeText(string newValue,float delay)
    {
        if(textComponent == null)
        {
             textComponent = GetComponent<TextMeshProUGUI>();
        }

        if(delay > 0)
        {
            StartCoroutine(GeneralFunctions.executeAfterSec(() => { textComponent.text = newValue; }, delay));
        }
        else
        {
            textComponent.text = newValue;
        }
        
    }

    public void changeColorFace(Color newColor, float duration, LeanTweenType animtype, float delay)
    {
        LeanTween.value(gameObject, textComponent.faceColor , newColor, duration).setEase(animtype).setDelay(delay);
        LeanTween.value(gameObject, textComponent.faceColor.a, newColor.a, duration).setEase(animtype).setDelay(delay);
    }

    public void changeColorOutLine(Color newColor, float duration, LeanTweenType animtype, float delay)
    {
        LeanTween.value(gameObject, textComponent.outlineColor, newColor, duration).setEase(animtype).setDelay(delay);
    }

}
