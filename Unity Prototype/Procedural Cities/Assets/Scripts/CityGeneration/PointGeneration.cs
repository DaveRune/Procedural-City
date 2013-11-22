using UnityEngine;
using System.Collections;

public class PointGeneration : MonoBehaviour {
	
	public string seed;
	public int maxX, maxZ;
	public float gridSpacer = 5, range = 5;
  
  private Vector2[,] points;
  
	void Awake () {
    // Initialize the 2D array of pointData
    points = new Vector2[maxX,maxZ];
    
    // Create container for all the points
    GameObject container = new GameObject();
    container.transform.position = Vector3.zero;
    container.name = "Container";
    
		for(int x_ = 0; x_ != maxX; ++x_) {
			for(int z_ = 0; z_ != maxZ; ++z_) {
        // Create point Object
        GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        point.transform.localScale = Vector3.one*0.2f;
        point.renderer.material.color = Color.black;
        point.name = "[" + x_ + "] , [" + z_ + "]";
        point.transform.parent = container.transform;
        
        // Seed the random with the string name and add X and Y
        Random.seed = seed.GetHashCode() * x_+1 * z_+1;
        
        // Position the point based on this new seed.
        Vector3 position = point.transform.position;
        position.x = x_ * gridSpacer + (Random.Range(0.1f, 0.9f) * gridSpacer);
        position.y = 1;
        position.z = z_ * gridSpacer + (Random.Range(0.1f, 0.9f) * gridSpacer);
        point.transform.position = position;
        
        // save the positional Data to a 2D array
        points[x_,z_] = new Vector2(position.x, position.z);
			}
		}
    
    
    // Cycle through all the data and find the neighbouring gridPoints
    // Check if they are within range - If so, connect with a vector.
    for(int x_ = 0; x_ != maxX; ++x_) {
     for(int z_ = 0; z_ != maxZ; ++z_) {
        
        Vector3 thisPoint = points[x_,z_];
        Vector3 nextPoint;
        
        // Check if the neibouring Grid[coord] is within the Grid Index
        // If so, calculate the distance to the neighbouring point
        // If the distance < range, draw a line to connect the two
        if(!(x_-1 < 0) && !(z_+1 > maxZ-1)) {
/*xoo*/   nextPoint = points[x_-1 , z_+1];
/*ooo*/   if(Vector2.Distance(thisPoint, nextPoint) < range ) {
/*ooo*/     DrawRay(thisPoint, nextPoint);
          }
        }
        
        if(!(z_+1 > maxZ-1)) {
 /*oxo*/  nextPoint = points[x_ , z_+1];
 /*ooo*/  if(Vector2.Distance(thisPoint, nextPoint) < range ) {
 /*ooo*/    DrawRay(thisPoint, nextPoint);
          }
        }
        
        if(!(x_+1 > maxX-1) && !(z_+1 > maxZ-1)) {
 /*oox*/  nextPoint = points[x_+1 , z_+1];
 /*ooo*/  if(Vector2.Distance(thisPoint, nextPoint) < range ) {
 /*ooo*/    DrawRay(thisPoint, nextPoint);
          }
        }
        
        if(!(x_-1 < 0)) {
 /*ooo*/  nextPoint = points[x_-1 , z_];
 /*xoo*/  if(Vector2.Distance(thisPoint, nextPoint) < range ) {
 /*ooo*/    DrawRay(thisPoint, nextPoint);
          }
        }
        
        if(!(x_+1 > maxX-1)) {
 /*ooo*/  nextPoint = points[x_+1 , z_];
 /*oox*/  if(Vector2.Distance(thisPoint, nextPoint) < range ) {
 /*ooo*/    DrawRay(thisPoint, nextPoint);
          }
        }
        
        if(!(x_-1 < 0) && !(z_-1 < 0)) {
 /*ooo*/  nextPoint = points[x_-1 , z_-1];
 /*ooo*/  if(Vector2.Distance(thisPoint, nextPoint) < range ) {
 /*xoo*/    DrawRay(thisPoint, nextPoint);
          }
        }
        
        if(!(z_-1 < 0)) {
 /*ooo*/  nextPoint = points[x_ , z_-1];
 /*ooo*/  if(Vector2.Distance(thisPoint, nextPoint) < range ) {
 /*oxo*/    DrawRay(thisPoint, nextPoint);
          }
        }
        
        if(!(x_+1 > maxX-1) && !(z_-1 < 0)) {
 /*ooo*/  nextPoint = points[x_+1 , z_-1];
 /*ooo*/  if(Vector2.Distance(thisPoint, nextPoint) < range ) {
 /*oox*/    DrawRay(thisPoint, nextPoint);
          }
        }
        
      }
    }
    
	}
	
	void DrawRay (Vector2 start, Vector2 end) {
    //if(false) return;
	  Vector3 a = new Vector3(start.x,1,start.y);
    Vector3 b = new Vector3(end.x,1,end.y);
    
    Debug.DrawLine(a,b, Color.red, 100);
	}
}








































