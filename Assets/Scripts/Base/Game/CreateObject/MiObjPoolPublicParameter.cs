using UnityEngine;
using BXB.Core;

public class MiObjPoolPublicParameter : MiBaseMonoBeHaviourClass, IObjPool
{
    [SerializeField, ReadOnly] protected ulong id;
    [SerializeField, ReadOnly] protected GameObject original;

    public virtual void SettingOriginal(GameObject original)
    {
        this.original = original;
    }
    public virtual void SettingId(ulong id)
    {
        this.id = id;
    }
    
    public virtual void Destroy()
    {
        gameObject.SetActive(false);
        transform.Normalization(null);
        ObjPool.Repulace(original, gameObject).Wait();
    }

    public virtual ref readonly ulong GetId()
    {
        return ref id;
    }
}
