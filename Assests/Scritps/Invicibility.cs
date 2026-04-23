using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Invicibility : MonoBehaviour
{
    bool hit;
    [SerializeField] GameObject hurtBox;

    [SerializeField] float timeInvi;

    float resetTimeInvi;

    [SerializeField] Material invicibilityShader;

    SkinnedMeshRenderer SKR;

    Material[] shaders;

    void Awake()
    {
        resetTimeInvi = timeInvi;
        SKR = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();

        shaders = SKR.sharedMaterials;
    }

    public void onHit()
    {
        hurtBox.SetActive(false);
        hit = true;
        shaders[1] = invicibilityShader;
        SKR.sharedMaterials = shaders;
    }

    void Update()
    {
        shaders[0] = SKR.sharedMaterials[0];
        if (hit)
        {
            timeInvi -= Time.deltaTime;
            if (timeInvi <= 0)
            {
                hit = false;
                hurtBox.SetActive(true);
                shaders[1] = shaders[0];
                SKR.sharedMaterials = shaders;
                timeInvi = resetTimeInvi;
            }
        }
    }
}
