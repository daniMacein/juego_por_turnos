using UnityEngine;

public class Mago : Personaje
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vida=1000;
        speed=10;
    }
    


    public override void Ataque1()
    {
        print("El mago ataca");
    }
  
}
