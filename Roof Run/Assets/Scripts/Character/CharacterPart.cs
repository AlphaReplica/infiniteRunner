using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper Class for character parts, saves starting position and parent and adds rigidbody when character dies
/// </summary>
[SerializeField]
public class CharacterPart
{
    private GameObject part;
    private Rigidbody  body;
    private Collider   collider;
    private Transform  parent;
    private Vector3    position;
    private Vector3    rotation;

    /// <summary>
    /// Position and rotation registered in constructor, also adds Rigidbody if it doesn't present
    /// </summary>
    public CharacterPart(GameObject obj)
    {
        part     = obj;
        parent   = part.transform.parent;
        position = part.transform.localPosition;
        rotation = part.transform.localEulerAngles;
        collider = obj.GetComponent<Collider>();
        body     = obj.GetComponent<Rigidbody>();

        if (body == null)
        {
            body = part.AddComponent<Rigidbody>();
        }
        body.mass = 10;
        body.isKinematic = true;
        collider.enabled = false;
    }

    /// <summary>
    /// called when character dies, part is detached from parent and rigidbody enables
    /// </summary>
    public void detach()
    {
        part.transform.parent = null;
        collider.enabled = true;
        body.isKinematic = false;
        body.AddForce( new Vector3(Random.Range(-100, 100),
                                               Random.Range(-100, 100),
                                               Random.Range(-100, 100)), ForceMode.Impulse);
    }

    /// <summary>
    /// resets to starting position and parent, in other words assembling to starting state
    /// </summary>
    public void reset()
    {
        collider.enabled                = false;
        body.isKinematic                = true;
        part.transform.parent           = parent;
        part.transform.localPosition    = position;
        part.transform.localEulerAngles = rotation;
    }
}
