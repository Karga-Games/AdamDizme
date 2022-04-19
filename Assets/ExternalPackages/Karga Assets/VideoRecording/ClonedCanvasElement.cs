using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ClonedCanvasElement : MonoBehaviour
{

    public GameObject OriginalElement;

    protected RectTransform OriginalRTransform;
    protected Image OriginalImage;
    protected RawImage OriginalRawImage;
    protected Text OriginalText;
    protected TextMeshProUGUI OriginalTMProText;
    protected Slider OriginalSlider;
    protected Mask OriginalMask;
    protected RectMask2D OriginalRectMask2D;

    protected RectTransform CloneRTransform;
    protected Image CloneImage;
    protected RawImage CloneRawImage;
    protected Text CloneText;
    protected TextMeshProUGUI CloneTMProText;
    protected Slider CloneSlider;
    protected Mask CloneMask;
    protected RectMask2D CloneRectMask2D;
    private void Awake()
    {

    }

    public void ReadComponents()
    {
        OriginalRTransform = OriginalElement.GetComponent<RectTransform>();
        OriginalImage = OriginalElement.GetComponent<Image>();
        OriginalRawImage = OriginalElement.GetComponent<RawImage>();
        OriginalText = OriginalElement.GetComponent<Text>();
        OriginalTMProText = OriginalElement.GetComponent<TextMeshProUGUI>();
        OriginalSlider = OriginalElement.GetComponent<Slider>();
        OriginalMask = OriginalElement.GetComponent<Mask>();
        OriginalRectMask2D = OriginalElement.GetComponent<RectMask2D>();
    }

    public void CopyComponents()
    {
        if (OriginalElement == null)
        {
            Destroy(gameObject);
        }

        CopyRectTransform();
        CopyMask();
        CopyRectMask();
        CopyImage();
        CopyRawImage();
        CopyText();
        CopyTextMeshPro();
        CopySlider();
    }

    public void CopyRectTransform()
    {
        if (OriginalRTransform != null)
        {
            if (CloneRTransform == null)
            {
                CloneRTransform = GetComponent<RectTransform>();
                if (CloneRTransform == null)
                {
                    CloneRTransform = gameObject.AddComponent<RectTransform>();
                }
            }

            CloneRTransform.anchoredPosition = OriginalRTransform.anchoredPosition;
            CloneRTransform.anchorMin = OriginalRTransform.anchorMin;
            CloneRTransform.anchorMax = OriginalRTransform.anchorMax;
            CloneRTransform.localPosition = OriginalRTransform.localPosition;
            CloneRTransform.sizeDelta = OriginalRTransform.sizeDelta;
            CloneRTransform.pivot = OriginalRTransform.pivot;
            CloneRTransform.localRotation = OriginalRTransform.localRotation;
            CloneRTransform.localScale = OriginalRTransform.localScale;

        }
    }
    public void CopyMask()
    {
        if (OriginalMask != null)
        {
            if (CloneMask == null)
            {
                CloneMask = GetComponent<Mask>();
                if (CloneMask == null)
                {
                    CloneMask = gameObject.AddComponent<Mask>();
                }
            }

            CloneMask.showMaskGraphic = OriginalMask.showMaskGraphic;



        }
    }

    public void CopyRectMask()
    {
        if (OriginalRectMask2D != null)
        {
            if (CloneRectMask2D == null)
            {
                CloneRectMask2D = GetComponent<RectMask2D>();
                if (CloneRectMask2D == null)
                {
                    CloneRectMask2D = gameObject.AddComponent<RectMask2D>();
                }
            }

            CloneRectMask2D.padding = OriginalRectMask2D.padding;
            CloneRectMask2D.softness = OriginalRectMask2D.softness;



        }
    }

    public void CopyImage()
    {
        if (OriginalImage != null)
        {
            if (CloneImage == null)
            {
                CloneImage = GetComponent<Image>();
                if (CloneImage == null)
                {
                    CloneImage = gameObject.AddComponent<Image>();
                }
            }

            CloneImage.sprite = OriginalImage.sprite;
            CloneImage.color = OriginalImage.color;
            CloneImage.material = OriginalImage.material;
            CloneImage.maskable = OriginalImage.maskable;
            CloneImage.type = OriginalImage.type;
            CloneImage.fillMethod = OriginalImage.fillMethod;
            CloneImage.fillOrigin = OriginalImage.fillOrigin;
            CloneImage.fillAmount = OriginalImage.fillAmount;



        }
    }

    public void CopyRawImage()
    {
        if (OriginalRawImage != null)
        {
            if (CloneRawImage == null)
            {
                CloneRawImage = GetComponent<RawImage>();
                if (CloneRawImage == null)
                {
                    CloneRawImage = gameObject.AddComponent<RawImage>();
                }
            }

            CloneRawImage.texture = OriginalRawImage.texture;
            CloneRawImage.color = OriginalRawImage.color;
            CloneRawImage.material = OriginalRawImage.material;
            CloneRawImage.maskable = OriginalRawImage.maskable;


        }
    }

    public void CopyText()
    {
        if (OriginalText != null)
        {
            if (CloneText == null)
            {
                CloneText = GetComponent<Text>();
                if (CloneText == null)
                {
                    CloneText = gameObject.AddComponent<Text>();
                }
            }

            CloneText.text = OriginalText.text;
            CloneText.font = OriginalText.font;
            CloneText.fontSize = OriginalText.fontSize;
            CloneText.fontStyle = OriginalText.fontStyle;
            CloneText.lineSpacing = OriginalText.lineSpacing;
            CloneText.supportRichText = OriginalText.supportRichText;
            CloneText.alignment = OriginalText.alignment;
            CloneText.alignByGeometry = OriginalText.alignByGeometry;
            CloneText.horizontalOverflow = OriginalText.horizontalOverflow;
            CloneText.verticalOverflow = OriginalText.verticalOverflow;
            CloneText.resizeTextForBestFit = OriginalText.resizeTextForBestFit;
            CloneText.color = OriginalText.color;
            CloneText.material = OriginalText.material;
            CloneText.maskable = OriginalText.maskable;

        }
    }

    public void CopyTextMeshPro()
    {
        if (OriginalTMProText != null)
        {
            if (CloneTMProText == null)
            {
                CloneTMProText = GetComponent<TextMeshProUGUI>();
                if (CloneTMProText == null)
                {
                    CloneTMProText = gameObject.AddComponent<TextMeshProUGUI>();
                }
            }

            CloneTMProText.text = OriginalTMProText.text;
            CloneTMProText.textStyle = OriginalTMProText.textStyle;
            CloneTMProText.font = OriginalTMProText.font;
            CloneTMProText.fontStyle = OriginalTMProText.fontStyle;
            CloneTMProText.fontSize = OriginalTMProText.fontSize;
            CloneTMProText.fontSizeMin = OriginalTMProText.fontSizeMin;
            CloneTMProText.fontSizeMax = OriginalTMProText.fontSizeMax;
            CloneTMProText.enableAutoSizing = OriginalTMProText.enableAutoSizing;
            CloneTMProText.autoSizeTextContainer = OriginalTMProText.autoSizeTextContainer;
            CloneTMProText.characterSpacing = OriginalTMProText.characterSpacing;
            CloneTMProText.lineSpacing = OriginalTMProText.lineSpacing;
            CloneTMProText.alignment = OriginalTMProText.alignment;
            CloneTMProText.color = OriginalTMProText.color;

        }
    }

    public void CopySlider()
    {
        if (OriginalSlider != null)
        {
            if (CloneSlider == null)
            {
                CloneSlider = GetComponent<Slider>();
                if (CloneSlider == null)
                {
                    CloneSlider = gameObject.AddComponent<Slider>();
                }
            }

            CloneSlider.interactable = OriginalSlider.interactable;
            CloneSlider.transition = OriginalSlider.transition;
            CloneSlider.targetGraphic = OriginalSlider.targetGraphic;
            CloneSlider.value = OriginalSlider.value;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ReadComponents();
    }

    // Update is called once per frame
    void Update()
    {
        CopyComponents();
    }
}

public class CloneElement
{

}

public class CloneImage : CloneElement
{

}

public class CloneText : CloneElement
{

}

public class CloneTMPro : CloneElement
{

}