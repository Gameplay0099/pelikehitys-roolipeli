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

    // Nuolikauppiaan tarvitsemat enumit, samat kuin tehtävässä 3 Nuolia kaupan
    enum Nuolenkärki
    {
        Puu,
        Teräs,
        Timantti
    }
    enum Nuolensulka
    {
        Lehti,
        Kanansulka,
        Kotkansulka
    }
    // Ateriakauppiaan tarvitsemat enumit, samat kuin tehtävässä 2 Ruoka-annos Generaattori
    enum Pääraaka_aine
    {
        Nautaa,
        Kanaa,
        Kasviksia
    }
    enum Lisuke
    {
        Perunaa,
        Riisiä,
        Pastaa
    }
    enum Kastike
    {
        Curry,
        Hapanimelä,
        Pippuri,
        Chili
    }

    // Nämä muuttujat muistavat pelaajan valinnat ruokakauppiaalta
    Pääraaka_aine valittuPää;
    Lisuke valittuLisuke;
    Kastike valittuKastike;

    // Nämä muuttujat muistavat pelaajan valinnat nuolikauppiaalta
    Nuolenkärki valittuKärki;
    Nuolensulka valittuSulka;

    // Viitteet paneeliin ja tekstielementteihin sekä ostonappiin
    // Näihin otetaan viitteet koska kaikki kauppiaat käyttävät niitä
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

    private void Start()
    {
        // Kun peli alkaa, hae paneeli
        merchantPanel = GameObject.Find("MerchantPanel");

        // Hae paneelista aina käytetyt elementit
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

    /// <summary>
    /// Kutsu tätä funktiota kun pelaaja löytää kauppiaan.
    /// </summary>
    /// <param name="merchantType">Millaisen kauppiaan pelaaja löysi</param>
    public void StartShopping(MerchantType merchantType)
    {
        this.merchantType = merchantType;
        // Laita paneeli ja elementit näkyviin
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

        // Laita elementtien sisältö kauppiaan tyypin mukaan
        if (merchantType == MerchantType.FoodMerchant)
        {
            slider.SetActive(false);
            option3.SetActive(true);
            // Laita kauppiaan nimi oikein
            nameText.text = "Food Merchant";

            // Aseta pudotusvalikkojen sisältö
            FillDropdownWithEnum(merchantPanel, "Option1", typeof(Pääraaka_aine));
            FillDropdownWithEnum(merchantPanel, "Option2", typeof(Lisuke));
            FillDropdownWithEnum(merchantPanel, "Option3", typeof(Kastike));

            // Oletusateria, ellei pelaaja vaihda aineksia
            valittuPää = Pääraaka_aine.Nautaa;
            valittuLisuke = Lisuke.Perunaa;
            valittuKastike = Kastike.Curry;

            // Päivitä hinta vastaamaan oletusaterian hintaa
            UpdatePrice();
        }
        else if (merchantType == MerchantType.ArrowMerchant)
        {
            slider.SetActive(true);
            option3.SetActive(false);
            // Jos kauppias onkin nuolikauppias, pitää pudotusvalikoiden sisältö
            // olla erilainen.
            nameText.text = "Arrow Merchant";

            // Aseta pudotusvalikkojen sisältö
            FillDropdownWithEnum(merchantPanel, "Option1", typeof(Nuolenkärki));
            FillDropdownWithEnum(merchantPanel, "Option2", typeof(NuolenSulka));

            // Oletusnuoli, ellei pelaaja vaihda osia
            valittuKärki = Nuolenkärki.Puu;
            valittuSulka = Nuolensulka.Lehti;
            nuolenPituus = 60;

            // Päivitä hinta vastaamaan oletusaterian hintaa
            UpdatePrice();
            // Olisiko siistimpää tehdä nuolikauppiaalle oma skripti?
            // Ei olisi
        }
    }

    /// <summary>
    /// Apufunktio joka etsii dropdown valikon annetulla nimellä ja hakee siltä Dropdown komponentin
    /// </summary>
    /// <param name="parent">Objekti jonka lapsista etsitään</param>
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
    /// <param name="parent">Objekti jonka lapsista pudotusvalikkoa etsitään</param>
    /// <param name="dropdownName">Pudotusvalikon nimi</param>
    /// <param name="enumType">Enumi jonka nimiä käytetään</param>
    void FillDropdownWithEnum(GameObject parent, string dropdownName, Type enumType)
    {
        TMP_Dropdown choices = parent.transform.Find(dropdownName).GetComponent<TMP_Dropdown>();
        string[] names = Enum.GetNames(enumType);
        choices.ClearOptions();
        choices.AddOptions(names.ToList<string>());
    }

    // Näitä funktioita kutsutaan kun pudotusvalikon valittu arvo muuttuu
    public void OnDropdownValueChanged_1(int newValue)
    {
        if (merchantType == MerchantType.ArrowMerchant)
        {
            TMP_Dropdown dd1 = GetDropdown(merchantPanel, "Option1");
            valittuKärki = (Nuolenkärki)(dd1.value);
            UpdatePrice();
        }
        else if (merchantType == MerchantType.FoodMerchant)
        {
            TMP_Dropdown dd1 = GetDropdown(merchantPanel, "Option1");
            valittuPää = (Pääraaka_aine)(dd1.value);
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
        UpdatePrice();
    }

    // Tämä funktio päivittää näkyvissä olevan hinnan
    private void UpdatePrice()
    {
        price = 0;
        if (merchantType == MerchantType.FoodMerchant)
        {
            price += valittuPää switch
            {
                Pääraaka_aine.Nautaa => 10,
                Pääraaka_aine.Kanaa => 5,
                Pääraaka_aine.Kasviksia => 3,
                _ => 0
            };
            price += valittuLisuke switch
            {
                Lisuke.Perunaa => 7,
                Lisuke.Riisiä => 2,
                Lisuke.Pastaa => 5,
                _ => 0
            };
            price += valittuKastike switch
            {
                Kastike.Curry => 3,
                Kastike.Hapanimelä => 2,
                Kastike.Pippuri => 4,
                Kastike.Chili => 1,
                _ => 0
            };
        }
        else if (merchantType == MerchantType.ArrowMerchant)
        {
            price += valittuKärki switch
            {
                Nuolenkärki.Puu => 3,
                Nuolenkärki.Teräs => 5,
                Nuolenkärki.Timantti => 50,
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
        // TODO vähennä pelaajalta rahaa ja anna pelaajalle esine tai osumapisteitä
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
                addedHP = price / 4;
                playerData.MuunnaArvoa(addedHP, "hp");
            }
        }
        else
        {
            
        }
    }

    // Kun pelaaja lähtee pois kauppiaan luota, piilota käyttöliittymä
    public void EndShopping()
    {
        if (merchantPanel != null)
        {
            merchantPanel.SetActive(false);
        }
    }
}