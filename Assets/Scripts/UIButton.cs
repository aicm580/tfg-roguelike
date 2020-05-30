using UnityEngine;

public class UIButton : MonoBehaviour
{
    public bool currentlySelected;

    public void OnMouseEnter()
    {
        AudioManager.audioManagerInstance.PlaySFX("ButtonHover");
    }

    public void OnMouseClick()
    {
        if (!currentlySelected)
            AudioManager.audioManagerInstance.PlaySFX("ButtonClick");

        currentlySelected = true;
    }

    public void OnStartNewRun()
    {
        AudioManager.audioManagerInstance.PlaySFX("NewRunButtonClick");
    }

    public void NotSelected()
    {
        currentlySelected = false;
    }
    
}
