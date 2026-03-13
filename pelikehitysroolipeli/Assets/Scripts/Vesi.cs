using UnityEngine;

public class Vesi : Tavara
{
    public Vesi() : base(2.0f, 2.0f) { }

    public override string ToString()
    {
        return $"Vesi";
    }
}
