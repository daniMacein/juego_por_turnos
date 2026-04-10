using UnityEngine;

public class Mago : Jugador
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vida=1000;
        speed=45;
    }
    


    public override void Ataque1()
    {
        print("El mago ataca");
    }
  
}
