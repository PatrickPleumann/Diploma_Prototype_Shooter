using System.Collections;
using UnityEditor.Timeline;
using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
    [SerializeField] private float respawnTime;
    [SerializeField] private Material curMaterial;

    private MeshRenderer rend;
    private bool targetHittable;

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        targetHittable = true;
        rend.material = curMaterial;
        rend.material.color = Color.green;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (targetHittable)
        {
            if (other.transform.TryGetComponent(out BulletBehaviour _temp) == true)
            {
                targetHittable = false;
                rend.material.color = Color.red;
                StartCoroutine(ChangeColorOnHit(respawnTime));
                ScoreBoard.Instance.AddPoints(_temp.points);
            }
        }
    }

    IEnumerator ChangeColorOnHit(float _respawnTime)
    {
        yield return new WaitForSecondsRealtime(_respawnTime);
        rend.material.color = Color.green;
        targetHittable = true;
    }
}
