using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EndlessTile helper class to manage tiles
/// </summary>
public class Tile : MonoBehaviour
{
    public Transform obstacleContainer;
    public Transform environmentContainer;
    public Vector2   obstacleRange = new Vector2(-5, 5);

	void Start ()
    {
        disableEnvironmentColliders();
	}

    /// <summary>
    /// disables every collider in tile gameobject's child environemtContainer, environemtContainer must be set from editoe
    /// </summary>
    private void disableEnvironmentColliders()
    {
        if(environmentContainer!=null)
        {
            foreach(Collider col in environmentContainer.GetComponentsInChildren<Collider>())
            {
                col.enabled = false;
            }
        }
    }

    /// <summary>
    /// sets object to x position and randomly rotates to 180 angles
    /// </summary>
    public void setTilePosition(int position)
    {
        int rotation = (Random.Range(0, 2) == 0) ? 0 : 180;

        transform.localEulerAngles = new Vector3(0, rotation, 0);
        transform.localPosition    = new Vector3(position, 0, 0);
    }

    /// <summary>
    /// sets obstacle object in current object, position is passed as lane z position, if object is null then every obstacle will be removed
    /// </summary>
    public void setObstacle(GameObject obj, int position)
    {
        if(obj!=null)
        {
            obj.SetActive(true);
            obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(Random.Range(obstacleRange.x, obstacleRange.y), 0, position);
        }
        else
        {
            foreach(Transform tr in obstacleContainer.GetComponentInChildren<Transform>())
            {
                if(tr!=obstacleContainer)
                {
                    tr.gameObject.SetActive(false);
                    tr.parent = null;
                }
            }
        }
    }
}
