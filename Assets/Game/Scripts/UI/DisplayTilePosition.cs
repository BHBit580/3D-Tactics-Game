using TMPro;
using UnityEngine;

public class DisplayTilePosition : MonoBehaviour
{
    private TextMeshProUGUI displayText;

    private void Start()
    {
        displayText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                TileInfo tileInfo = hit.transform.GetComponent<TileInfo>();
                
                if (tileInfo != null)
                {
                    DisplayTileInfo(tileInfo);
                }
            }
        }
    }
    
    private void DisplayTileInfo(TileInfo tileInfo)
    {
        displayText.text = "Tile Position = "  + tileInfo.cubePosition.ToString();
    }
    
    
}
