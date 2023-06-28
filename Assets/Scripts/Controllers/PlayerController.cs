using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerEntity))]
public class PlayerController : MonoBehaviour
{
    private PlayerEntity m_playerEntity;
    private Vector3 m_initialPos;
    private Quaternion m_initialRot;

    public void OnFlyInputAction(InputAction.CallbackContext callbackContext)
    {
        HandleFlyAction(callbackContext.ReadValue<float>());
    }

    public void OnStrafeInputAction(InputAction.CallbackContext callbackContext)
    {
        HandleStrafeAction(callbackContext.ReadValue<float>());
    }

    public void OnPauseInputAction(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Canceled)
        {
            HandlePauseAction();
        }
    }
    
    private void Awake()
    {
        m_playerEntity = GetComponent<PlayerEntity>();
    }

    private void Start()
    {
        m_initialPos = transform.position;
        m_initialRot = transform.rotation;

        var gameManager = GameManager.Instance;
        gameManager.OnEnterMenu += OnEnterMenu;
        gameManager.OnStart += OnGameStart;

        var screenInputManager = ScreenInputManager.Instance;
        screenInputManager.OnPauseClickAction += HandlePauseAction;
        screenInputManager.OnFlyAction += HandleFlyAction;
        screenInputManager.OnStrafeAction += HandleStrafeAction;
    }

    private void OnDestroy()
    {
        var gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.OnEnterMenu -= OnEnterMenu;
            gameManager.OnStart -= OnGameStart;
        }

        var screenInputManager = ScreenInputManager.Instance;
        if (screenInputManager != null)
        {
            screenInputManager.OnPauseClickAction -= HandlePauseAction;
            screenInputManager.OnFlyAction -= HandleFlyAction;
            screenInputManager.OnStrafeAction -= HandleStrafeAction;
        }
    }

    private void OnEnterMenu()
    {
        m_playerEntity.gameObject.SetActive(false);
    }

    private void OnGameStart()
    {
        m_playerEntity.gameObject.SetActive(true);

        transform.SetPositionAndRotation(m_initialPos, m_initialRot);
        m_playerEntity.Spawn();
    }

    private void HandleFlyAction(float value)
    {
        m_playerEntity.FlyFactor = value;
    }

    private void HandleStrafeAction(float value)
    {
        m_playerEntity.StrafeFactor = value;
    }

    private void HandlePauseAction()
    {
        var gameManager = GameManager.Instance;
        if (gameManager.IsPaused)
        {
            gameManager.ResumeGame();
        }
        else
        {
            gameManager.PauseGame();
        }
    }
}
