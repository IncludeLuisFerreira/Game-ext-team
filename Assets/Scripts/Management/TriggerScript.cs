using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            switch (gameObject.name)
            {
                case "FruitTrigger":
                    TextManager.Instance.FruitText();
                    TextManager.Instance.fruitText = true;
                    break;
                case "JumpTrigger":
                    TextManager.Instance.JumpText();
                    TextManager.Instance.jumpText = true;
                    break;
                case "AttackTrigger":
                    TextManager.Instance.AttackText();
                    TextManager.Instance.attackText = true;
                    break;
                case "FireTrigger":
                    TextManager.Instance.FireText();
                    TextManager.Instance.rollText = true;
                    break;
            }
        }
    }
}
