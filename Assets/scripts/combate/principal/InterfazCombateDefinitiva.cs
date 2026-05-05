using TMPro;
using UnityEngine;

public class InterfazCombateDefinitiva : MonoBehaviour
{
    public TextMeshProUGUI    nombre;

    public TextMeshProUGUI   textoBoton;
        public void MostrarMenuPersonaje(Personaje p)
        {
            
            if (p == null)
            {
                Debug.LogError("ERROR: Se llamó a MostrarMenuPersonaje con p = NULL");
                return;
            }
            nombre.text=p.nombre;
            textoBoton.text=p.Ataque1Nombre;
            personaje=p;
        }

    private Personaje personaje;

    public void AlpulsarBoton1()
    {
        if (personaje == null)
        {
            Debug.LogError("Se pulsó el botón sin personaje seleccionado");
            return;
        }
        personaje.Ataque1();

    }

    public void AlpulsarBoton2()
    {
        if (personaje == null)
        {
            Debug.LogError("Se pulsó el botón sin personaje seleccionado");
            return;
        }
        //personaje.Ataque2(); Se ponrá el ataque 2 aquí cuando la función este creada 
        Debug.Log("Botón 2 pulsado, pero Ataque2 no implementado aún");
    }

    public void AlpulsarBoton3()
    {
        if (personaje == null)
        {
            Debug.LogError("Se pulsó el botón sin personaje seleccionado");
            return;
        }
        //personaje.Ataque3(); Se ponrá el ataque 3 aquí cuando la función este creada 
        Debug.Log("Botón 3 pulsado, pero Ataque3 no implementado aún");
    }

    public void AlpulsarBoton4()
    {
        if (personaje == null)
        {
            Debug.LogError("Se pulsó el botón sin personaje seleccionado");
            return;
        }
        //personaje.Ataque4(); Se ponrá el ataque 4 aquí cuando la función este creada 
        Debug.Log("Botón 4 pulsado, pero Ataque4 no implementado aún");
    }
}
