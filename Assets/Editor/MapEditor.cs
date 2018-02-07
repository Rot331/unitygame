using UnityEngine;
using UnityEditor;
using System.Collections;

// Unityエディタの拡張。
// これは、ゲームではなく、Unityエディタを拡張しているコードで、
// MapGeneratorのスライダーなどを操作したら、OnInspectorGUIが呼ばれて、
// 再生成する。
[CustomEditor (typeof(MapGenerator))]
public class MapEditor : Editor
{
	// エディタで値を修正したら、再生成
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		MapGenerator map = target as MapGenerator;

		map.GenerateMap ();
	}
}
