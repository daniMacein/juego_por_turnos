using UnityEngine;

public class HerirEfecto : Efecto
{

    private float herir=50;
    public override void AlAplicarse(Personaje objetivo, float potencia, float critico )
    {
        duracion=10;
        this.objetivo = objetivo;
        afectaEnTurnoGeneral=true; 
        herir=potencia*critico*herir;
       

    }

    public override EfectoData EnTurnoGeneral(Personaje personaje)
    {
       
        EfectoData efecto = new EfectoData(herir,objetivo,TipoEfecto.Daño,TipoAnimacion.Ataque1_Golpe1);
        return efecto; 
    }


}
