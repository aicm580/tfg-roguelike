using UnityEngine;

public class UIButton : MonoBehaviour
{
    public bool currentlySelected;

    public void OnMouseEnter()
    {
        AudioManager.audioManagerInstance.PlaySFX("ButtonHover");
    }

    public void OnMouseClickSelect()
    {
        if (!currentlySelected)
            AudioManager.audioManagerInstance.PlaySFX("ButtonClick");

        currentlySelected = true;
    }

    public void NotSelected()
    {
        currentlySelected = false;
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
