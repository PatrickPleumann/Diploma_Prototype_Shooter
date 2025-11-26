using TMPro;
using UnityEngine;

public class WeaponAnimScript : MonoBehaviour
{
    private Transform weaponTransform;
    private Transform oldTransform;

    private void Awake()
    {
        weaponTransform = GetComponent<Transform>();
        oldTransform = weaponTransform;
    }
    private void OnEnable()
    {
        EventHandler.onWeaponShoot += WeaponWiggle;
    }

    private void WeaponWiggle()
    {
        oldTransform = weaponTransform;

        weaponTransform.eulerAngles = Vector3.Lerp(weaponTransform.eulerAngles, new Vector3
            (weaponTransform.eulerAngles.x + 20, weaponTransform.eulerAngles.y, weaponTransform.eulerAngles.z), 20 * Time.deltaTime);
    }

    void Update()
    {
        weaponTransform.eulerAngles = Vector3.Lerp(weaponTransform.eulerAngles, oldTransform.eulerAngles, 20 * Time.deltaTime);
    }
}
