using UnityEngine;

public abstract class Jugador : MonoBehaviour
{
   
   public float vida { get; protected set; }
   public float armadura  { get; protected set; }

   public int speed  { get; protected set; }

   public int probCritica  { get; protected set; }

    public abstract void Ataque1();

     //public abstract void Ataque2();
    //public abstract void Ataque3();
    // public abstract void Ataque4();

}