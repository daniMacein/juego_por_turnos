using System.Collections; 
using UnityEngine;

public class AnimarPersonajes : MonoBehaviour
{
    [SerializeField] public GameObject personaje;
    [SerializeField] public int numAnimacion; 
    [SerializeField] private bool derrotaAnimada; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numAnimacion = 0; 
        derrotaAnimada = false; 
        Animator animator = personaje.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (numAnimacion == 0)
        {
            StartCoroutine(AnimacionIdle());
        }
        else if (numAnimacion == 1)
        {
            StartCoroutine(AnimacionGolpeado());
        }
        else if (numAnimacion == 2)
        {
            StartCoroutine(AnimacionAtaqueCerca());
        }
         else if (numAnimacion == 3)
        {
            StartCoroutine(AnimacionAtaqueLejos());
        }
         else if (numAnimacion == 4)
        {
            StartCoroutine(AnimacionDerrota());
        }
    }

    public virtual IEnumerator AnimacionIdle()
    {
        Animator animator = personaje.GetComponent<Animator>();
        animator.Play("Idle");
        Debug.Log("Animacion idle");
        yield return new WaitForSeconds(1f);
    
    }

    public virtual IEnumerator AnimacionGolpeado()
    {
        Animator animator = personaje.GetComponent<Animator>();
        animator.Play("Defeat");
        Debug.Log("Animacion golpeado");
        yield return new WaitForSeconds(1f);
    
    }

    public virtual IEnumerator AnimacionAtaqueCerca()
    {
        Animator animator = personaje.GetComponent<Animator>();
        animator.Play("AtaqueCerca");
        Debug.Log("Animacion ataque de cerca");
        yield return new WaitForSeconds(1f);
    
    }

     public virtual IEnumerator AnimacionAtaqueLejos()
    {
        Animator animator = personaje.GetComponent<Animator>();
        animator.Play("AtaqueLejos");
        Debug.Log("Animacion ataque de lejos");
        yield return new WaitForSeconds(3f);
    
    }

    public virtual IEnumerator AnimacionDerrota()
    {
         if(!derrotaAnimada){
            derrotaAnimada = true;
            Animator animator = personaje.GetComponent<Animator>();
            animator.Play("Derrota");
            Debug.Log("Animacion de derrota");
        }
        yield return new WaitForSeconds(3f);
    
    }
}
