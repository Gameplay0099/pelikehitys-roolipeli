using UnityEngine;

public class Tavara : MonoBehaviour
{
    [SerializeField] public float paino { get; private set; }
    [SerializeField] public float tilavuus { get; private set; }

    public Tavara(float paino, float tilavuus)
    {
        this.paino = paino;
        this.tilavuus = tilavuus;
    }
}
