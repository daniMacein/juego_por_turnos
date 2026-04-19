using UnityEngine;

public class SeñorPirotecnic : Personaje
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vidaMaxima=860;
        vida=860;
        armadura=30;
        speed=28;
        
        probCritico=8;
        probEvasion=4;

        potencia=1;
        probGolpe=50;
        alteracionDaño=1;
        
    }
    


    public override void Ataque1()
    {
       float daño=AplicarDaño(100);
       print("He atacado: "+daño);

    }
}
