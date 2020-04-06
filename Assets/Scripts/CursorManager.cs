using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager cursorInstance;

    public Texture2D gameCursor;
    public Texture2D basicCursor;
    private Texture2D cursorTexture;
    private int cursorSizeX = 26;
    private int cursorSizeY = 26;

    private void Awake()
    {
        //Nos aseguramos de que solo haya 1 CursorManager
        if (cursorInstance == null)
        {
            cursorInstance = this;
        }
        else if (cursorInstance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        cursorTexture = basicCursor;
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(Event.current.mousePosition.x - cursorSizeX/2,
                        Event.current.mousePosition.y - cursorSizeY/2, 
                        cursorSizeX, cursorSizeY), cursorTexture);
    }

    public void SetCursor(Texture2D texture)
    {
        cursorTexture = texture;
    }
}
