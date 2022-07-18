using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

//This is something that needs to be used with cautions as this script is running now regardless in play mode and editor mode. 
[ExecuteAlways]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] private TextMeshPro CoordinateLabelText;
    [SerializeField] private Color LabelsTextColour = Color.white;
    [SerializeField] private Color LabelsNonValidColour = Color.red;

    [SerializeField] private Waypoint _waypoint;

    // Start is called before the first frame update
    void Start()
    {
        //Retrieves this on start, which is located on this current gameobject.
        CoordinateLabelText = GetComponent<TextMeshPro>();

        _waypoint = GetComponentInParent<Waypoint>();
        CoordinateLabelText.enabled = false;

        //run the coordinates to be set in game once, but not update.
        DisplayCurrentCoordinates();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the unity editor is in play mode or edit mode.
        if (!Application.isPlaying)
        {
            //if it is in edit mode and only edit mod we do this.
            //we want to display the coordinates of this tile ontop of the object while we are in the editor.
            DisplayCurrentCoordinates();
        }

        ColorCoordinates();
        ToggleLabels();
    }

    private void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CoordinateLabelText.enabled = !CoordinateLabelText.IsActive();
        }
    }


    private void ColorCoordinates()
    {
        if (_waypoint.IsPlaceableDefence)
        {
            CoordinateLabelText.color = LabelsTextColour;
        }
        else
        {
            CoordinateLabelText.color = LabelsNonValidColour;
            var color =new Color(CoordinateLabelText.color.r, CoordinateLabelText.color.g, CoordinateLabelText.color.b ,CoordinateLabelText.color.a);
            color.a = 0.75f;
            CoordinateLabelText.color = color;
        }
    }

    private void DisplayCurrentCoordinates()
    {
        //The way he does trhis is by just making a Vector2Int variable at the top and storing the x and z and then just writes it out
        //to the label. Which is another way to do it and maybe is more efficient but this way works and is just as effective.

        //In the lecture he divides the texts positions to be relative to the grid sizes, it doesnt really visually work with mine anyway because my text is scaled to one,
        //but for his, since he was scaling by a grid size of 10 it scaled it to 1 so it displays 1 for his position. Although i still included it so i have it available if needed.

        int xVal = Mathf.RoundToInt(this.gameObject.transform.parent.position.x /
                                    UnityEditor.EditorSnapSettings.move.x);
        int zVal = Mathf.RoundToInt(this.gameObject.transform.parent.position.z /
                                    UnityEditor.EditorSnapSettings.move.z);

        CoordinateLabelText.text = $"X:{xVal}\nZ:{zVal}";
        //he does this in another method but i do not believe it is that necessary lol. Its literally updating from the same values.
        CoordinateLabelText.transform.parent.name = $"(X:{xVal},Z:{zVal})";
    }
}