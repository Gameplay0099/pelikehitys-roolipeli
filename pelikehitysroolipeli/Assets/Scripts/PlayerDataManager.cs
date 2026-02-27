using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }

    GameObject canvas;
    TextMeshProUGUI hpText;
    TextMeshProUGUI xpText;
    TextMeshProUGUI moneyText;
    
    public int xp = 2;
    public int money = 15;
    public int hp = 10;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Pelissä on liikaa PlayerDataManager objekteja!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        // Tee sellaiseksi, että saa helposti jokaisen tekstikentän käyttöön.
        canvas = GameObject.Find("UI Canvas");

        hpText = canvas.transform.Find("PlayerInfo").transform.Find("PlayerHealthBackground").GetComponentInChildren<TextMeshProUGUI>();
        hpText.text = $"Player HP: {hp}";

        xpText = canvas.transform.Find("PlayerInfo").transform.Find("PlayerXPBackground").GetComponentInChildren<TextMeshProUGUI>();
        xpText.text = $"Player XP: {xp}";

        moneyText = canvas.transform.Find("PlayerInfo").transform.Find("PlayerMoneyBackground").GetComponentInChildren<TextMeshProUGUI>();
        moneyText.text = $"Player Money: {money}";
    }

    public void MuunnaArvoa(int maara, string muunnettavat)
    {
        switch (muunnettavat)
        {
            case "hp":
                {
                    hp += maara;
                    if (hp < 0)
                    {
                        hp = 0;
                    }
                    //if (vahenna && maara<=hp) { hp -= maara; }
                    //else if (!vahenna){ hp += maara; }
                    //else { Debug.LogError("HP cannot be negative!"); }
                    hpText.text = $"Player HP: {hp}";
                        break;
                }
            case "xp":
                {
                    xp += maara;
                    if (xp < 0)
                    {
                        xp = 0;
                    }
                    //if(vahenna && maara <= xp) { xp -= maara; }
                    //else if (!vahenna) { xp += maara; }
                    //else { Debug.LogError("XP cannot be negative!"); }
                    xpText.text = $"Player XP: {xp}";
                        break;
                }
            case "money":
                {
                    money += maara;
                    if (money < 0)
                    {
                        money = 0;
                    }
                    //if (vahenna && maara <= money) { money -= maara; }
                    //else if (!vahenna) { money += maara; }
                    //else { Debug.LogError("Money cannot be negative!"); }
                    moneyText.text = $"Player Money: {money}";
                        break;
                }
        }
    }
    public GUIContent testContent;
    public GUIStyle testStyle;
    public GUISkin testSkin;
     void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.height/1200f, Screen.height/940f, 1f));
        //GUIStyle buttonStyle = GUI.skin.GetStyle("Button");
        //buttonStyle.fontSize = (int)hpText.fontSize;
        //GUI.skin.GetStyle("Button").fontSize = (int)xpText.fontSize;
        GUI.skin.GetStyle("Button").fontSize = 24;

        //GUILayout.BeginArea(new Rect(50,80,DebugFontSize*12,DebugFontSize*100));
        GUILayout.BeginArea(new Rect(60, 70, hpText.fontSize * 14, hpText.fontSize * 75));
        
        GUILayout.BeginHorizontal(GUILayout.Height(120));
        if (GUILayout.Button("-1 HP"))
        {
            MuunnaArvoa(-1, "hp");
        }
        if (GUILayout.Button("+1 HP"))
        {
            MuunnaArvoa(1, "hp");
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUILayout.Height(120));
        if (GUILayout.Button("-1 XP"))
        {
            MuunnaArvoa(-1, "xp");
        }
        if (GUILayout.Button("+1 XP"))
        {
            MuunnaArvoa(1, "xp");
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("-5 Money"))
        {
            MuunnaArvoa(-5, "money");
        }
        if (GUILayout.Button("+5 Money"))
        {
            MuunnaArvoa(5, "money");
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
}
