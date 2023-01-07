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
    [HideInInspector]
    public int location = -1;
    [SerializeField]
    float score = -1;
    float bestScore = -1;
    [SerializeField]
    float[] weights = new float[16];
    [SerializeField]
    float[] biases = new float[16];
    [SerializeField]
    float[] outputs;
    [SerializeField]
    float[] middle;
    float rotation;
    [SerializeField]
    float[] bestWeights = new float[16];
    [SerializeField]
    float[] bestBiases = new float[16];
    [SerializeField]
    float timer = 0;
    [SerializeField]
    LayerMask LayerMask;

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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, float.PositiveInfinity, LayerMask);
        inputs[0] = hit.distance;
        Debug.DrawRay(transform.position, transform.right);

        //raycast up+right
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, transform.up + transform.right, float.PositiveInfinity, LayerMask);
        inputs[1] = hit1.distance;
        Debug.DrawRay(transform.position, transform.up + transform.right);

        //raycast up
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, transform.up, float.PositiveInfinity, LayerMask);
        inputs[2] = hit2.distance;
        Debug.DrawRay(transform.position, transform.up);

        //raycast up + left
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, transform.up - transform.right, float.PositiveInfinity, LayerMask);
        inputs[3] = hit3.distance;
        Debug.DrawRay(transform.position, transform.up - transform.right);

        //raycast left
        RaycastHit2D hit4 = Physics2D.Raycast(transform.position, -transform.right, float.PositiveInfinity, LayerMask);
        inputs[4] = hit4.distance;
        Debug.DrawRay(transform.position, -transform.right);

        //raycast left+down
        RaycastHit2D hit5 = Physics2D.Raycast(transform.position, -transform.up - transform.right, float.PositiveInfinity, LayerMask);
        inputs[5] = hit5.distance;
        Debug.DrawRay(transform.position, -transform.up - transform.right);

        //raycast down
        RaycastHit2D hit6 = Physics2D.Raycast(transform.position, -transform.up, float.PositiveInfinity, LayerMask);
        inputs[6] = hit6.distance;
        Debug.DrawRay(transform.position, -transform.up);
        
        //raycast down+right
        RaycastHit2D hit7 = Physics2D.Raycast(transform.position, -transform.up + transform.right, float.PositiveInfinity, LayerMask);
        inputs[7] = hit7.distance;
        Debug.DrawRay(transform.position, -transform.up + transform.right);

        //Converts the inputs to middle
        for (int i = 0; i < 8; i++)
        {
            middle[i] = (inputs[i] * weights[i]) + biases[i];
            MiddleNode(middle[i]);
            outputs[i] = (middle[i] * weights[i+8]) + biases[i + 8];
            rotation += outputs[i];
        }


    }
    void NextGen()
    {
        generation++;
        //Updates the score
        score = location + (timer / 3);
        //resets the score
        if (score > bestScore)
        {
            bestScore = score;
            print("New Best: " + bestScore);
            for (int i = 0; i < 16; i++)
            {
                bestWeights[i] = weights[i];
                bestBiases[i] = biases[i];
            }


        }
        else
        {
            for (int i = 0; i < 16; i++)
            {
                weights[i] = bestWeights[i];
                biases[i] = bestBiases[i];
            }

        }
        location = -1;
        //Slightly adjusts the weights
        for (int i = 0; i < 16; i++)
        {
            weights[i] += Random.Range(-0.30f, 0.30f);
            biases[i] += Random.Range(-0.30f, 0.30f);
        }
        //resets the timer
        timer = 0;
        StartCoroutine(Timer());
        //resets the position and rotation of the AI
        rotation = 0;
        transform.SetPositionAndRotation(
            new Vector3(-2.6789999f, -3.18000007f, 0),
            Quaternion.Euler(0, 0, rotation));
    }
    private void Start()
    {
        for (int i = 0; i < 16; i++)
        {
            bestWeights[i] = weights[i];
            bestBiases[i] = biases[i];
        }    

            NextGen();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopAllCoroutines();
        NextGen();
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.01f);
        timer += 0.01f;
        //begins a recursive loop
        StartCoroutine(Timer());
    }
    int MiddleNode(float node)
    {
        if (node > 1)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
