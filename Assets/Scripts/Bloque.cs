using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoBloque
{
    Normal,
    FullVagones,
    Trenes
}

public class Bloque : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private TipoBloque tipoBloque;
    [SerializeField] private bool tieneRampa;

    [Header("Tren")]
    [SerializeField] private Tren[] trenes;

    public TipoBloque TipoDeBloque => tipoBloque;
    public bool TieneRampa => tieneRampa;

    public void InicializarBloque()
    {
        if(tipoBloque == TipoBloque.Trenes)
        {
            SeleccionTren();
        }
    }

    private void SeleccionTren()
    {
        if(trenes == null || trenes.Length == 0)
        {
            return;
        }
        int index = Random.Range(0, trenes.Length);
        trenes[index].gameObject.SetActive(true);
    }
}
