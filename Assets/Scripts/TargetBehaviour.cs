using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
    private float respawnTimer = 3;
    private bool respawnCanStart = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<BulletBehaviour>(out _) == true)
        {
            transform.gameObject.SetActive(false);
            respawnTimer = 3;
            respawnCanStart = true;
        }
    }

    private void Update()
    {
        if (respawnCanStart)
        {
            respawnTimer -= Time.deltaTime;
        }
        if (respawnTimer <= 0)
        {
            transform.gameObject.SetActive(true);
            respawnCanStart = false;
        }
    }
}
