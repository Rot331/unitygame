using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerController))]

public class Player : LivingEntity
{
    public float moveSpeed = 5;

	Camera viewCamera;
	PlayerController controller;
    Rigidbody rigidbodyCache;


    protected override void Start ()
	{
		base.Start ();
        rigidbodyCache = GetComponent<Rigidbody>();
        controller = GetComponent<PlayerController> ();
		viewCamera = Camera.main;
    }

	void Update ()
	{
		// 移動用入力
		Vector3 moveInput = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
		// TODO why normailze?
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;
		controller.Move (moveVelocity);
	
		//// プレイヤーの向きを変更するロジック
		//// カメラからマウスポイントに向かう光線
		//Ray ray = viewCamera.ScreenPointToRay (Input.mousePosition);
		//Plane groundPlane = new Plane (Vector3.up, Vector3.zero);
		//float rayDistance;
		//if (groundPlane.Raycast (ray, out rayDistance))
  //      { // 平面と光線が交差したところまでの光線の距離が取得できる。
		//	Vector3 point = ray.GetPoint (rayDistance); // Ray光線は、直線がずっと続いているので、分かった距離の座標を取得
		//	Debug.DrawLine (ray.origin, point, Color.red);
		//	controller.LookAt (point);
		//}

	}
}
