using System.Numerics;
using UnityEngine;

public class Box : MonoBehaviour
{

    new Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Green_Controller green_Controller))
                {
                    //Debug.Log("Se empujar");

                    green_Controller.canPush = true;
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Green_Controller green_Controller))
                {
                    //Debug.Log("No se empujar");
                    green_Controller.canPush = false;
                }
            }
        }
    }
}
