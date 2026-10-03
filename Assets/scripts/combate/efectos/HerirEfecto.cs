using UnityEngine;

public class HerirEfecto : Efecto
{

    private float herir=4;
    public override void AlAplicarse(Personaje objetivo, float potencia, float critico )
    {
        duracion=10;
        herir=potencia*critico*herir;
        Debug.Log("Aplicado efecto");

    }
}
