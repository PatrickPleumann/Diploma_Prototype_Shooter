using System.Collections;
using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<BulletBehaviour>(out _) == true)
        {
            transform.gameObject.SetActive(false);
        }
    }
}
