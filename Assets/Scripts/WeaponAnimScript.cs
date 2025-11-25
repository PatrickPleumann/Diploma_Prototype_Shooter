using UnityEngine;

public class WeaponAnimScript : MonoBehaviour
{
    public Transform weapon;
    public Vector3 weaponOld;

    private void Awake()
    {
        //weaponOld = weapon.localRotation;
    }
    private void OnEnable()
    {
        EventHandler.onWeaponShoot += WeaponWiggle;
    }

    private void WeaponWiggle()
    {
        var temp = weapon;
        weapon.localEulerAngles = new Vector3(weapon.localEulerAngles.x + 20, weapon.localEulerAngles.y, weapon.localEulerAngles.z);
    }


    // Update is called once per frame
    void Update()
    {
        //weapon.localRotation = Quaternion.Euler(Vector3.Lerp(weapon.eulerAngles, weaponOld, Time.deltaTime));
    }
}
