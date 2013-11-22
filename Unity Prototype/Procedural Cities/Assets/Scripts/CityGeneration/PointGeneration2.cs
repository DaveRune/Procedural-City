using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointGeneration2 : MonoBehaviour {
	
  public string seed;
  public int maxX, maxZ;
  public float gridSpacer = 5;
  public int pointsPerGrid = 1;
  public float trackerRange = 10, trackerWeakenRate = 0.9f;
  
  public point[,,] points;
  public List<Traveler> travelers = new List<Traveler>();
  
  private GameObject start;
  
  void Awake () {
    // Initialize the 2D array of pointData
    points = new point[maxX,maxZ,pointsPerGrid];
    
    // Create container for all the points
    GameObject container = new GameObject();
    container.transform.position = Vector3.zero;
    container.name = "Container";
    
    Random.seed = seed.GetHashCode();
    
    for(int x_ = 0; x_ != maxX; ++x_) {
      for(int z_ = 0; z_ != maxZ; ++z_) {
        for(int p_ = 0; p_ != pointsPerGrid; ++p_) {
          // Create point Object
          GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
          point.transform.localScale = Vector3.one*0.5f;
          point.renderer.material.color = Color.black;
          point.name = "[" + x_ + "] , [" + z_ + "]" + "   " + p_;
          point.transform.parent = container.transform;
          
          // Position the point based on this new seed.
          Vector3 position = point.transform.position;
          position.x = x_ * gridSpacer + (Random.Range(0.0f, 1.0f) * gridSpacer);
          position.y = 1;
          position.z = z_ * gridSpacer + (Random.Range(0.0f, 1.0f) * gridSpacer);
          point.transform.position = position;
          
          // save the positional Data to a 2D array
          points[x_,z_,p_] = new point(position.x, position.z);
        }
      }
    }
    
    start = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    start.transform.localScale = Vector3.one * 5;
    
    Vector3 newPos = start.transform.position;
    Vector2 coord = new Vector2(Random.Range(0,maxX), Random.Range(0,maxZ));
    newPos.x = coord.x * gridSpacer + (Random.Range(0.0f, 1.0f) * gridSpacer);
    newPos.y = 1;
    newPos.z = coord.y * gridSpacer + (Random.Range(0.0f, 1.0f) * gridSpacer);
    
    // Create the ball, which we will initialize with this class.
    start.transform.position = newPos;
    start.AddComponent<TentacleOrigin>().init(this);
    
    //Grow(newPos, coord);
  }
  
  
  public void Grow (Vector3 origin, Vector2 coord) {
    //Debug.Log(coord);
    
    float distance = Mathf.Infinity;
    Vector2 tPos = new Vector2(origin.x, origin.z);
    point targetPoint = new point();
    
    #region Search Boundaries 
    for(int i = 0; i != pointsPerGrid; ++i) {
      float tDistance = 0;
      point currentPoint = new point();
      // Same Cell Check
      if((coord.x >= 0 && coord.x < maxX) && (coord.y >= 0 && coord.y < maxZ)) {
        currentPoint = points[(int)coord.x,(int)coord.y,i];
        tDistance = Vector2.Distance(currentPoint.pos, tPos);
        if(currentPoint.active && tDistance < distance) {
          distance = tDistance;
          targetPoint = points[(int)coord.x,(int)coord.y,i];
        }
      } else continue;
      // Border Cell Checks
      if(coord.x-1 >= 0 && coord.y+1 < maxZ) {
        currentPoint = points[(int)coord.x-1,(int)coord.y+1,i];
        tDistance = Vector2.Distance(currentPoint.pos, tPos);
        if(currentPoint.active && tDistance < distance) {
          distance = tDistance;
          targetPoint = points[(int)coord.x-1,(int)coord.y+1,i];
        }
      }
      if(coord.y+1 < maxZ) {
        currentPoint = points[(int)coord.x,(int)coord.y+1,i];
        tDistance = Vector2.Distance(currentPoint.pos, tPos);
        if(currentPoint.active && tDistance < distance) {
          distance = tDistance;
          targetPoint = points[(int)coord.x,(int)coord.y+1,i];
        }
      }
      if(coord.x+1 < maxX && coord.y+1 < maxZ) {
        currentPoint = points[(int)coord.x+1,(int)coord.y+1,i];
        tDistance = Vector2.Distance(currentPoint.pos, tPos);
        if(currentPoint.active && tDistance < distance) {
          distance = tDistance;
          targetPoint = points[(int)coord.x+1,(int)coord.y+1,i];
        }
      }
      if(coord.x-1 >= 0) {
        currentPoint = points[(int)coord.x-1,(int)coord.y,i];
        tDistance = Vector2.Distance(currentPoint.pos, tPos);
        if(currentPoint.active && tDistance < distance) {
          distance = tDistance;
          targetPoint = points[(int)coord.x-1,(int)coord.y,i];
        }
      }
      if(coord.x+1 < maxX) {
        currentPoint = points[(int)coord.x+1,(int)coord.y,i];
        tDistance = Vector2.Distance(currentPoint.pos, tPos);
        if(currentPoint.active && tDistance < distance) {
          distance = tDistance;
          targetPoint = points[(int)coord.x+1,(int)coord.y,i];
        }
      }
      if(coord.x-1 >= 0 && coord.y-1 >= 0) {
        currentPoint = points[(int)coord.x-1,(int)coord.y-1,i];
        tDistance = Vector2.Distance(currentPoint.pos, tPos);
        if(currentPoint.active && tDistance < distance) {
          distance = tDistance;
          targetPoint = points[(int)coord.x-1,(int)coord.y-1,i];
        }
      }
      if(coord.y-1 >= 0) {
        currentPoint = points[(int)coord.x,(int)coord.y-1,i];
        tDistance = Vector2.Distance(currentPoint.pos, tPos);
        if(currentPoint.active && tDistance < distance) {
          distance = tDistance;
          targetPoint = points[(int)coord.x,(int)coord.y-1,i];
        }
      }
      if(coord.x+1 < maxX && coord.y-1 >= 0) {
        currentPoint = points[(int)coord.x+1,(int)coord.y-1,i];
        tDistance = Vector2.Distance(currentPoint.pos, tPos);
        if(currentPoint.active && tDistance < distance) {
          distance = tDistance;
          targetPoint = points[(int)coord.x+1,(int)coord.y-1,i];
        }
      }
    }
    #endregion Search Boundaries 
    
    //Debug.Log(distance + "     " + targetPoint.pos);
    
    targetPoint.active = false;

    if(distance != Mathf.Infinity)DrawRay(new Vector2(origin.x, origin.z), targetPoint.pos);
  }
  
  
  public Vector3 FindNext (Vector3 origin, Vector2 coord, Traveler t, float hopDistance) {
    float distance = Mathf.Infinity;
    Vector2 tPos = new Vector2(origin.x, origin.z);
    point targetPoint = new point();
    
    #region Search Boundaries 
    for(int i = 0; i != pointsPerGrid; ++i) {
      float tDistance = 0;
      point currentPoint = new point();
      // Same Cell Check
      if((coord.x >= 0 && coord.x < maxX) && (coord.y >= 0 && coord.y < maxZ)) {
        currentPoint = points[(int)coord.x,(int)coord.y,i];
        tDistance = Vector2.Distance(currentPoint.pos, tPos);
        if(currentPoint.active && tDistance < distance) {
          distance = tDistance;
          targetPoint = points[(int)coord.x,(int)coord.y,i];
        }
      } else continue;
      // Border Cell Checks
      if(coord.x-1 >= 0 && coord.y+1 < maxZ) {
        currentPoint = points[(int)coord.x-1,(int)coord.y+1,i];
        tDistance = Vector2.Distance(currentPoint.pos, tPos);
        if(currentPoint.active && tDistance < distance) {
          distance = tDistance;
          targetPoint = points[(int)coord.x-1,(int)coord.y+1,i];
        }
      }
      if(coord.y+1 < maxZ) {
        currentPoint = points[(int)coord.x,(int)coord.y+1,i];
        tDistance = Vector2.Distance(currentPoint.pos, tPos);
        if(currentPoint.active && tDistance < distance) {
          distance = tDistance;
          targetPoint = points[(int)coord.x,(int)coord.y+1,i];
        }
      }
      if(coord.x+1 < maxX && coord.y+1 < maxZ) {
        currentPoint = points[(int)coord.x+1,(int)coord.y+1,i];
        tDistance = Vector2.Distance(currentPoint.pos, tPos);
        if(currentPoint.active && tDistance < distance) {
          distance = tDistance;
          targetPoint = points[(int)coord.x+1,(int)coord.y+1,i];
        }
      }
      if(coord.x-1 >= 0) {
        currentPoint = points[(int)coord.x-1,(int)coord.y,i];
        tDistance = Vector2.Distance(currentPoint.pos, tPos);
        if(currentPoint.active && tDistance < distance) {
          distance = tDistance;
          targetPoint = points[(int)coord.x-1,(int)coord.y,i];
        }
      }
      if(coord.x+1 < maxX) {
        currentPoint = points[(int)coord.x+1,(int)coord.y,i];
        tDistance = Vector2.Distance(currentPoint.pos, tPos);
        if(currentPoint.active && tDistance < distance) {
          distance = tDistance;
          targetPoint = points[(int)coord.x+1,(int)coord.y,i];
        }
      }
      if(coord.x-1 >= 0 && coord.y-1 >= 0) {
        currentPoint = points[(int)coord.x-1,(int)coord.y-1,i];
        tDistance = Vector2.Distance(currentPoint.pos, tPos);
        if(currentPoint.active && tDistance < distance) {
          distance = tDistance;
          targetPoint = points[(int)coord.x-1,(int)coord.y-1,i];
        }
      }
      if(coord.y-1 >= 0) {
        currentPoint = points[(int)coord.x,(int)coord.y-1,i];
        tDistance = Vector2.Distance(currentPoint.pos, tPos);
        if(currentPoint.active && tDistance < distance) {
          distance = tDistance;
          targetPoint = points[(int)coord.x,(int)coord.y-1,i];
        }
      }
      if(coord.x+1 < maxX && coord.y-1 >= 0) {
        currentPoint = points[(int)coord.x+1,(int)coord.y-1,i];
        tDistance = Vector2.Distance(currentPoint.pos, tPos);
        if(currentPoint.active && tDistance < distance) {
          distance = tDistance;
          targetPoint = points[(int)coord.x+1,(int)coord.y-1,i];
        }
      }
    }
    #endregion Search Boundaries 
    
    if(distance > hopDistance){
      travelers.Remove(t);
      return Vector3.zero;
    }
    
    if(distance == Mathf.Infinity) {
      travelers.Remove(t);
      return Vector3.zero;
    }
    
    if(distance != Mathf.Infinity)DrawRay(new Vector2(origin.x, origin.z), targetPoint.pos);
    
    targetPoint.active = false;
    
    return new Vector3(targetPoint.pos.x, 0, targetPoint.pos.y);
  }
  
  
  public void CreateTraveler (Vector3 startPos, bool showSelf) {
    Traveler t = new Traveler();
    t.Initialize(this, startPos, showSelf, trackerRange, trackerWeakenRate);
    
    travelers.Add(t);
  }
  
  
  public void RequestDeletion (Traveler t) {
    travelers.Remove(t);
  }
  
  
	void DrawRay (Vector2 start, Vector2 end) {
    if(false) return;
    
    Vector3 a = new Vector3(start.x,1,start.y);
    Vector3 b = new Vector3(end.x,1,end.y);
    
    Debug.DrawLine(a,b, Color.red, 600);
	}
  
  
  void Update () {
    if(travelers.Count != 0)
    {
      for(int i = travelers.Count-1; i >= 0; --i)
        travelers[i].UpdatePosition();
    }
    
    float speed = 100 * Time.deltaTime;
    
    Vector3 newPos = start.transform.position;
    
    if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
      newPos.z += speed;
    }
    if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
      newPos.z -= speed;
    }
    if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
      newPos.x -= speed;
    }
    if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
      newPos.x += speed;
    }
    
    start.transform.position = newPos;
    
  }
}


