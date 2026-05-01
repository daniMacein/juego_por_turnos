using TMPro;
using UnityEngine;

public class InterfazCombate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    public TextMeshProUGUI    nombre;

    public TextMeshProUGUI   textoBoton;
        public void MostrarMenuPersonaje(Personaje p)

    {
        nombre.text=p.nombre;
        textoBoton.text=p.Ataque1Nombre;
        personaje=p;
    }

    private Personaje personaje;

    public void AlpulsarBoton1()

    {
        personaje.Ataque1();
    }
}
