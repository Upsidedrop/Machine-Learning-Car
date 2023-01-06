using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Checkpoint : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Reward (" + (collision.GetComponent<NeuralNetwork>().score + 1) + ")");
        if (gameObject.name == "Reward (" + (collision.GetComponent<NeuralNetwork>().score + 1) + ")")
        {
            print(collision.name);
            collision.GetComponent<NeuralNetwork>().score++;
        }
    }
}
