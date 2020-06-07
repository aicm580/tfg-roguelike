using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public bool currentlySelected;
    public Color initialColor;
    public Color hoverColor;

    private Text buttonText;

    private void Start()
    {
        buttonText = GetComponentInChildren<Text>();
    }
    
    public void OnMouseEnter()
    {
        if (!currentlySelected)
        {
            AudioManager.audioManagerInstance.PlaySFX("ButtonHover");
            if (buttonText != null)
                buttonText.color = hoverColor;
        }
    }

    public void OnMouseExit()
    {
        if (buttonText != null && !currentlySelected)
            buttonText.color = initialColor;
        else
            buttonText.color = Color.green;
    }

    public void OnMouseClickSelect()
    {
        if (!currentlySelected)
        {
            AudioManager.audioManagerInstance.PlaySFX("ButtonClick");
            currentlySelected = true;
            buttonText.color = Color.green;
        }
    }

    public void NotSelected()
    {
        currentlySelected = false;
        buttonText.color = initialColor;
    }

    public void OnMouseClick()
    {
        AudioManager.audioManagerInstance.PlaySFX("ButtonClick");
    }

    public void OnStartNewRun()
    {
        AudioManager.audioManagerInstance.PlaySFX("NewRunButtonClick");
    }

    public void OnSave()
    {
        AudioManager.audioManagerInstance.PlaySFX("SaveButton");
    }
}
