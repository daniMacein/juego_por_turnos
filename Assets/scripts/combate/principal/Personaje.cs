using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public abstract class Personaje : MonoBehaviour
{

    #region Interfaz

    public string nombre;
    public string clase;
    public string descripcion;

    #endregion


    #region Estadisticas
    public float vidaMaxima { get; protected set; }
    public float vida { get; protected set; } //vida actual
    public float armadura { get; protected set; }
    public int speed { get; protected set; } //velocidad
    public List<int> posicion { get; protected set; } = new List<int>(); //posicion en la ronda actual 

    //*------------------------------------------
    public int probCritico { get; protected set; } //prob de que sea critico el ataque
    public int probEvasion { get; protected set; } //prob de evadir el ataque
    public int probGolpe { get; protected set; } //probabilidad de asestar el ataque
     //*------------------------------------------
    public float potencia { get; protected set; }
    public float alteracionDaño { get; protected set; } //porcentaje que implica cuanto por ciento te entra de mas o de menos en el daño.

    #endregion


    #region Ataques

    public abstract void Ataque1();

    //public abstract void Ataque2();
    //public abstract void Ataque3();
    // public abstract void Ataque4();
    #endregion


    #region Metodos de jugador
    //*probabilidades
    public bool ProbabilidadAcertada(int porcentaje)
    {
        porcentaje = Mathf.Clamp(porcentaje, 0, 100);
        return Random.value < porcentaje / 100f;
    }

    //*Realizar ataques
    public float AplicarDaño(float daño)
    {
        if(ProbabilidadAcertada(probGolpe))
        {
            daño = daño * potencia*alteracionDaño;
        
        }
        else
        {
            daño=0;
        }

        return daño;
        
    }


    #endregion


    #region gets/set

    public void SetPosicion(List <int> posiciones)
    {
        for (int i = 0; i < posiciones.Count; i++)

        {
            this.posicion.Add(posiciones[i]);
        }
    }





    public ResultadoGolpe RecibirGolpe(GolpeData golpe)
    {

         


        vida += golpe.vida; // recuerda: daño es negativo

        Debug.Log(nombre + " recibe " + golpe.vida + " de vida. Vida actual: " + vida);


         ResultadoGolpe resultado = new ResultadoGolpe();


         resultado.dañoFinal=-golpe.vida;
         resultado.armaduraReducida=golpe.vida*-0.1f;

         resultado.estadoGolpe=EstadoGolpe.Golpeado;
         resultado.tiTipoObjetivo=golpe.tipoObjetivo;

         return resultado;

    }

    public void AtaqueRecibido(ResultadoAtaque resultadoAtaque)

    {
        Debug.Log(resultadoAtaque.ToString());
    }
    
    
    
    
}
    #endregion
