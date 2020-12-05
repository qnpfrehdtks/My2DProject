using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRespawnObject
{
    int RespawnNumber { get; set; }
    void InitializeRespawn(ObjectRespawner respawner);
    void OnRespawn(int idx);
}

/// <summary>
/// 카메라에 닿으면 투명해지는 오브젝트용
/// </summary>
public interface ICameraObstacle
{
    // 투명 상태인가? 투명 상태인데 또 투명 만들 필요가 없어서 bool 변수 둠
    bool IsFade { get; set; }
    void EnterCollisionToCameraRay();
    void ExitCollisionToCameraRay();
}

public interface IPoolingObject
{ 
     int StartCount { get; set; }
     void Initialize(GameObject factory);
     void OnPushToQueue();
     void OnPopFromQueue();
}

public interface IInteractableObject
{
    void OnEnterCollision(CharacterBase character, Vector3 hitPos);
    void OnExitCollision(CharacterBase character);
}

public interface IRayCastObject
{

}
//{
//    bool isCanHit { get; set; }

//    void ExitRayHovering();
//    void OnBeginRayHovering(Vector3 lazerPostion);
//    void OnRayHovering(Vector3 lazerPostion);
//    void OnRayClickPressd(CharacterBase character);
//    void OnRayClickUp(CharacterBase character);
//    void OnRayLongPressed(CharacterBase character);


public interface IControllerObserver
{
    TouchPhase keyCode { get; set; }
    void OnMoved(Touch touch, CharacterBase character);
    void OnTouchStationary(Touch touch, CharacterBase character);
    void OnTouchBegan(Touch touch, CharacterBase character);
    void OnTouchEnded(Touch touch, CharacterBase character);
}