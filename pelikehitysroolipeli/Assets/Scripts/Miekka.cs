using UnityEngine;

public class Miekka : Tavara
{
    public Miekka() : base(5.0f, 3.0f) { }

    public override string ToString()
    {
        return $"Miekka";
    }
}
