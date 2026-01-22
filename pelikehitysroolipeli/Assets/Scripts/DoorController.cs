using System;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public enum DoorStates
{
    Open,
    Closed,
    Locked
}
public enum Actions
{
    Open,
    Close,
    Lock,
    Unlock
}
public class DoorController : MonoBehaviour
{
    // Kuvat oven eri tiloille
    [SerializeField]
    Sprite ClosedDoorSprite;
    [SerializeField]
    Sprite OpenDoorSprite;
    [SerializeField]
    Sprite LockedSprite;
    [SerializeField]
    Sprite UnlockedSprite;

    BoxCollider2D colliderComp;

    // Näitä värejä käytetään lukkosymbolin piirtämiseen.
    public static Color lockedColor;
    public static Color openColor;

    SpriteRenderer doorSprite; // Oven kuva
    SpriteRenderer lockSprite; // Lapsi gameobjectissa oleva lukon kuva

    // Debug ui
    [SerializeField]
    bool ShowDebugUI;
    [SerializeField]
    int DebugFontSize = 32;

    DoorStates doorState = DoorStates.Locked;

    void Start()
    {
        doorSprite = GetComponent<SpriteRenderer>();
        colliderComp = GetComponent<BoxCollider2D>();
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        if (sprites.Length == 2 && sprites[0] == doorSprite)
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
    public void ReceiveAction(Actions action)
    {
        string[] toiminnot = Enum.GetNames(typeof(Actions));
        for (int i = 0; i < toiminnot.Length; i++)
        {
            if (action.ToString().ToLower() == toiminnot[i].ToLower())
            {
                switch (action)
                {
                    case Actions.Open:
                        {
                            if(doorState == DoorStates.Closed)
                            {
                                OpenDoor();
                            }
                            break;
                        }
                    case Actions.Close:
                        {
                            if (doorState == DoorStates.Open)
                            {
                                CloseDoor();
                            }
                            break;
                        }
                    case Actions.Lock:
                        {
                            if (doorState == DoorStates.Closed)
                            {
                                LockDoor();
                            }
                            break;
                        }
                    case Actions.Unlock:
                        {
                            if (doorState == DoorStates.Locked)
                            {
                                UnlockDoor();
                            }
                            break;
                        }
                }
            }
        }
    }

    // Kun tulee toiminto, sen perusteella kutsutaan jotakin
    // näistä funktioista että oven tila muuttuu

    /// <summary>
    /// Vaihtaa oven kuvan avoimeksi oveksi
    /// ja laittaa törmäysalueen pois päältä
    /// </summary>
    private void OpenDoor()
    {
        doorSprite.sprite = OpenDoorSprite;
        colliderComp.isTrigger = true;
        doorState = DoorStates.Open;
    }

    /// <summary>
    /// Vaihtaa oven kuvan suljetuksi oveksi
    /// ja laittaa törmäysalueen päälle
    /// </summary>
    private void CloseDoor()
    {
        doorSprite.sprite = ClosedDoorSprite;
        colliderComp.isTrigger = false;
        doorState = DoorStates.Closed;
    }

    /// <summary>
    /// Vaihtaa lukkosymbolin lukituksi ja
    /// vaihtaa sen värin
    /// </summary>
    private void LockDoor()
    {
        lockSprite.sprite = LockedSprite;
        lockSprite.color = lockedColor;
        doorState = DoorStates.Locked;
    }

    /// <summary>
    /// Vaihtaa lukkosymbolin avatuksi ja
    /// vaihtaa sen värin
    /// </summary>
    private void UnlockDoor()
    {
        lockSprite.sprite = UnlockedSprite;
        lockSprite.color = openColor;
        doorState = DoorStates.Closed;
    }

    // *********************************
    // Unityssä on välittömän käyttöliittymän
    // järjestelmä, jolla voi piirtää 
    // nappeja ja tekstiä koodin avulla.
    // Se on kätevä erilaisten oikoteiden ja
    // testaamisen apuvälineiden kehittämiseen.
    // Tässä sitä on käytetty tekemään napit, joilla
    // voi testata että oven eri toiminnot
    // toimivat oikein.


  

    // Unity kutsuu tätä funktiota kaiken muun piirtämisen jälkeen
    // Sen sisällä voi piirtää käyttöliittymää
    private void OnGUI()
    {
        if (ShowDebugUI == false)
        {
            return;
        }
        GUIStyle buttonStyle = GUI.skin.GetStyle("button");
        GUIStyle labelStyle = GUI.skin.GetStyle("label");
        buttonStyle.fontSize = DebugFontSize;
        labelStyle.fontSize = DebugFontSize;
        Rect guiRect = GetGuiRect();
        GUILayout.BeginArea(guiRect);
        
        GUILayout.Label("Door");
        if (GUILayout.Button("Open"))
        {
            OpenDoor();
        }
        if (GUILayout.Button("Close"))
        {
            CloseDoor();
        }
        if (GUILayout.Button("Lock"))
        {
            LockDoor();
        }
        if (GUILayout.Button( "Unlock"))
        {
            UnlockDoor();
        }
        
        GUILayout.EndArea();
    }

    // Näiden kahden funktion avulla ei tarvitse itse
    // määrittää jokaisen napin paikkaa, vaan ne
    // ladotaan automaattisesti allekkain.
   

    private Rect GetGuiRect()
    {
        Vector3 buttonPos = transform.position;
        buttonPos.x += 1;
        buttonPos.y -= 0.25f;
        // Tällä tavalla voi laskea paikan jossa GameObject näkyy
        // ruudulla ja piirtää käyttöliittymän sen kohdalle.
        // WorldToScreenPoint antaa Y koordinaatin väärin päin
        // tai GUI koodi ymmärtää sen väärin,
        // ja siksi se pitä vähentää ruudun korkeudesta.
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(buttonPos);
        float screenHeight = Screen.height;
        return new Rect(screenPoint.x, screenHeight - screenPoint.y, 
            DebugFontSize * 8,  // Leveys ja korkeus niin että varmasti mahtuu
            DebugFontSize * 100);
    }    
}
