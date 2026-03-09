using TMPro;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class PlayerShoppingController : MonoBehaviour
{
    //Enum joka listaa erilaiset kauppiaat
    public enum MerchantType
    {
        FoodMerchant,
        ArrowMerchant
    }

    // Nuolikauppiaan tarvitsemat enumit, samat kuin teht‰v‰ss‰ 3 Nuolia kaupan
    enum Nuolenk‰rki
    {
        Puu,
        Ter‰s,
        Timantti
    }
    enum Nuolensulka
    {
        Lehti,
        Kanansulka,
        Kotkansulka
    }
    // Ateriakauppiaan tarvitsemat enumit, samat kuin teht‰v‰ss‰ 2 Ruoka-annos Generaattori
    enum P‰‰raaka_aine
    {
        Nautaa,
        Kanaa,
        Kasviksia
    }
    enum Lisuke
    {
        Perunaa,
        Riisi‰,
        Pastaa
    }
    enum Kastike
    {
        Curry,
        Hapanimel‰,
        Pippuri,
        Chili
    }

    // N‰m‰ muuttujat muistavat pelaajan valinnat ruokakauppiaalta
    P‰‰raaka_aine valittuP‰‰;
    Lisuke valittuLisuke;
    Kastike valittuKastike;

    // N‰m‰ muuttujat muistavat pelaajan valinnat nuolikauppiaalta
    Nuolenk‰rki valittuK‰rki;
    Nuolensulka valittuSulka;

    // Viitteet paneeliin ja tekstielementteihin sek‰ ostonappiin
    // N‰ihin otetaan viitteet koska kaikki kauppiaat k‰ytt‰v‰t niit‰
    GameObject merchantPanel;
    GameObject option3;
    GameObject slider;

    float nuolenPituus;

    TextMeshProUGUI nameText;
    TextMeshProUGUI priceText;
    TextMeshProUGUI arrowLengthText;
    Button buyButton;

    MerchantType merchantType;

    int price = 1;
    int addedHP = 0;
    PlayerDataManager playerData = PlayerDataManager.Instance;
    AudioManager audioManager = AudioManager.Instance;
    float sliderSoundCooldown = 0.0f;
    bool isSlider = false;

    private void Start()
    {
        // Kun peli alkaa, hae paneeli
        merchantPanel = GameObject.Find("MerchantPanel");

        // Hae paneelista aina k‰ytetyt elementit
        nameText = merchantPanel.transform.Find("NameText").gameObject.GetComponent<TextMeshProUGUI>();

        option3 = merchantPanel.transform.Find("Option3").gameObject;
        slider = merchantPanel.transform.Find("Slider").gameObject;

        priceText = merchantPanel.transform.Find("PriceText").GetComponent<TextMeshProUGUI>();

        arrowLengthText = merchantPanel.transform.Find("Slider").transform.Find("ArrowLengthPanel").GetComponentInChildren<TextMeshProUGUI>();

        buyButton = merchantPanel.transform.Find("BuyButton").GetComponent<Button>();
        buyButton.onClick.AddListener(OnBuyButtonPressed);

        // Piilota paneeli
        merchantPanel.SetActive(false);
    }

    private void FixedUpdate()
    {
        sliderSoundCooldown -= Time.deltaTime;
    }

    /// <summary>
    /// Kutsu t‰t‰ funktiota kun pelaaja lˆyt‰‰ kauppiaan.
    /// </summary>
    /// <param name="merchantType">Millaisen kauppiaan pelaaja lˆysi</param>
    public void StartShopping(MerchantType merchantType)
    {
        this.merchantType = merchantType;
        // Laita paneeli ja elementit n‰kyviin
        merchantPanel.SetActive(true);

        // Kytke funktiot, joita kutsutaan, kun valinta muuttuu
        TMP_Dropdown dd1 = GetDropdown(merchantPanel, "Option1");
        dd1.onValueChanged.AddListener(OnDropdownValueChanged_1);

        // Lisukkeen pudotusvalikko
        TMP_Dropdown dd2 = GetDropdown(merchantPanel, "Option2");
        dd2.onValueChanged.AddListener(OnDropdownValueChanged_2);

        TMP_Dropdown dd3 = GetDropdown(merchantPanel, "Option3");
        dd3.onValueChanged.AddListener(OnDropDownValueChanged_3);

        Slider sl1 = GetSlider(merchantPanel, "Slider");
        sl1.onValueChanged.AddListener(OnSliderValueChanged_1);
        float sliderValue = sl1.value;

        // Laita elementtien sis‰ltˆ kauppiaan tyypin mukaan
        if (merchantType == MerchantType.FoodMerchant)
        {
            slider.SetActive(false);
            option3.SetActive(true);
            // Laita kauppiaan nimi oikein
            nameText.text = "Food Merchant";

            // Aseta pudotusvalikkojen sis‰ltˆ
            FillDropdownWithEnum(merchantPanel, "Option1", typeof(P‰‰raaka_aine));
            FillDropdownWithEnum(merchantPanel, "Option2", typeof(Lisuke));
            FillDropdownWithEnum(merchantPanel, "Option3", typeof(Kastike));

            // Oletusateria, ellei pelaaja vaihda aineksia
            valittuP‰‰ = P‰‰raaka_aine.Nautaa;
            valittuLisuke = Lisuke.Perunaa;
            valittuKastike = Kastike.Curry;

            // P‰ivit‰ hinta vastaamaan oletusaterian hintaa
            UpdatePrice();
        }
        else if (merchantType == MerchantType.ArrowMerchant)
        {
            slider.SetActive(true);
            option3.SetActive(false);
            // Jos kauppias onkin nuolikauppias, pit‰‰ pudotusvalikoiden sis‰ltˆ
            // olla erilainen.
            nameText.text = "Arrow Merchant";

            // Aseta pudotusvalikkojen sis‰ltˆ
            FillDropdownWithEnum(merchantPanel, "Option1", typeof(Nuolenk‰rki));
            FillDropdownWithEnum(merchantPanel, "Option2", typeof(NuolenSulka));

            // Oletusnuoli, ellei pelaaja vaihda osia
            valittuK‰rki = Nuolenk‰rki.Puu;
            valittuSulka = Nuolensulka.Lehti;
            nuolenPituus = 0.60f;

            // P‰ivit‰ hinta vastaamaan oletusaterian hintaa
            UpdatePrice();
            // Olisiko siistimp‰‰ tehd‰ nuolikauppiaalle oma skripti?
            // Ei olisi
        }
    }

    /// <summary>
    /// Apufunktio joka etsii dropdown valikon annetulla nimell‰ ja hakee silt‰ Dropdown komponentin
    /// </summary>
    /// <param name="parent">Objekti jonka lapsista etsit‰‰n</param>
    /// <param name="dropdownName">Haetun objektin nimi</param>
    /// <returns></returns>
    TMP_Dropdown GetDropdown(GameObject parent, string dropdownName)
    {
        return parent.transform.Find(dropdownName).GetComponent<TMP_Dropdown>();
    }

    Slider GetSlider(GameObject parent, string sliderName)
    {
        return parent.transform.Find(sliderName).GetComponent<Slider>();
    }
    /// <summary>
    /// Apufunktio joka etsii pudotusvalikon ja laittaa sen vaihtoehdoiksi annetun Enum tyypin nimet
    /// </summary>
    /// <param name="parent">Objekti jonka lapsista pudotusvalikkoa etsit‰‰n</param>
    /// <param name="dropdownName">Pudotusvalikon nimi</param>
    /// <param name="enumType">Enumi jonka nimi‰ k‰ytet‰‰n</param>
    void FillDropdownWithEnum(GameObject parent, string dropdownName, Type enumType)
    {
        TMP_Dropdown choices = parent.transform.Find(dropdownName).GetComponent<TMP_Dropdown>();
        string[] names = Enum.GetNames(enumType);
        choices.ClearOptions();
        choices.AddOptions(names.ToList<string>());
    }

    // N‰it‰ funktioita kutsutaan kun pudotusvalikon valittu arvo muuttuu
    public void OnDropdownValueChanged_1(int newValue)
    {
        if (merchantType == MerchantType.ArrowMerchant)
        {
            TMP_Dropdown dd1 = GetDropdown(merchantPanel, "Option1");
            valittuK‰rki = (Nuolenk‰rki)(dd1.value);
            UpdatePrice();
        }
        else if (merchantType == MerchantType.FoodMerchant)
        {
            TMP_Dropdown dd1 = GetDropdown(merchantPanel, "Option1");
            valittuP‰‰ = (P‰‰raaka_aine)(dd1.value);
            UpdatePrice();
        }
        
    }

    public void OnDropdownValueChanged_2(int newValue)
    {
        if(merchantType == MerchantType.ArrowMerchant)
        {
            TMP_Dropdown dd2 = GetDropdown(merchantPanel, "Option2");
            valittuSulka = (Nuolensulka)(dd2.value);
            UpdatePrice();
        }
        else if (merchantType == MerchantType.FoodMerchant)
        {
            TMP_Dropdown dd2 = GetDropdown(merchantPanel, "Option2");
            valittuLisuke = (Lisuke)(dd2.value);
            UpdatePrice();
        }
        
    }

    public void OnDropDownValueChanged_3(int newValue)
    {
        TMP_Dropdown dd3 = GetDropdown(merchantPanel, "Option3");
        valittuKastike = (Kastike)(dd3.value);
        UpdatePrice();
    }

    public void OnSliderValueChanged_1(float newValue)
    {
        Slider sl1 = GetSlider(merchantPanel, "Slider");
        nuolenPituus = sl1.value;
        arrowLengthText.text = $"{Convert.ToInt32(nuolenPituus*100)} cm";
        isSlider = true;
        UpdatePrice();
    }

    // T‰m‰ funktio p‰ivitt‰‰ n‰kyviss‰ olevan hinnan
    private void UpdatePrice()
    {
        if (!isSlider || isSlider && sliderSoundCooldown <= 0)
        {
            audioManager.PlaySound(AudioManager.SoundEffects.Select);
            sliderSoundCooldown = 0.2f;
        }
        isSlider = false;
        price = 0;
        if (merchantType == MerchantType.FoodMerchant)
        {
            price += valittuP‰‰ switch
            {
                P‰‰raaka_aine.Nautaa => 10,
                P‰‰raaka_aine.Kanaa => 6,
                P‰‰raaka_aine.Kasviksia => 3,
                _ => 0
            };
            price += valittuLisuke switch
            {
                Lisuke.Perunaa => 7,
                Lisuke.Riisi‰ => 3,
                Lisuke.Pastaa => 5,
                _ => 0
            };
            price = valittuKastike switch
            {
                Kastike.Curry => Convert.ToInt32(price*1.1),
                Kastike.Hapanimel‰ => Convert.ToInt32(price * 1.2),
                Kastike.Pippuri => Convert.ToInt32(price * 1.25),
                Kastike.Chili => Convert.ToInt32(price * 1.33),
                _ => 0
            };
        }
        else if (merchantType == MerchantType.ArrowMerchant)
        {
            price += valittuK‰rki switch
            {
                Nuolenk‰rki.Puu => 3,
                Nuolenk‰rki.Ter‰s => 5,
                Nuolenk‰rki.Timantti => 50,
                _ => 0
            };
            price += valittuSulka switch
            {
                Nuolensulka.Lehti => 0,
                Nuolensulka.Kanansulka => 1,
                Nuolensulka.Kotkansulka => 5,
                _ => 0
            };
            price = Convert.ToInt32(price + Mathf.Pow(price,nuolenPituus-0.5f));
        }
        priceText.text = $"Price: {price} c";
    }

    public void OnBuyButtonPressed()
    {
        // TODO v‰henn‰ pelaajalta rahaa ja anna pelaajalle esine tai osumapisteit‰
        if (playerData.money >= price)
        {
            playerData.MuunnaArvoa(-price, "money");
            if (merchantType == MerchantType.ArrowMerchant)
            {
                // Add code here later
            }
            else if (merchantType == MerchantType.FoodMerchant)
            {
                // Increase player's hp depending on the type of food bought
                addedHP = price / 3;
                playerData.MuunnaArvoa(addedHP, "hp");
            }
            audioManager.PlaySound(AudioManager.SoundEffects.BuyItem);
        }
        else
        {
            audioManager.PlaySound(AudioManager.SoundEffects.InvalidAction);
        }
    }

    // Kun pelaaja l‰htee pois kauppiaan luota, piilota k‰yttˆliittym‰
    public void EndShopping()
    {
        if (merchantPanel != null)
        {
            merchantPanel.SetActive(false);
        }
    }
}