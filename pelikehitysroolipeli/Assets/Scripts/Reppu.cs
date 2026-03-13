using System.Collections.Generic;
using UnityEngine;

public class Reppu : MonoBehaviour
{
    public int maxTavaraMaara { get; private set; }
    public float maxKantoPaino { get; private set; }
    public float maxTilavuus { get; private set; }

    public float käytettyPaino { get; private set; }
    public float käytettyTilavuus { get; private set; }

    public List<Tavara> tavarat { get; private set; }


    public Reppu(int maxTavaraMaara, float maxKantoPaino, float maxTilavuus)
    {
        this.maxTavaraMaara = maxTavaraMaara;
        this.maxKantoPaino = maxKantoPaino;
        this.maxTilavuus = maxTilavuus;

        tavarat = new List<Tavara>();
    }

    public bool Lisää(Tavara tavara)
    {
        if (tavarat.Count < maxTavaraMaara && käytettyPaino + tavara.paino < maxKantoPaino && käytettyTilavuus + tavara.tilavuus < maxTilavuus)
        {
            tavarat.Add(tavara);
            käytettyPaino += tavara.paino;
            käytettyTilavuus += tavara.tilavuus;
            return true;
        }
        else
        {
            Debug.LogError("Ei mahdu!");
            return false;
        }

    }

    public override string ToString()
    {
        string repunTavarat = "";
        if (tavarat.Count > 0)
        {
            for (int i = 0; i < tavarat.Count - 1; i++)
            {
                repunTavarat += tavarat[i] + ", ";
            }
            repunTavarat += tavarat[tavarat.Count - 1] + ".";
        }
        return $"Repussa on seuraavat tavarat: {repunTavarat}";
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
