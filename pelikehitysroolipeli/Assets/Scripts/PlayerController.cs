using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Vector2 lastMovement;
    Rigidbody2D rb;
    [SerializeField]
    float moveSpeed;

    DoorController doorController;

    [SerializeField]
    GameObject uiCanvas;
    //[SerializeField]
    //Button openButton;
    //[SerializeField]
    //Button closeButton;
    //[SerializeField]
    //Button lockButton;
    //[SerializeField]
    //Button unlockButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        lastMovement = Vector2.zero;
        rb = GetComponent<Rigidbody2D>();

        //openButton.onClick.AddListener(OnOpenButton);
        //closeButton.onClick.AddListener(OnCloseButton);
        //lockButton.onClick.AddListener(OnLockButton);
        //unlockButton.onClick.AddListener(OnUnlockButton);

        Button openbutton = GameObject.Find("OpenButton").GetComponent<Button>();
        openbutton.onClick.AddListener(OnOpenButton);
        Button closebutton = GameObject.Find("CloseButton").GetComponent<Button>();
        closebutton.onClick.AddListener(OnCloseButton);
        Button lockbutton = GameObject.Find("LockButton").GetComponent<Button>();
        lockbutton.onClick.AddListener(OnLockButton);
        Button unlockbutton = GameObject.Find("UnlockButton").GetComponent<Button>();
        unlockbutton.onClick.AddListener(OnUnlockButton);

        uiCanvas.SetActive(false);
    }

    void OnOpenButton()
    {
        doorController.ReceiveAction(Actions.Open);        
    }
    void OnCloseButton()
    {
        doorController.ReceiveAction(Actions.Close);
    }
    void OnLockButton()
    {
        doorController.ReceiveAction(Actions.Lock);
    }
    void OnUnlockButton()
    {
        doorController.ReceiveAction(Actions.Unlock);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        rb.MovePosition(rb.position + lastMovement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Huomaa mitä pelaaja löytää
        if (collision.CompareTag("Door"))
        {
            doorController = collision.GetComponent<DoorController>();
            uiCanvas.SetActive(true);
        }
        else if (collision.CompareTag("Merchant"))
        {
            Debug.Log("Found Merchant");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        uiCanvas.SetActive(false);
    }

    void OnMoveAction(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();
        lastMovement = v;
    }    
}
