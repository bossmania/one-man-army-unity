using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{

    public Transform trans;
    public Transform player;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public int mouseRadius;

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(Camera.main);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x = Mathf.Clamp(mousePos.x, -mouseRadius, mouseRadius);
        mousePos.y = Mathf.Clamp(mousePos.y, -mouseRadius, mouseRadius);
        float xx = Mathf.Clamp((player.position.x + mousePos.x)/2, minX, maxX);
        float yy = Mathf.Clamp((player.position.y + mousePos.y)/2, minY, maxY);
        trans.position = new Vector3(xx, yy, trans.position.z);
    }
}
