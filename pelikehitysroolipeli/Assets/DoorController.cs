using UnityEngine;

public class DoorController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    Sprite ClosedDoorSprite;
    [SerializeField]
    Sprite OpenDoorSprite;
    [SerializeField]
    Sprite LockedSprite;
    [SerializeField]
    Sprite UnlockedSprite;

    BoxCollider2D colliderComp;

    
    public static Color lockedColor;
   
    public static Color openColor;

    SpriteRenderer sp;
    SpriteRenderer lockSprite;

    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        colliderComp = GetComponent<BoxCollider2D>();
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        if (sprites.Length == 2 && sprites[0] == sp)
        {
            lockSprite = sprites[1];
        }

        
        lockedColor = new Color(1.0f, 0.63f, 0.23f);
        openColor = new Color(0.5f, 0.8f, 1.0f);


         // TODO
         // missä tilassa ovi on kun peli alkaa?
    }

    /// <summary>
    /// Oveen kohdistuu jokin toiminto joka muuttaa sen tilaa
    /// </summary>
    public void ReceiveAction()
    {
        
    }

    // Kun tulee toiminto, sen perusteella kutsutaan jotakin
    // näistä funktioista että oven tila muuttuu

    private void OpenDoor()
    {
        sp.sprite = OpenDoorSprite;
        colliderComp.isTrigger = true;
    }
    private void CloseDoor()
    {
        sp.sprite = ClosedDoorSprite;
        colliderComp.isTrigger = false;
    }
    private void LockDoor()
    {
        lockSprite.sprite = LockedSprite;
        lockSprite.color = lockedColor;
    }
    private void UnlockDoor()
    {
        lockSprite.sprite = UnlockedSprite;
        lockSprite.color = openColor;
    }

    // Tällä voi testata että funktiot toimivat

    Rect guiRect;
    private Rect NextGuiRect()
    {
        Rect next = guiRect;
        guiRect.y += 32;
        return next;
    }

    
    private void ResetGuiRect()
    {
        Vector3 buttonPos = transform.position;
        buttonPos.x += 1;
        buttonPos.y -= 0.25f;
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(buttonPos);
        float screenHeight = Screen.height;
        guiRect = new Rect(screenPoint.x, screenHeight - screenPoint.y, 100, 32);
    }    

    private void OnGUI()
    {
        ResetGuiRect();
        if (GUI.Button(NextGuiRect(), "Open"))
        {
            OpenDoor();
        }
        if (GUI.Button(NextGuiRect(), "Close"))
        {
            CloseDoor();
        }
        if (GUI.Button(NextGuiRect(), "Lock"))
        {
            LockDoor();
        }
        if (GUI.Button(NextGuiRect(), "Unlock"))
        {
            UnlockDoor();
        }
    }
}
