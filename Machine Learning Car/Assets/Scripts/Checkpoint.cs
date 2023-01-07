using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Reward (" + (collision.GetComponent<NeuralNetwork>().location + 1) + ")");
        if (gameObject.name == "Reward (" + (collision.GetComponent<NeuralNetwork>().location + 1) + ")")
        {
            print(collision.name);
            collision.GetComponent<NeuralNetwork>().location++;
        }
    }
}
