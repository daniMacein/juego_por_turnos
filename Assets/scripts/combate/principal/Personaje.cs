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



    #region gets/set

    public void SetPosicion(List <int> posiciones)
    {
        for (int i = 0; i < posiciones.Count; i++)

        {
            this.posicion.Add(posiciones[i]);
        }
    }


  #endregion
    #region Metodos
    //*formulas

    

public bool ProbabilidadAcertada(int porcentaje)
{
    porcentaje = Mathf.Clamp(porcentaje, 0, 100);

    float prob = porcentaje / 100f;


    return Random.value < prob;
}

private float DañoReducidoPorArmadura(float daño)
    {
        return daño*(100/(100+armadura));
    }

private float DesgasteArmadura(float dañoArmadura)
    {
        float armaduraAntigua=armadura;
        armadura=armadura-((dañoArmadura*armadura/50)/100);
        return armaduraAntigua-armadura;
    }

    //todo: Metodos para infligir daño
    //Aplicar daño que vAS A INFLIGIR
    public GolpeData AplicarEstadisticasAGolpe(GolpeData golpe)
    {
        
          GolpeData nuevo = new GolpeData(
        -golpe.vida,
        golpe.objetivos,
        golpe.tipoAtaque,
        golpe.tipoObjetivo
    );
    nuevo.armadura=golpe.armadura;

        if(ProbabilidadAcertada(probGolpe))
        {
            nuevo.vida = nuevo.vida * potencia*alteracionDaño;
            nuevo.armadura=nuevo.armadura * potencia * alteracionDaño;
        }
        else
        {
             nuevo.vida=0;
             nuevo.estadoGolpe=EstadoGolpe.Fallado;
        }

        return nuevo;
        
    }



   //todo: Metodos para recibir daño

    private float RecibirDaño(float dañoRecibido)
    {
        return DañoReducidoPorArmadura(dañoRecibido*alteracionDaño);
    }
    public ResultadoGolpe RecibirGolpe(GolpeData golpe)
    {
         // recuerda: daño es negativo
        float dañoRecibido=RecibirDaño(golpe.vida);

        vida += dañoRecibido;
        float armaduraGastada=DesgasteArmadura(dañoRecibido);

         ResultadoGolpe resultado = new ResultadoGolpe();


         resultado.dañoFinal=-dañoRecibido;
         resultado.armaduraReducida=armaduraGastada;

         resultado.estadoGolpe=EstadoGolpe.Golpeado;
         resultado.tiTipoObjetivo=golpe.tipoObjetivo;

         return resultado;

    }

    //Despues de haber recibido un ataque completo. Se indica que el ataque ha sido finalizado/ejecutado
    //y se consigue cuantos daño y armadura total ha sido quitada.
    public void AtaqueRecibido(ResultadoAtaque resultadoAtaque)

    {
        Debug.Log(resultadoAtaque.ToString());
    }
    
    
    
    
}
  
#endregion