using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum UIMovementGroup
{
    None,
    StartingMovement,
    IdleMovement,
    FinishingMovement
}

[System.Serializable]
public enum UIMovementCommand
{
    PositionChange,
    RotationChange,
    ScaleChange,
    ColorChange,
    AlphaChange,
    ImageChange
}



[System.Serializable]
public class UIMovement
{

    [Header("Movement Setup")]
    public UIMovementGroup movementGroup;
    public LeanTweenType movementType;

    [Header("Movement Targets")]
    public Vector3 movementTargetPosition;
    public bool positionChangeEnabled;
    public Vector3 movementTargetRotation;
    public bool rotationChangeEnabled;
    public Vector3 targetScale;
    public bool scaleChangeEnabled;

    [Header("Timing")]
    public float movementDuration;
    public float waitAfterComplete;


    public UnityEvent OnComplete;

}

public class UITextMovement : UIMovement
{
    [Header("Change Text")]
    public string targetText;
    public bool textChangeEnabled;
}

[System.Serializable]
public class UIUnityTextMovement : UITextMovement
{
    [Header("Change Unity Text Color")]
    public Color targetColor;
    public bool colorChangeEnabled;
}
[System.Serializable]
public class UITMProMovement : UITextMovement
{
    [Header("Change Face Color")]
    public Color targetFaceColor;
    public bool faceColorChangeEnabled;

    [Header("Change Outline Color")]
    public Color targetOutlineColor;
    public bool outlineColorChangeEnabled;
}

[System.Serializable]
public class UIImageMovement: UIMovement
{
    [Header("Change Image Source")]
    public Sprite targetSprite;
    public bool spriteChangeEnabled;

    [Header("Change Image Color")]
    public Color targetColor;
    public bool colorChangeEnabled;

}

