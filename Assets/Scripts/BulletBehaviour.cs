using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private float despawnTime = 3;

    private void Update()
    {
        despawnTime -= Time.deltaTime;
        if (despawnTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
