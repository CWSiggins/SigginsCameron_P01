using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLoadout : MonoBehaviour
{
    public Ability EquippedAbility { get; private set; }

    public void EquipAbility(Ability ability)
    {
        //TODO Equipping VFX, SFX
        RemoveCurrentAbilityObject();
        CreateNewAbilityObject(ability);
    }

    public void UseEquippedAbility(Transform target)
    {
        EquippedAbility.Use(this.transform, target);
    }

    public void RemoveCurrentAbilityObject()
    {
        //TODO replace with Object Pooling
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void CreateNewAbilityObject(Ability ability)
    {
        EquippedAbility = Instantiate(ability, transform.position, Quaternion.identity);
        //make sure it's grouped under this loadout object
        EquippedAbility.transform.SetParent(this.transform);
    }
}
