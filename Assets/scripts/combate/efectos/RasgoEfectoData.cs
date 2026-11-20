using System.Collections.Generic;
public class RasgoEfectoData
{

    public float valorOriginal;
    public TipoStat tipoStat; // daño, potencia etc
    public TiempoRasgo tiempoRasgo;  // turno actual, ronda 
   
    public int duracionOriginal; //duracion original
    public TipoAnimacion tipoAnimacion; // Animacion del efecto

    public List<StackEfecto> stacks = new List<StackEfecto>(); //x todos los stacks acumulados


    public RasgoEfectoData(float valor, TipoStat tipoStat, TiempoRasgo tiempoRasgo, int duracion, TipoAnimacion tipoAnimacion)
    {


        this.valorOriginal = valor;
        this.duracionOriginal = duracion;

        this.tipoStat = tipoStat;
        this.tiempoRasgo = tiempoRasgo;
       

        this.tipoAnimacion = tipoAnimacion;

        StackEfecto nuevoRasgo = new StackEfecto(valor,duracion);
        stacks.Add(nuevoRasgo);
        

    }


    public void anadirNuevosStack(float valor)
    {
        StackEfecto nuevoRasgo = new StackEfecto(valor,duracionOriginal);
        stacks.Add(nuevoRasgo);
    }


    public void AnadirTiempoInicial(TiempoRasgo tiempoRasgo, int tiempoInicial)
    {

    }



    public void AnadirCooldown(int cooldown)
    {

    }



}
