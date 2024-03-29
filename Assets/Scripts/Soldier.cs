using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public Map map;
    public bool isMoving;
    public float speed = 5f;
    
    private Transform target;
    private Transform currentTerritoryPos;

    private void Start()
    {
        currentTerritoryPos = GameObject.FindGameObjectWithTag("NorthAmerica").transform;
    }

    void Update()
    {
        Transform territory = currentTerritoryPos;
        // Check for a mouse click.
        if (Input.GetMouseButtonDown(0) && !isMoving) // 0 is the left mouse button.
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast and assign the details of the object that the ray collided with to the hit variable
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the object hit has a tag of a neighbouring territory
                List<Transform> adjTerritories = currentTerritoryPos.gameObject.GetComponentInParent<Map>().GetAdjacentTerritories(currentTerritoryPos);
                if (hit.collider.CompareTag(adjTerritories[0].gameObject.tag) || hit.collider.CompareTag(adjTerritories[1].gameObject.tag) || hit.collider.CompareTag(adjTerritories[2].gameObject.tag)) //if we clicked on any of the adjacent countries to the country we're currently on
                {
                    // Set this country as the new target.
                    target = hit.transform;
                    isMoving = true;

                    // Optionally, you can retrieve the "Territory" component to access specific properties.
                    territory = hit.collider.GetComponent<Transform>();
                }
            }
        }

        // If there is a target, move towards it.
        if (target != null && isMoving)
        {
            isMoving = true;
            Vector3 positionToMove = Vector3.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime);
            positionToMove.y = this.transform.position.y; // Keep the soldier at the same height.
            this.transform.position = positionToMove;

            // When the soldier reaches the target, you can clear the target or do other actions.
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                target = null; // Clear the target once reached.
                isMoving = false;
            }

            currentTerritoryPos = territory;
        }
        
    }

}
