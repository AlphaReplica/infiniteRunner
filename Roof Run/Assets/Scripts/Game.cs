using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Game class where game update is happening
/// </summary>
public class Game : MonoBehaviour
{
    public EndlessTile tiles;
    public Character   character;
    public Camera      cam;
    public GameObject  menuCont;
    public Button      play;
    public Text        label;
    public Text        score;
    public float       speed = 10;

    private Vector3    camPosition;
    private float      distance = 0;
    private float      oldDistance;
    private bool       gameStarted = false;


    /// <summary>
    /// registers event listeners and gets score from saves
    /// </summary>
    void Start()
    {
        score.gameObject.SetActive(false);
        play.onClick.AddListener(onPlayClicked);
        character.onDestroyed = onCharacterDied;

        oldDistance = PlayerPrefs.GetFloat("highScore");
        camPosition = cam.transform.localPosition;

        label.text = (oldDistance > 0) ? "Longest Run\n" + oldDistance.ToString("0.##") : "";
    }

    /// <summary>
    /// called when UI button clicked everything gets reseted and game is starting
    /// </summary>
    public void onPlayClicked()
    {
        distance = 0;
        tiles.reset();
        character.reset();
        menuCont.SetActive(false);
        score.gameObject.SetActive(true);
        gameStarted = true;
    }

    /// <summary>
    /// called when character death delegate gets invoked
    /// </summary>
    public void onCharacterDied()
    {
        gameStarted = false;
        menuCont.SetActive(true);
        score.gameObject.SetActive(false);
        label.text = "You Run : " + distance.ToString("0.##") + " your best Run is : " + oldDistance.ToString("0.##");
        if (distance>oldDistance)
        {
            oldDistance = distance;
            PlayerPrefs.SetFloat("highScore",oldDistance);
            label.text = "New Longest Run!\n" + oldDistance.ToString("0.##");
        }
    }

    /// <summary>
    /// Game Update happens from here, other update methods in game aren't present
    /// </summary>
    void Update ()
    {
		if(gameStarted)
        {
            shakeCamera();
            tiles.updateTiles(distance);
            character.updateCharacter(inputPosition, Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space));
            distance  += Time.deltaTime * speed;
            score.text = "Distance\n"+distance.ToString("0.##");
        }
	}

    /// <summary>
    /// shakes camera to make game more dynamic
    /// </summary>
    private void shakeCamera()
    {
        cam.transform.localPosition = camPosition + new Vector3(character.spinePosition.x,
                                                               (character.spinePosition.y-5)/5,
                                                                character.spinePosition.z);
    }

    /// <summary>
    /// gets position from input arrows and returns lane position where character should navigate
    /// </summary>
    private int inputPosition
    {
        get
        {
            int charPosition = character.lane;

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                charPosition = (character.lane < tiles.lanes[1]) ? tiles.lanes[1] : tiles.lanes[2];
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                charPosition = (character.lane > tiles.lanes[1]) ? tiles.lanes[1] : tiles.lanes[0];
            }
            return charPosition;
        }
    }
}
