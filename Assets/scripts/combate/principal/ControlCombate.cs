using UnityEngine;

public class ControlCombate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public Personaje jugador1;

      public Personaje jugador2;
    void Start()
    {
        
        jugador1.Ataque1();
         jugador2.Ataque1();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
