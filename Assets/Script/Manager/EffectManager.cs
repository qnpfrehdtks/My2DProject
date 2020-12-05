using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    public override void InitializeManager()
    {
        string[] enumName = System.Enum.GetNames(typeof(E_EFFECT));

        for (int i = 0; i < enumName.Length; i++)
        {
            string path = "Prefabs/Effect/" + enumName[i];
            if (enumName[i] == "NONE" || enumName[i] == "END") continue;

            GameObject effectOriginal = ResourceManager.Instance.Load<GameObject>(path);

            GameObject go = ResourceManager.Instance.instantiate(effectOriginal, transform);
            Common.GetOrAddComponent<EffectObject>(go);
            PoolingManager.Instance.PushToPool(go);
        }
    }


    public GameObject PlayEffect(E_EFFECT effect, Vector3 pos, Quaternion quat, bool isInstance = true, float DestroyTime = 2.0f, Transform tr = null)
    {
        GameObject effectObject = PoolingManager.Instance.PopFromPool(effect.ToString(), pos, quat);

        if(effectObject == null)
        {
            return null;
        }

        EffectObject effectObj = Common.GetOrAddComponent<EffectObject>(effectObject);

        if (effectObj != null)
        {
            if(isInstance)
                effectObj.DestroyEffect(DestroyTime);

            if (tr != null)
            {
                effectObj.AttachToTransform(tr, Vector3.zero);
            }
        }

        return effectObject;
    }

    public void StopEffect(EffectObject effect)
    {
        if(effect == null)
        {
            return;
        }

        PoolingManager.Instance.PushToPool(effect.gameObject);
    }
}