public class Traveler {
  
  PointGeneration2 manager;
  GameObject self;
  Vector3 targetPosition;
  float hopDistance;
  float weakenRate;
  
  public void Initialize (PointGeneration2 manager, Vector3 startPos, bool showSelf, float hopDistance, float weakenRate = 0.9f) {
    if(showSelf) {
      self = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      self.transform.localScale = Vector3.one*0.8f;
      self.renderer.material.color = Color.green;
      self.name = "Tracker";
    } else {
      self = new GameObject();
      self.name = "Tracker";
    }
    
    self.transform.position = startPos;
    this.manager = manager;
    this.hopDistance = hopDistance;
    this.weakenRate = weakenRate;
    
    // Calculate current position and coordinate within the grid. send it to the manager class to find the next local point
    Vector3 myPos = self.transform.position;
    Vector2 coord = new Vector2();
    coord.x = Mathf.Floor(myPos.x / manager.gridSpacer);
    coord.y = Mathf.Floor(myPos.z / manager.gridSpacer);
    targetPosition = manager.FindNext(startPos, coord, this, hopDistance);
    
    if(targetPosition == Vector3.zero) {
      GameObject.Destroy(self);
      manager.RequestDeletion(this);
      return;
    }
  }
  
  public void UpdatePosition () {
    
    if(targetPosition == Vector3.zero) {
      GameObject.Destroy(self);
      manager.RequestDeletion(this);
      return;
    }
    
    self.transform.position = Vector3.MoveTowards(self.transform.position, targetPosition, Time.deltaTime * 5);
    
    if(self.transform.position == targetPosition){
      Vector3 myPos = self.transform.position;
      Vector2 coord = new Vector2();
      coord.x = Mathf.Floor(myPos.x / manager.gridSpacer);
      coord.y = Mathf.Floor(myPos.z / manager.gridSpacer);
      targetPosition = manager.FindNext(myPos, coord, this, hopDistance);
      hopDistance *= weakenRate;
    }
  }
}


public class point {
  public Vector2 pos;
  public bool active = true;
  
  public point() {}
  
  public point(float x, float y) {
    pos = new Vector2(x,y);
  }
  
}


class TentacleOrigin : MonoBehaviour
{
  
  PointGeneration2 manager;
  Vector3 prevPos = new Vector3();
  
  public void init (PointGeneration2 manager) {
    this.manager = manager;
  }
  
  void Update () {
    if(transform.position != prevPos)
      UpdateOrigin();
    
    prevPos = transform.position;
    
    
    if(Input.GetKeyDown(KeyCode.Space)) {
      manager.CreateTraveler(transform.position, true);
    }
    
  }
  
  void UpdateOrigin () {
    
    Vector3 myPos = transform.position;
    Vector2 coord = new Vector2();
    
    coord.x = Mathf.Floor(myPos.x / manager.gridSpacer);
    coord.y = Mathf.Floor(myPos.z / manager.gridSpacer);
    
    //manager.Grow(myPos, coord);
  }
  
}







































