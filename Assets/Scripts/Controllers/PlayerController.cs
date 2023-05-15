using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerEntity))]
public class PlayerController : MonoBehaviour
{
    private PlayerEntity m_playerEntity;

    public void OnFlyInputAction(InputAction.CallbackContext callbackContext)
    {
        m_playerEntity.FlyFactor = callbackContext.ReadValue<float>();
    }

    public void OnStrafeInputAction(InputAction.CallbackContext callbackContext)
    {
        m_playerEntity.StrafeFactor = callbackContext.ReadValue<float>();
    }

    public void OnPauseInputAction(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Canceled)
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

    private void Awake()
    {
        m_playerEntity = GetComponent<PlayerEntity>();
    }
}
