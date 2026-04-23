using System.Collections;
using System.Numerics;
using Unity.Cinemachine;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Player_Life : MonoBehaviour
{
    [SerializeField] private float maxLife;
    [SerializeField] private float lifeCurrent;

    public GameObject healthMaxSprite;
    public GameObject healthMediumSprite;
    public GameObject healthLowSprite;

    public GameObject shakeCamera;

    new Rigidbody rigidbody;

    Animator animator;

    void Awake()
    {
        lifeCurrent = maxLife;
        healthMaxSprite.SetActive(true);
        healthMediumSprite.SetActive(false);
        healthLowSprite.SetActive(false);

        rigidbody = GetComponent<Rigidbody>();

        animator = transform.Find("Green_Model").GetComponent<Animator>();
    }

    void Start()
    {
    }

    void Update()
    {
        if (lifeCurrent <= maxLife && lifeCurrent >= maxLife / 1.33333333)
        {
            healthMaxSprite.SetActive(true);
            healthMediumSprite.SetActive(false);
            healthLowSprite.SetActive(false);
        }
        else if (lifeCurrent <= maxLife / 1.33333333 && lifeCurrent >= maxLife / 4)
        {
            healthMaxSprite.SetActive(false);
            healthMediumSprite.SetActive(true);
            healthLowSprite.SetActive(false);
        }
        else
        {
            healthMaxSprite.SetActive(false);
            healthMediumSprite.SetActive(false);
            healthLowSprite.SetActive(true);
        }
    }
    public void getDamage(float damageTaken, Transform enemyTransform, float knockBackForce)
    {
        lifeCurrent = lifeCurrent - damageTaken;
        animator.SetTrigger("Hurt");
        if (lifeCurrent <= 0)
        {
            gameOver();
        }
        knockBack(enemyTransform, knockBackForce);
        StartCoroutine(doshake(.4f));
    }

    void gameOver()
    {
        Destroy(gameObject);
    }


    void knockBack(Transform enemyTransform, float knockBackForce)
    {
        Vector3 direction = (transform.position - enemyTransform.position).normalized;
        rigidbody.linearVelocity = direction * knockBackForce;
    }

    IEnumerator doshake(float timeShake)
    {
        shakeCamera.GetComponent<CinemachineBasicMultiChannelPerlin>().enabled = true;
        yield return new WaitForSeconds(timeShake);
        shakeCamera.GetComponent<CinemachineBasicMultiChannelPerlin>().enabled = false;
        StopCoroutine(doshake(0f));
    }
}
