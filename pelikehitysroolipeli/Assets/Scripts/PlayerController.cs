using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum NuolenKärki
{
    Puu,
    Teräs,
    Timantti
}
public enum NuolenSulka
{
    Lehti,
    Kanansulka,
    Kotkansulka
}
public enum Paaraaka_aine
{
    Nautaa,
    Kanaa,
    Kasviksia
}
public enum Lisuke
{
    Perunaa,
    Riisiä,
    Pastaa
}
public enum Kastike
{
    Curry,
    Hapanimelä,
    Pippuri,
    Chili
}

public class PlayerController : MonoBehaviour
{
    Vector2 lastMovement;
    Rigidbody2D rb;
    [SerializeField]
    float moveSpeed;

    DoorController doorController;
    PlayerShoppingController playerShoppingController;

    [SerializeField]
    GameObject doorButtons;

    //[SerializeField]
    //GameObject merchantButtons;

    //GameObject option3;
    //GameObject slider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerShoppingController = transform.GetComponent<PlayerShoppingController>();
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

        //option3 = GameObject.Find("Option3");
        //slider = GameObject.Find("SliderText");
        

        doorButtons.SetActive(false);
        //merchantButtons.SetActive(false);
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
            doorButtons.SetActive(true);
        }
        else if (collision.CompareTag("Merchant"))
        {
            if (collision.name == "ArrowMerchant")
            {
                playerShoppingController.StartShopping(PlayerShoppingController.MerchantType.ArrowMerchant);
            }
            else if (collision.name == "FoodMerchant")
            {
                playerShoppingController.StartShopping(PlayerShoppingController.MerchantType.FoodMerchant);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        doorButtons.SetActive(false);
        //merchantButtons.SetActive(false);
        playerShoppingController.EndShopping();
    }

    void OnMoveAction(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();
        lastMovement = v;
    }    
}
