using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum DirInput
{
    Null,
    Arriba,
    Izquierda,
    Derecha,
    Abajo
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [Header("Config")]
    [SerializeField] private float velMov;
    [SerializeField] private float valSalto = 15f;
    [SerializeField] private float gravedad = 20f;

    [Header("Carril")]
    [SerializeField] private float posCarrilIzq = -3.1f;
    [SerializeField] private float posCarrilDer = 3.1f;

    public bool EstaSaltando { get; private set; }
    public bool EstaDeslizando { get; private set; }

    private DirInput direccionInput;
    private Coroutine coroutineDeslizar;
    private CharacterController characterController;
    private PlayerAnimaciones playerAnimaciones;
    private float posVertical;
    private int carrilAct;
    private Vector3 dirDeseada;

    private float controllerRadio;
    private float controllerAltura;
    private float controllerPosicionY;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimaciones = GetComponent<PlayerAnimaciones>();
    }

    // Start is called before the first frame update
    void Start()
    {
        controllerRadio = characterController.radius;
        controllerAltura = characterController.height;
        controllerPosicionY = characterController.center.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.EstadoActual == EstadoJuego.Inicio ||
            gameManager.EstadoActual == EstadoJuego.GameOver)
        {
            return;
        }
        DetectarInput();
        ControlarCarriles();
        CalMovVertical();
        MoverPersonaje();
    }

    private void MoverPersonaje()
    {
        Vector3 nuevaPos = new Vector3(dirDeseada.x, posVertical, velMov);
        characterController.Move(nuevaPos*Time.deltaTime);
    }

    private void CalMovVertical()
    {
        //Logica del Salto
        if (characterController.isGrounded)
        {
            EstaSaltando = false;
            posVertical = 0f;

            if(EstaDeslizando == false && EstaSaltando == false)
            {
                playerAnimaciones.AnimCorrer();
            }

            if(direccionInput == DirInput.Arriba)
            {
                posVertical = valSalto;
                EstaSaltando = true;
                playerAnimaciones.AnimSaltar();
                if(coroutineDeslizar != null)
                {
                    StopCoroutine(coroutineDeslizar);
                    EstaDeslizando = false;
                    CollaiderDeslizar(false);
                }
            }//Logica de Deslizar
            if(direccionInput == DirInput.Abajo)
            {
                if (EstaDeslizando)
                {
                    return;
                }
                if(coroutineDeslizar != null)
                {
                    StopCoroutine(coroutineDeslizar);
                }
                DeslPers();
            }
        }
        else
        {
            if(direccionInput == DirInput.Abajo)
            {
                posVertical -= valSalto;
                DeslPers();
            }
        }

        posVertical -= gravedad * Time.deltaTime;
    }

    private void ControlarCarriles()
    {
        switch (carrilAct)
        {
            case -1:
                //Mover izq
                MoverHorizontal(posCarrilIzq, Vector3.left);
                break;
            case 0:
                //Mover centro
                if(transform.position.x > 0.1f)
                {
                    MoverHorizontal(0f, Vector3.left);
                }
                else if(transform.position.x < -0.1f)
                {
                    MoverHorizontal(0f, Vector3.right);
                }
                else
                {
                    dirDeseada = Vector3.zero;
                }
                break;
            case 1:
                //Mover der
                MoverHorizontal(posCarrilDer, Vector3.right);
                break;
        }
    }

    private void MoverHorizontal(float posX, Vector3 dirMov)
    {
        float posHorizontal = Mathf.Abs(transform.position.x - posX);
        if (posHorizontal > 0.1f)
        {
            dirDeseada = Vector3.Lerp(dirDeseada, dirMov*20f, Time.deltaTime*500f);
        }
        else
        {
            dirDeseada = Vector3.zero;
            transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        }
    }

    private void DeslPers()
    {
        coroutineDeslizar = StartCoroutine(DeslizarPersonaje());
    }

    private IEnumerator DeslizarPersonaje()
    {
        EstaDeslizando = true;
        playerAnimaciones.AnimDeslizar();
        CollaiderDeslizar(true);
        yield return new WaitForSeconds(1.1f);
        EstaDeslizando = false;
        CollaiderDeslizar(false);
    }

    private void CollaiderDeslizar(bool deslizar)
    {
        if (deslizar)
        {
            //Modificar collaider para el deslizamiento
            characterController.radius = 0.4f;
            characterController.height = 0.6f;
            characterController.center = new Vector3(0f, 0.4f, 0f);
        }
        else
        {
            characterController.radius = controllerRadio;
            characterController.height = controllerAltura;
            characterController.center = new Vector3(0f, controllerPosicionY,0f);
        }
    }

    private void DetectarInput()
    {
        direccionInput = DirInput.Null;
        if(Input.GetKeyDown(KeyCode.A))
        {
            direccionInput = DirInput.Izquierda;
            carrilAct--;
        }
        else if(Input.GetKeyDown(KeyCode.D)) 
        {
            direccionInput = DirInput.Derecha;
            carrilAct++; 
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direccionInput = DirInput.Abajo;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            direccionInput = DirInput.Arriba;
        }

        carrilAct = Mathf.Clamp(carrilAct, -1, 1);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Obstaculo"))
        {
            if(gameManager.EstadoActual == EstadoJuego.GameOver)
            {
                return;
            }
            playerAnimaciones.AnimColision();
            gameManager.CambiarEstado(EstadoJuego.GameOver);
        }
    }
}
