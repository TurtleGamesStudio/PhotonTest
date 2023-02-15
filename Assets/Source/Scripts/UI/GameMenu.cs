using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private Image _menuPanel;

    private KeyboardInput _keyboardInput;

    private bool _isMenuOpen = false;

    private void OnDisable()
    {
        _keyboardInput.EscapePressed -= OnEscapePressed;
    }

    public void Init(KeyboardInput keyboardInput)
    {
        _keyboardInput = keyboardInput;
        _keyboardInput.EscapePressed += OnEscapePressed;
        Close();
    }

    private void OnEscapePressed()
    {
        if (_isMenuOpen)
            Close();
        else
            Open();
    }

    private void Close()
    {
        _menuPanel.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _isMenuOpen = false;
    }

    private void Open()
    {
        _menuPanel.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _isMenuOpen = true;
    }
}
