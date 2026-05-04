using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using System.Collections;
using System;
public abstract class Personaje : MonoBehaviour
{



    #region Interfaz

    public string nombre;
    public string clase;
    public string descripcion;

    public string Ataque1Nombre;

    #endregion

    #region Interaccion
    public SelectorPersonaje selector;
    public ControlCombate controlCombate;
    public List<Efecto> efectosActivos = new List<Efecto>();
    public Equipo equipo;



    public bool haTerminadoElAtaque;
    public AtaqueData ataqueElegido;
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

    public virtual IEnumerator AnimarAtaque(GolpeData golpeData, List<Personaje> objetivosFinales)

    {
        //aqui miras cual es el golpe, porque tendra un identificador dentro de golpe y haces la animacion
        //correspondiente
        yield return new WaitForSeconds(1f);
    }

    #region gets/set

    public void SetPosicion(List<int> posiciones)
    {
        posicion.Clear();
        for (int i = 0; i < posiciones.Count; i++)

        {
            this.posicion.Add(posiciones[i]);
        }
    }

    void setvida(float vidaRecibida)
    {



        if ((vida + vidaRecibida) > vidaMaxima)

        {
            vida = vidaMaxima;
        }
        else if ((vida + vidaRecibida) <= 0)
        {
            vida = 0;
        }
        else
        {
            vida += vidaRecibida;
        }

        if (vida == 0)
        {
            Debug.Log("me morís");
        }
    }


    #endregion
    #region Metodos
    //*formulas

    //todo: Formulas de utilidad
    //Formulas que sirve para calcular cosas de forma mas cómoda


    //Le metes un numero y te devuelve true o false. 
    //Si le metes 50, tendra un 50% de devolverte true. Un 100% siempre te dara true. 0 siempre false
    public bool ProbabilidadAcertada(int porcentaje)
    {
        porcentaje = Mathf.Clamp(porcentaje, 0, 100);

        float prob = porcentaje / 100f;


        return UnityEngine.Random.value < prob;
    }

    //todo: Formulas de estadisticas

    private float DañoReducidoPorArmadura(float daño)
    {
        return daño * (100 / (100 + armadura));
    }

    private float DesgasteArmadura(float dañoArmadura)
    {
        float armaduraAntigua = armadura;
        armadura = armadura - ((dañoArmadura * armadura / 50) / 100);

        if (armadura < 0)
        {
            armadura = 0;
        }
        return armaduraAntigua - armadura;
    }

    //todo: Metodos para infligir 
    //(utilizados para infligir ataques y efectos)


    //Aplicar a tu daño todo lo que hay que aplicar
    public GolpeData AplicarEstadisticasAGolpe(GolpeData golpe)
    {
        float vidaUsada = golpe.vida;
        if (golpe.esPositivo == false)
        {
            vidaUsada = -vidaUsada;
        }
        GolpeData nuevo = new GolpeData(
      vidaUsada,
      golpe.objetivos,
      golpe.tipoAtaque,
      golpe.tipoObjetivo,
      golpe.tipoAnimacion
  );
        nuevo.armadura = golpe.armadura;

        if (ProbabilidadAcertada(probGolpe))
        {
            nuevo.vida = nuevo.vida * potencia;
            nuevo.armadura = nuevo.armadura * potencia;
        }
        else
        {
            nuevo.vida = 0;
            nuevo.estadoGolpe = EstadoGolpe.Fallado;
        }

        return nuevo;

    }


    //Indico que he terminado un ataque
    public virtual void TerminarAtaque(AtaqueData ataqueData)
    {
        haTerminadoElAtaque = true;
    }


    //todo: Metodos para recibir

    //aplicar todo lo que hay que aplicar al recibir daño
    private float RecibirDaño(float daño, bool ignoraArmadura)

    {

        float nuevoDaño = daño * alteracionDaño;
        if (ignoraArmadura == false)
        {
            nuevoDaño = DañoReducidoPorArmadura(nuevoDaño);
        }
        return nuevoDaño;
    }

    private float GastarArmadura(float desgaste)
    {
        return DesgasteArmadura(desgaste * alteracionDaño);
    }

    public ResultadoGolpe RecibirGolpe(GolpeData golpe)
    {
        // recuerda: daño es negativo
        float vidaRecibida = 0;
        float armaduraGastada = 0;
        if (golpe.esPositivo == false)
        {
            vidaRecibida = RecibirDaño(golpe.vida, golpe.ignoraArmadura);
            setvida(vidaRecibida);
            armaduraGastada = GastarArmadura(golpe.armadura);
        }
        else
        {
            setvida(golpe.vida);
            vidaRecibida = golpe.vida;
        }

        ResultadoGolpe resultado = new ResultadoGolpe
        (vidaRecibida, armaduraGastada, EstadoGolpe.Golpeado, golpe.tipoObjetivo, golpe.tipoAtaque);
        AnimacionRecibirGolpe(resultado);

        return resultado;

    }

    private void AnimacionRecibirGolpe(ResultadoGolpe resultadoGolpe)
    {
        Debug.Log(nombre + " golpeado con " + resultadoGolpe.dañoFinal.ToString("F0") + "p de vida");
    }

    //Despues de haber recibido un ataque completo. Se indica que el ataque ha sido finalizado/ejecutado
    //y se consigue cuantos daño y armadura total ha sido quitada.
    public virtual void AtaqueRecibido(ResultadoAtaque resultadoAtaque)

    {
        Debug.Log(nombre + ": vida actual: " + vida.ToString("F0") + "/" + vidaMaxima + "\n" + "armadura:" + armadura.ToString("F0"));

    }



    //todo: EFECTOS


    public void AplicarEfecto(Efecto efecto)
    {
        efectosActivos.Add(efecto);
        efecto.AlAplicarse(this);
    }


    public void EjecutarInicioDeRonda()
    {
        foreach (Efecto efecto in efectosActivos)
        {
            efecto.InicioDeRonda(this);
        }
    }


    public void LimpiarEfectos()
    {
        for (int i = efectosActivos.Count - 1; i >= 0; i--)
        {
            if (efectosActivos[i].disipado)
            {
                efectosActivos.RemoveAt(i);
            }
        }
    }

    //! tostring
    public override string ToString()
    {
        string resultado = "";
        resultado += "Personaje" + "\n";
        resultado += "nombre: " + nombre + "\n";
        resultado += "vida actual: " + vida + "\n";
        resultado += "armadura: " + armadura + "\n";

        resultado += "equipo: " + equipo + "\n";
        resultado += "posicion: " + string.Join(", ", posicion) + "\n";


        return resultado;
    }
}

    #endregion