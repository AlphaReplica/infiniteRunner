using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public  Action              onDestroyed;
    public  Transform           spineTransform;
    public  float               speed = 10;
    private List<CharacterPart> parts;
    private Animator            anim;
    private Vector3             defaultPos;
    private float               jumpPosition;
    private float               yPosition;
    private int                 currentPosition;

    /// <summary>
    /// when started registers character parts in parts list
    /// </summary>
    void Start()
    {
        jumpPosition = 0;
        defaultPos   = transform.localPosition;
        anim         = GetComponent<Animator>();
        parts        = new List<CharacterPart>();

        foreach(Renderer rend in GetComponentsInChildren<Renderer>())
        {
            parts.Add(new CharacterPart(rend.gameObject));
        }
    }

    /// <summary>
    /// cycles to parts list where every part's rigidbody gets enabled
    /// </summary>
    private void destroyCharacter()
    {
        foreach(CharacterPart part in parts)
        {
            part.detach();
        }
    }

    /// <summary>
    /// it's needed for jump delay, basically it delays jump y position value
    /// </summary>
    private IEnumerator groundCharacter()
    {
        yield return new WaitForSeconds(0.5f);

        jumpPosition = 0;
    }

    /// <summary>
    /// it must be called every frame, position is z axis of transform
    /// </summary>
    public void updateCharacter(int position, bool isJumping = false)
    {
        if(yPosition < 0.5f)
        {
            currentPosition = position;
            if (isJumping)
            {
                anim.SetTrigger("jump");
                jumpPosition = 5;
                StartCoroutine(groundCharacter());
            }
        }
        
        anim.SetBool("running", true);

        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, defaultPos.y+yPosition, currentPosition),Time.deltaTime * speed);
        yPosition               = Mathf.Lerp(yPosition,jumpPosition,Time.deltaTime * speed);
       
    }

    /// <summary>
    /// resets everything from position to parts
    /// </summary>
    public void reset()
    {
        anim.SetBool("running", false);
        foreach (CharacterPart part in parts)
        {
            part.reset();
        }
        jumpPosition            = 0;
        yPosition               = 0;
        currentPosition         = Mathf.RoundToInt(defaultPos.z);
        transform.localPosition = defaultPos;
    }

    /// <summary>
    /// when collided on something character gets destoryed and onDestroy invokes
    /// </summary>
    private void OnTriggerEnter(Collider col)
    {
        if(anim.GetBool("running"))
        {
            destroyCharacter();
            anim.SetBool("running", false);
            if (onDestroyed!=null)
            {
                onDestroyed.Invoke();
            }
        }
    }

    /// <summary>
    /// lanes are represented as transform z position, basically it returns in what position character is
    /// </summary>
    public int lane
    {
        get
        {
            return currentPosition;
        }
    }

    /// <summary>
    /// returns spine localposition which changes every frame because of animation
    /// </summary>
    public Vector3 spinePosition
    {
        get
        {
            return spineTransform.localPosition;
        }
    }
}
