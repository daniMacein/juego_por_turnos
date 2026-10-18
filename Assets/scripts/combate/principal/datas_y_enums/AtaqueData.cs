using UnityEngine;
using System.Collections.Generic;
public class AtaqueData 
{

   public List<GolpeData> golpes;

  public Personaje meleeObjetivo;

   public bool seDesplaza=false;


public void EstablecerObjetivoMelee(Personaje personaje)
    {
        meleeObjetivo=personaje;
        
        seDesplaza=true;
    }

public AtaqueData(params GolpeData[] golpes)
{
    this.golpes = new List<GolpeData>(golpes);
}

}
