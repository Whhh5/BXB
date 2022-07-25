using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MiCommonCollider : MonoBehaviour
{
    [SerializeField] GameObject mainObj;

    public Action<Collision2D> onColliderEnter = new Action<Collision2D>((collision) => { });
    public Action<Collision2D> onColliderExit = new Action<Collision2D>((collision) => { });
    public Action<Collision2D> onColliderStay = new Action<Collision2D>((collision) => { });

    public Action<Collider2D> onColliderTriggerEnter = new Action<Collider2D>((collider2D) => { });
    public Action<Collider2D> onColliderTriggerExit = new Action<Collider2D>((collider2D) => { });
    public Action<Collider2D> onColliderTriggerStay = new Action<Collider2D>((collider2D) => { });
    public GameObject GetMainObject()
    {
        return mainObj;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        onColliderEnter.Invoke(collision);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        onColliderExit.Invoke(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        onColliderStay.Invoke(collision);
    }
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        onColliderTriggerEnter.Invoke(collider2D);
    }
    private void OnTriggerExit2D(Collider2D collider2D)
    {
        onColliderTriggerExit.Invoke(collider2D);
    }
    private void OnTriggerStay2D(Collider2D collider2D)
    {
        onColliderTriggerStay.Invoke(collider2D);
    }
}
