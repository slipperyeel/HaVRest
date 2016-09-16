using UnityEngine;
using System.Collections;
using System;
using slipperyeel;

public class TestTool : PhysicalItem
{
    [SerializeField]
    private PlayerSkillsEnum skillType = PlayerSkillsEnum.Axe;

    protected override void HandleItemCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.name);
        if (col != null)
        {
            StatUsageTriggerEvent sute = new StatUsageTriggerEvent(skillType);
            SEEventManager.Instance.TriggerEvent(sute);

            Debug.Log("Collision enter with: " + col.gameObject.name);
            PhysicalObject physObj = col.gameObject.GetComponent<PhysicalObject>();
            if(physObj != null)
            {
                float resistance = physObj.GetPhysicalResistanceByImpactType(sImpactType);

                // Do something here I dunno. Some random thoughts.  Apply damage? I dunno. We'd need to apply the strength to a tree or something.
                Debug.Log("Collision object had a resistance of: " + resistance);
                Debug.Log("Strength of this object is: " + sImpactStrength);
            }
        }
    }

    protected override void HandleItemCollisionExit(Collision col)
    {
        Debug.Log("Collision exit with: " + col.gameObject.name);
    }

    protected override void HandleItemCollisionStay(Collision col)
    {
        Debug.Log("Collision stay with: " + col.gameObject.name);
    }
}
