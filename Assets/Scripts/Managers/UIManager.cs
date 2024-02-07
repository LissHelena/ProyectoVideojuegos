using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject panelMenuInicio;
    [SerializeField] private GameObject panelMenuGameOver;

    private GameManager gameManager; // Referencia al GameManager

    private void Start()
    {
        // Obtener la instancia del GameManager
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("No se encontró el GameManager en la escena.");
        }
        else
        {
            ActualizarMenu();
        }
    }

    private void ActualizarMenu()
    {
        // Aquí podrías añadir lógica para actualizar el menú si fuera necesario
    }

    private void ActualizarGameOver()
    {
        panelMenuGameOver.SetActive(true);
    }

    public void Jugar()
    {
        panelMenuInicio.SetActive(false);
        gameManager.CambiarEstado(EstadoJuego.Jugando);
    }

    public void Reintentar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void RespuestaCambioEstado(EstadoJuego nuevoEstado)
    {
        if (nuevoEstado == EstadoJuego.GameOver)
        {
            ActualizarGameOver();
        }
    }

    private void OnEnable()
    {
        GameManager.EventoCambioDeEstado += RespuestaCambioEstado;
    }

    private void OnDisable()
    {
        GameManager.EventoCambioDeEstado -= RespuestaCambioEstado;
    }
}
