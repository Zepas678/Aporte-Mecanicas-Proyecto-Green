using UnityEngine;

public class Goal : MonoBehaviour
{
    public bool isGoal;
    void Awake()
    {
        isGoal = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Box")
        {
            isGoal = true;
            GameManagerPuzzle.instante.checkGoals();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Box")
        {
            isGoal = false;
        }
    }
}
