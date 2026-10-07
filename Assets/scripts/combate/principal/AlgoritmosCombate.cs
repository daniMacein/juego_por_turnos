using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class AlgoritmosCombate : MonoBehaviour
{


public List<Personaje> equipoA = new List<Personaje>();
public List<Personaje> equipoB = new List<Personaje>();
public List<Personaje> equi = new List<Personaje>();

public List<Personaje> OrdenarPersonajesVidas( Equipo equipo)
    {
        
    equipoA=ControlCombate.controlCombate.equipoA.ToList();
    equipoB=ControlCombate.controlCombate.equipoB.ToList();

    if (equipo == Equipo.equipoA)
        {
            equipoA.Sort((p1, p2) => p1.porcentajeVida.CompareTo(p2.porcentajeVida));
            equi=equipoA;
        }
    else
        {
            equipoB.Sort((p1, p2) => p1.porcentajeVida.CompareTo(p2.porcentajeVida));
            equi=equipoB;
        }
        
        return equi;
    }




}
