using System.Collections.Generic;
using System.Linq;
public class RasgoEfectoData
{

    public float valorOriginal;
    public TipoStat tipoStat; // daño, potencia etc
    public TiempoRasgo tiempoRasgo;  // turno actual, ronda 
   
    public int duracionOriginal; //duracion original
    public TipoAnimacion tipoAnimacion; // Animacion del efecto

    public List<StackEfecto> stacks = new List<StackEfecto>(); //x todos los stacks acumulados

    public int cooldown=1;
    public int cooldownActual=1;
   

   
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


    public void AnadirNuevosStack(float valor)
    {
        StackEfecto nuevoRasgo = new StackEfecto(valor,duracionOriginal);
        stacks.Add(nuevoRasgo);
    }


    public void AnadirTiempoInicial(TiempoRasgo tiempoRasgo, int tiempoInicial)
    {

    }



    public void AnadirCooldown(int cooldown)
    {
        this.cooldown=cooldown;
        
        
    }

    public void ReducirDuracion()
    {
        foreach (StackEfecto stackEfecto in stacks.ToList())
        {
            stackEfecto.duracion=stackEfecto.duracion-1;
            if (stackEfecto.duracion==0)
            {
                stacks.Remove(stackEfecto);
            }
        }
        
    }




}
