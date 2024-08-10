using UnityEngine;

public class TextManager : MonoBehaviour
{
    [HideInInspector]public static TextManager Instance;

    [Header("Triggers")]
    public Collider2D fruitExplain;
    public Collider2D jumpExplain;
    public Collider2D attackExplain;
    public Collider2D fireExplain;

    [Header("Booleans")]
    public bool fruitText;
    public bool jumpText;
    public bool attackText;
    public bool rollText;

    private void Start() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void FruitText() {
        if (!fruitText) {
            Debug.Log("Aperte F para comer a fruta e recuperar a sua vida.");
        }
    }

    public void JumpText() {
        if (!jumpText) {
            Debug.Log("Aperte Space ou W para pular.");
        }
    }

    public void AttackText() {
        if (!attackText) {
            Debug.Log("Aperte L para bater e K para defender.");
        }
    }

    public void FireText() {
        if (!rollText) {
            Debug.Log("Aperte C para rolar.");
        }
    }
}
