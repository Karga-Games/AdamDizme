using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAnimationObject : MonoBehaviour
{
   
    private RectTransform rectTransform;

    public void SetRectTransform()
    {
        if(GetComponent<RectTransform>() != null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
        
    }

    public RectTransform GetRectTransform()
    {
        if(rectTransform == null)
        {
            SetRectTransform();
        }
        return rectTransform;
    }


    public virtual float PlayCommandsSimple()
    {
        Debug.LogWarning("Base Class Virtual PlayAllCommands function executed!");
        return 0;
    }

    public virtual float PlayCommandsByGroup(UIMovementGroup animationGroup)
    {
        Debug.LogWarning("Base Class Virtual PlayAllCommandsByGroup function executed!");
        return 0;
    }

    public void changePosition(Vector3 newPos,float duration,LeanTweenType animtype,float delay = 0f)
    {

        LeanTween.move(GetRectTransform(), newPos, duration).setEase(animtype).setDelay(delay);

    }

    public void changeRotation(Vector3 newRot, float duration, LeanTweenType animtype,float delay = 0f)
    {

        LeanTween.rotate(GetRectTransform(), newRot, duration).setEase(animtype).setDelay(delay);

    }

    public void changeScale(Vector3 newScale, float duration, LeanTweenType animtype, float delay = 0f)
    {
        LeanTween.scale(GetRectTransform(),newScale,duration).setEase(animtype).setDelay(delay);
    }
}

public class UIAnimationText : UIAnimationObject
{

    public virtual void changeText(string newValue, float delay)
    {

    }

    public virtual void changeColor()
    {

    }

}



