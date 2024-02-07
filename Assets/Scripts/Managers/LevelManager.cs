using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int bloquesInicio = 5;
    [SerializeField] private int bloquesFullVagones = 5;
    [SerializeField] private int bloquesTrenes = 10;
    [SerializeField] private int bloquesTrenesReset = 2;

    [Header("Bloques")]
    [SerializeField] private Bloque bloqueInicial;
    [SerializeField] private int longitudBloqueNormal = 40;
    [SerializeField] private int longitudBloqueTrenes = 80;
    [SerializeField] private Bloque[] bloquePrefab;

    private List<Bloque> listaBloquesNormales = new List<Bloque>();
    private List<Bloque> listaBloquesFullVagones = new List<Bloque>();
    private List<Bloque> listaBloquesTrenes = new List<Bloque>();
    private List<Bloque> listaBloquesRampa = new List<Bloque>();

    private Pooler pooler;
    private Bloque ultimoBloque;
    private int bloquesCreados;

    private void Awake()
    {
        pooler = GetComponent<Pooler>();
    }

    void Start()
    {
        LlenarBloqueSegunTipo();
        ultimoBloque = bloqueInicial;
        
        for (int i = 0; i < bloquesInicio; i++)
        {
            CrearBloque();
        }
    }

    private void CrearBloque()
    {
        if(bloquesCreados >= bloquesTrenes)
        {
            if(bloquesCreados < bloquesTrenes + 1)
            {
                AgregarBloque(TipoBloque.Trenes, longitudBloqueNormal);
            }
            else
            {
                AgregarBloque(TipoBloque.Trenes, longitudBloqueTrenes);
            }
            if (bloquesCreados == bloquesTrenes + bloquesTrenesReset)
            {
                bloquesCreados = 0;
            }
        }
        else if (bloquesCreados >= bloquesFullVagones)
        {
             AgregarBloque(TipoBloque.FullVagones, longitudBloqueNormal);
        }
        else
        {
            if (bloquesCreados == bloquesFullVagones - 1)
            {
                AgregarBloque(TipoBloque.Normal, longitudBloqueNormal, true);
            }
            else
            {
                if (ultimoBloque.TipoDeBloque == TipoBloque.Trenes)
                {
                    AgregarBloque(TipoBloque.Normal, longitudBloqueTrenes);
                }
                else
                {
                    AgregarBloque(TipoBloque.Normal, longitudBloqueNormal);
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            CrearBloque();
        }
    }
    private void AgregarBloque(TipoBloque tipo, float longitud, bool conRampa = false)
    {
        Bloque nuevoBloque = ObtenerBloqueSegunTipo(tipo, conRampa);
        nuevoBloque.transform.position = PosNuevoBloque(longitud);
        ultimoBloque = nuevoBloque;
        bloquesCreados++;
    }

    private Bloque ObtenerBloqueSegunTipo(TipoBloque tipo, bool conRampa = false)
    {
        Bloque nuevoBloque = null;
        if (conRampa)
        {
            nuevoBloque = ObtenerInstPooler(listaBloquesRampa);
        }
        else
        {
            switch(tipo)
            {
                case TipoBloque.Normal:
                    nuevoBloque = ObtenerInstPooler(listaBloquesNormales);
                    break;
                case TipoBloque.FullVagones:
                    nuevoBloque = ObtenerInstPooler(listaBloquesFullVagones);
                    break;
                case TipoBloque.Trenes:
                    nuevoBloque = ObtenerInstPooler(listaBloquesTrenes);
                    break;
            }
        }
        if(nuevoBloque != null)
        {
            nuevoBloque.InicializarBloque();
        }
        return nuevoBloque;
    }

    private Bloque ObtenerInstPooler(List<Bloque> lista)
    {
        int bloqueRandom = Random.Range(0, lista.Count);
        string nombreBloque = lista[bloqueRandom].name;
        GameObject instancia = pooler.ObtenerInstPooler(nombreBloque);
        instancia.SetActive(true);
        Bloque bloque = instancia.GetComponent<Bloque>();
        return bloque;
    }

    private Vector3 PosNuevoBloque(float longitud)
    {
        return ultimoBloque.transform.position + Vector3.forward * longitud;
    }

    private void LlenarBloqueSegunTipo()
    {
        foreach(Bloque bloque in bloquePrefab)
        {
            switch(bloque.TipoDeBloque) 
            {
                case TipoBloque.Normal:
                    listaBloquesNormales.Add(bloque);
                    if (bloque.TieneRampa)
                    {
                        listaBloquesRampa.Add(bloque);
                    }
                    break;
                case TipoBloque.FullVagones:
                    listaBloquesFullVagones.Add(bloque);
                    break;
                case TipoBloque.Trenes:
                    listaBloquesTrenes.Add(bloque);
                    break;
            }
        }
    }

    private void RespuestaNuevoBloque()
    {
        CrearBloque();
    }

    private void OnEnable()
    {
        Limite.EventoNuevoBloque += RespuestaNuevoBloque;
    }

    private void OnDisable()
    {
        Limite.EventoNuevoBloque -= RespuestaNuevoBloque;
    }


}
