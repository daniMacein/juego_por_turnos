using UnityEngine;
using System.Collections.Generic;
public class SelectorData 
{
    
     public List<Equipo> equipos;

     public int numeroObjetivos;


     public SelectorData(int numeroObjetivos, params Equipo[] equipos)
    {
        this.numeroObjetivos=numeroObjetivos;
      this.equipos= new List<Equipo>(equipos);
    }


}
