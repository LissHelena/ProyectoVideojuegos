using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MostrarPanelHistoria : MonoBehaviour
{
    [SerializeField] private GameObject panelHistoria;

    public void MostrarHistoria()
    {
        if (panelHistoria != null)
        {
            panelHistoria.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Panel de historia no asignado en el inspector.");
        }
    }
}
