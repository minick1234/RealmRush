using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

//This is something that needs to be used with cautions as this script is running now regardless in play mode and editor mode. 
[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] private TextMeshPro CoordinateLabelText;
    [SerializeField] private Color LabelsTextColour = Color.white;
    [SerializeField] private Color LabelsNonValidColour = Color.red;
    [SerializeField] private Color LabelsExploredColour = Color.cyan;
    [SerializeField] private Color LabelsPathColour = Color.green;

    [SerializeField] private Vector2 coordinates;
    [SerializeField] private GridManager _gridManager;

    // Start is called before the first frame update
    void Awake()
    {
        //Retrieves this on start, which is located on this current gameobject.
        CoordinateLabelText = GetComponent<TextMeshPro>();

        _gridManager = FindObjectOfType<GridManager>();
        //CoordinateLabelText.enabled = false;
        //DisplayCurrentCoordinates();
        DisplayCurrentCoordinates();
        ColorCoordinates();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the unity editor is in play mode or edit mode.
        // if (!Application.isPlaying)
        // {
        //     CoordinateLabelText.enabled = true;
        // }

        DisplayCurrentCoordinates();
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
        if (_gridManager == null)
        {
            return;
        }

        Node node = _gridManager.GetNode(coordinates);

        if (node == null)
        {
            return;
        }

        if (!node.isWalkable)
        {
            CoordinateLabelText.color = LabelsNonValidColour;
        }
        else if (node.isPath)
        {
            CoordinateLabelText.color = LabelsPathColour;
        }
        else if (node.isExplored)
        {
            CoordinateLabelText.color = LabelsExploredColour;
        }
        else
        {
            CoordinateLabelText.color = Color.gray;
        }
    }

    private void DisplayCurrentCoordinates()
    {

        if (_gridManager == null)
        {
            return;
        }
        
        //The way he does trhis is by just making a Vector2Int variable at the top and storing the x and z and then just writes it out
        //to the label. Which is another way to do it and maybe is more efficient but this way works and is just as effective.

        //In the lecture he divides the texts positions to be relative to the grid sizes, it doesnt really visually work with mine anyway because my text is scaled to one,
        //but for his, since he was scaling by a grid size of 10 it scaled it to 1 so it displays 1 for his position. Although i still included it so i have it available if needed.

        int xVal = Mathf.RoundToInt(this.gameObject.transform.parent.position.x /
                                    _gridManager.UnityGridSize);
        int zVal = Mathf.RoundToInt(this.gameObject.transform.parent.position.z /
                                   _gridManager.UnityGridSize);

        
        CoordinateLabelText.text = $"X:{xVal}\nZ:{zVal}";
        //he does this in another method but i do not believe it is that necessary lol. Its literally updating from the same values.
        CoordinateLabelText.transform.parent.name = $"(X:{xVal},Z:{zVal})";
        coordinates = new Vector2(xVal, zVal);
    }
}