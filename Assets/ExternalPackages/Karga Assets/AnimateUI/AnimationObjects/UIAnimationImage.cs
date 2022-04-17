using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimationImage : UIAnimationObject
{
    [Header("Movement Commands on Image Object")]
    [SerializeField]
    public List<UIImageMovement> MovementCommands;

    private Image imageSource;



    public override float PlayCommandsSimple()
    {
        return PlayCommands(MovementCommands);
    }

    public float PlayCommands(List<UIImageMovement> commandList)
    {
        float animationDuration = 0;
        foreach (UIImageMovement movementCommand in commandList)
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
            if (movementCommand.spriteChangeEnabled)
            {
                changeImage(movementCommand.targetSprite, animationDuration);
            }
            if (movementCommand.colorChangeEnabled)
            {
                changeColor(movementCommand.targetColor, movementCommand.movementDuration, movementCommand.movementType, animationDuration);
            }

            animationDuration += movementCommand.movementDuration + movementCommand.waitAfterComplete;

            StartCoroutine(GeneralFunctions.executeAfterSec(() => {

                movementCommand.OnComplete.Invoke();

            },animationDuration)); ;

        }
        return animationDuration;
    }


    public override float PlayCommandsByGroup(UIMovementGroup animationGroup)
    {
        List<UIImageMovement> groupCommandList = new List<UIImageMovement>();
        foreach (UIImageMovement movementCommand in MovementCommands)
        {
            if (movementCommand.movementGroup == animationGroup)
            {

                groupCommandList.Add(movementCommand);

            }
        }

       return PlayCommands(groupCommandList);
    }

    public void changeImage(Sprite newImage, float delay)
    {
        if (imageSource == null)
        {
            imageSource = GetComponent<Image>();
        }

       StartCoroutine(GeneralFunctions.executeAfterSec(() => { imageSource.sprite = newImage; },delay));

    }

    public void changeColor(Color newColor, float duration, LeanTweenType animtype, float delay)
    {
        LeanTween.color(GetRectTransform(), newColor, duration).setEase(animtype).setDelay(delay);
    }


}
