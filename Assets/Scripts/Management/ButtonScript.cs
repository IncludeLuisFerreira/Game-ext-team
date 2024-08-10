using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public Button StartButton;
    public Button OptionButton;
    public Button ExtraButton;

    private bool loaded = false;

    private void Update() {
        StartButton.onClick.AddListener(OnStart);
        OptionButton.onClick.AddListener(OnOption);
        ExtraButton.onClick.AddListener(OnExtra);
    }

    void OnStart() {
        if (!loaded) {
            SceneManager.LoadSceneAsync("Cena 1");
            loaded = true;
        }
    }

    void OnOption() {
        Debug.Log("Options...");

    }

    void OnExtra() {
        Debug.Log("Extras content...");
    }
}
