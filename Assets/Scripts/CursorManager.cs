using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager cursorInstance;

    public Texture2D gameCursorTexture;
    public Texture2D basicCursorTexture;

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
        SetCursor(basicCursorTexture);
    }

    public void SetCursor(Texture2D tex)
    {
        CursorMode mode = CursorMode.ForceSoftware;
        Cursor.SetCursor(tex, Vector2.zero, mode);
    }
}
