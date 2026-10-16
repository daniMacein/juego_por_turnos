using System.Collections.Generic;
using UnityEngine;

//Hace que el modelo "resalte" subiéndole el brillo, sin tocar el material
//(usa MaterialPropertyBlock, así que aunque varios personajes compartan
//material solo se ilumina este).
//Recoge TODAS las mallas del personaje porque los modelos importados
//traen varias partes (cuerpo, ropa, pelo...), cada una con su renderer.
//Funciona solo con los eventos de ratón de Unity: necesita el Collider,
//que tus personajes ya tienen.
public class ResaltadoPersonaje : MonoBehaviour
{
    [Header("Color con el que se tiñe al resaltar")]
    //public Color colorResaltado = new Color(1f, 0.85f, 0.4f, 1f);
    private Color colorResaltado = Color.white;
    //[Range(0f, 1f)] public float intensidadResaltado = 0.6f;

    private float intensidadResaltado=1;

    private class Parte
    {
        public Renderer render;
        public string propiedad;
        public Color colorOriginal;
    }

    private List<Parte> partes = new List<Parte>();
    private MaterialPropertyBlock bloque;

    void Awake()
    {
        bloque = new MaterialPropertyBlock();

        Renderer[] renders = GetComponentsInChildren<Renderer>(true);
        foreach (Renderer r in renders)
        {
            //solo mallas (se ignoran partículas, textos, etc.)
            if (!(r is MeshRenderer) && !(r is SkinnedMeshRenderer))
            {
                continue;
            }
            if (r.sharedMaterial == null)
            {
                continue;
            }

            Material mat = r.sharedMaterial;
            string prop = null;
            if (mat.HasProperty("_BaseColor"))
            {
                prop = "_BaseColor"; //URP Lit
            }
            else if (mat.HasProperty("_Color"))
            {
                prop = "_Color"; //Standard / otros
            }
            if (prop == null)
            {
                continue;
            }

            Parte p = new Parte();
            p.render = r;
            p.propiedad = prop;
            p.colorOriginal = mat.color;
            partes.Add(p);
        }

        if (partes.Count == 0)
        {
            Debug.LogWarning("ResaltadoPersonaje en " + name + ": no se encontró ninguna malla con color que resaltar");
        }
    }

    void OnMouseEnter()
    {
        Resaltar(true);
    }

    void OnMouseExit()
    {
        Resaltar(false);
    }

    //Puedes llamarlo también desde tu lógica (p. ej. solo objetivos válidos)
    public void Resaltar(bool activo)
    {
        foreach (Parte p in partes)
        {
            Color c = p.colorOriginal;
            if (activo)
            {
                c = Color.Lerp(c, colorResaltado, intensidadResaltado);
            }

            p.render.GetPropertyBlock(bloque);
            bloque.SetColor(p.propiedad, c);
            p.render.SetPropertyBlock(bloque);
        }
    }
}
