using UnityEngine;

public class WeaponAnimScript : MonoBehaviour
{
    public Transform recoilEnd;
    public Transform weapon;
    private Vector3 startPos;
    private Vector3 startRot;


    private void Awake()
    {
        startPos = weapon.localPosition;
        startRot = weapon.eulerAngles;
    }
    private void OnEnable()
    {
        EventHandler.onWeaponShoot += WeaponWiggle;
    }

    private void WeaponWiggle()
    {
        weapon.localPosition = Vector3.Lerp(weapon.localPosition, recoilEnd.localPosition, Time.deltaTime * 3);
        weapon.eulerAngles = Vector3.Lerp(weapon.eulerAngles, recoilEnd.eulerAngles, Time.deltaTime * 3);
    }

    void Update()
    {
        weapon.localPosition = Vector3.Lerp(weapon.localPosition, startPos,3);
        weapon.eulerAngles = Vector3.Lerp(weapon.eulerAngles, startRot, 3);
    }
}
