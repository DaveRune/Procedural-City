using UnityEngine;
using System.Collections;

public class River1 : MonoBehaviour {
 
  Vector2 startDirection;
  Vector2 previousPoint, currentPoint;
  
  void Awake () {
    // Create random 2D Unit Vector
    float r = Random.Range(0.0f,360.0f);
    float angle = r * Mathf.PI/180;
    float x = Mathf.Cos(angle);
    float y = Mathf.Sin(angle);
    startDirection = new Vector2(x,y);
    
    InvokeRepeating("Step", 0.2f, 0.2f);
  }
  
  void Step () {
    Vector2 direction = startDirection*Random.Range(1.0f,3.0f);
    currentPoint = previousPoint + direction;
    DrawRay(previousPoint,currentPoint);
    
    
    previousPoint = currentPoint;
  }
  
  void DrawRay (Vector2 start, Vector2 end) {
    if(false) return;
    
    Vector3 a = new Vector3(start.x,1,start.y);
    Vector3 b = new Vector3(end.x,1,end.y);
    
    Debug.DrawLine(a,b, Color.red, 600);
 }
  
}
