using UnityEngine;
using UnityEngine.SceneManagement;
public class InitTutorial : MonoBehaviour
{
    public static InitTutorial Instance;

    public bool canMove = true;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        if (SceneManager.GetActiveScene().name == "Cena 1") {
            //canMove = false;
            // Faça animação de entrada
            //perceber que a vida esta baixa
            //deixar o player se mover
        }
    }

    

    public void CanMove() {
        canMove = true;
    }
}
