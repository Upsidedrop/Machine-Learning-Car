using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
    Rigidbody2D Rigidbody2D;
    [SerializeField]
    float[] inputs;
    [SerializeField]
    int generation = 0;
    public int score = -1;
    int bestScore = -1;
    [SerializeField]
    float[] weights = new float[8];
    [SerializeField]
    float[] outputs;
    float rotation;
    [SerializeField]
    float[] bestWeights = new float[8];

    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        rotation = 0;
    }
    private void Update()
    {
        
        //Provides constant movement
        Rigidbody2D.velocity = transform.right*1.4f;

        //Raycast right
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
        inputs[0] = hit.distance;
        Debug.DrawRay(transform.position, transform.right);

        //raycast up+right
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, transform.up + transform.right);
        inputs[1] = hit1.distance;
        Debug.DrawRay(transform.position, transform.up + transform.right);

        //raycast up
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, transform.up);
        inputs[2] = hit2.distance;
        Debug.DrawRay(transform.position, transform.up);

        //raycast up + left
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, transform.up - transform.right);
        inputs[3] = hit3.distance;
        Debug.DrawRay(transform.position, transform.up - transform.right);

        //raycast left
        RaycastHit2D hit4 = Physics2D.Raycast(transform.position, -transform.right);
        inputs[4] = hit4.distance;
        Debug.DrawRay(transform.position, -transform.right);

        //raycast left+down
        RaycastHit2D hit5 = Physics2D.Raycast(transform.position, -transform.up - transform.right);
        inputs[5] = hit5.distance;
        Debug.DrawRay(transform.position, -transform.up - transform.right);

        //raycast down
        RaycastHit2D hit6 = Physics2D.Raycast(transform.position, -transform.up);
        inputs[6] = hit6.distance;
        Debug.DrawRay(transform.position, -transform.up);
        
        //raycast down+right
        RaycastHit2D hit7 = Physics2D.Raycast(transform.position, -transform.up + transform.right);
        inputs[7] = hit7.distance;
        Debug.DrawRay(transform.position, -transform.up + transform.right);

        //Converts the inputs to outputs
        for (int i = 0; i < 8; i++)
        {
            outputs[i] = inputs[i] * weights[i];
            rotation += outputs[i];
        }


    }
    void NextGen()
    {
        generation++;
        //resets the score
        score = -1;
        if (score > bestScore)
        {
            bestScore = score;
            print("New Best: " + bestScore);
            for (int i = 0; i < 8; i++)
            {
                bestWeights[i] = weights[i];
            }


        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                weights[i] = bestWeights[i];
            }

        }
        //Slightly adjusts the weights
        for (int i = 0; i < 8; i++)
        {
            weights[i] += Random.Range(-5.00f, 5.00f);
        }
        //resets the position and rotation of the AI
        rotation = 0;
        transform.SetPositionAndRotation(
            new Vector3(-2.6789999f, -3.18000007f, 0),
            Quaternion.Euler(0, 0, rotation));
    }
    private void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            bestWeights[i] = weights[i];
        }    

            NextGen();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        NextGen();
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}
