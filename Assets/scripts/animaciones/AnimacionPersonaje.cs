using System.Collections;
using UnityEngine;

public class AnimacionPersonaje : MonoBehaviour
{
    [SerializeField] public GameObject personaje;
    [SerializeField] public int numAnimacion; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numAnimacion = 0; 
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
        animator.Play("Golpeado");
        Debug.Log("Animacion golpeado");
        yield return new WaitForSeconds(1f);
    
    }

}
