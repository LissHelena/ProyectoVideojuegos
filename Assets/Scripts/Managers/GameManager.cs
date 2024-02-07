using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EstadoJuego
{
    Inicio,
    Jugando,
    GameOver
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject menuInicio;
    [SerializeField] private GameObject pantallaGameOver;

    public static GameManager Instancia { get; private set; }
    public static event Action<EstadoJuego> EventoCambioDeEstado;

    public EstadoJuego EstadoActual { get; private set; }

    private void Start()
    {
        Instancia = this;
        EstadoActual = EstadoJuego.Inicio;
        MostrarMenuInicio();
    }

    public void IniciarJuego()
    {
        EstadoActual = EstadoJuego.Jugando;
        OcultarMenuInicio();
    }

    public void MostrarMenuInicio()
    {
        EstadoActual = EstadoJuego.Inicio;
        menuInicio.SetActive(true);
        pantallaGameOver.SetActive(false);
    }

    public void MostrarGameOver()
    {
        EstadoActual = EstadoJuego.GameOver;
        pantallaGameOver.SetActive(true);
    }

    public void ReiniciarJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OcultarMenuInicio()
    {
        menuInicio.SetActive(false);
    }

    public void CambiarEstado(EstadoJuego nuevoEstado)
    {
        if (EstadoActual != nuevoEstado)
        {
            EstadoActual = nuevoEstado;
            EventoCambioDeEstado?.Invoke(nuevoEstado);
        }
    }
}
