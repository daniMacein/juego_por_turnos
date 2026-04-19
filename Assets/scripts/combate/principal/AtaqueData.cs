using UnityEngine;

public class AtaqueData : MonoBehaviour
{

    //*Personas
    public Personaje emisor;
    public Personaje[] receptores;

    //* Tipo de ataque
    public bool positivo;
    public bool soyUnEfecto;
    public int numGolpes;

    public TipoAtaque[] tipoAtaque;


    public bool tieneEfectos;

    //*Ataque
    public float[] vida;
    public float[] vidaArmadura;



    //* Informacion post ataque
    public float dañoRecibido;
    public float armaduraQuitada;




    //* Estado
    public enum AtaqueEstado
    {
        Previo,
        Atacando,
        Invocando,
        golpeado,
        Fallado,
        Persuadido
    }

    AtaqueEstado[] ataqueEstado;



    //*Efectos


    public int[] indiceEfectos;

    public Efecto[] efectos;

    public float[] vidaTotalEfectos;

    public int[] aturdimientos;



    //*Estadisticas

    public float escudo;
    public float armaduraEfecto;

    public int speed;

    //*------------------------------------------
    public int probCritico;

    public int probEvasion;

    public int probGolpe;
    //*------------------------------------------
    public float potencia;
    public float alteracionDaño;

}
