using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private float despawnTime = 3;
    public int scoreMultiplier = 1;
    public int points = 1;
    

    private void Update()
    {
        despawnTime -= Time.deltaTime;
        if (despawnTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
