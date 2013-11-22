using UnityEngine;
using System.Collections;

public class River2 : MonoBehaviour {
 
  Vector3 previousPoint, currentPoint;
  Vector3 prevA, prevB, curA, curB;
  bool first = true;
  private int drawTime = 200;
  
  public float riverWidth = 10;
  
  public float timescale = 1;
  
  void Awake () {
    // Create random 2D Unit Vector
    float r = Random.Range(0.0f,360.0f);
    float angle = r * Mathf.PI/180;
    float x = Mathf.Cos(angle);
    float y = Mathf.Sin(angle);
    Vector2 startDirection = new Vector2(x,y);
    transform.eulerAngles = new Vector3(startDirection.x, startDirection.y, 0);
    
    InvokeRepeating("Step", 0.2f, 0.2f);
  }
  
  void Step () {
    previousPoint = transform.position;
    
    float newRiverWidth = riverWidth + Random.Range(-5,5);;
    
    // Move the pointer forwards a random amount with a bit of random angle
    transform.position += transform.forward * (Random.Range(5,10) + (newRiverWidth * 0.5f));
    Vector3 tempEuler = transform.eulerAngles;
    tempEuler.y += Random.Range(-10.0f,10.0f);
    //tempEuler.y = Curve() * 360;
    tempEuler.x = 0;
    tempEuler.z = 0;
    transform.eulerAngles = tempEuler;
    currentPoint = transform.position;
    
    // Calculate the direction traveled and find the perpendicular
    Vector3 tOLD = previousPoint;
    Vector3 tNEW = currentPoint;
    tOLD.y = 0;
    tNEW.y = 0;
    Vector3 direction = tNEW - tOLD;
    
    direction.Normalize();
    direction*= (riverWidth + (Mathf.Sin(Time.time)*15));
    
    direction = new Vector3(-direction.z, 0, direction.x);
    
    if(first)
    {
      prevA = transform.position + direction;
      prevB = transform.position - direction;
      
      Debug.DrawLine(Vector3.zero+direction, prevA, Color.red, drawTime);
      Debug.DrawLine(Vector3.zero-direction, prevB, Color.red, drawTime);
    } else {
      
      curA = transform.position + direction;
      curB = transform.position - direction; 
      
      Debug.DrawRay(transform.position, direction, Color.red, drawTime);
      Debug.DrawRay(transform.position, -direction, Color.red, drawTime);
      
      Debug.DrawLine(prevA, curA, Color.red, drawTime);
      Debug.DrawLine(prevB, curB, Color.red, drawTime);
      Debug.DrawLine(prevA, curB, Color.red, drawTime);
      
      prevA = curA;
      prevB = curB;
      
    }
    
    
    
    
    
    Debug.DrawLine(previousPoint, currentPoint, Color.blue, drawTime);
    first = false;
  }
  
  void DrawRay (Vector2 start, Vector2 end) {
    if(false) return;
    
    Vector3 a = new Vector3(start.x,1,start.y);
    Vector3 b = new Vector3(end.x,1,end.y);
    
    Debug.DrawLine(a,b, Color.red, 600);
 }
  
  float Curve () {
    float a = Mathf.Sin(Time.deltaTime * 50);
    float b = Mathf.Sin(Time.deltaTime * 50);
    float c = Mathf.Sin(Time.deltaTime * 50);
    
    float curveTightness = 10;
    
    float d = Mathf.Sin(Time.deltaTime * curveTightness) * 10;
    float e = Mathf.Sin(Time.deltaTime * curveTightness) * 20;
    float f = Mathf.Sin(Time.deltaTime * curveTightness) * 30;
    
    //return (a-b+c-d+e-f)/6;
    //return (a+b+c)/3;
    return a;
    
  }
 
  void Update () {
    Time.timeScale = timescale;
  }
  
}
