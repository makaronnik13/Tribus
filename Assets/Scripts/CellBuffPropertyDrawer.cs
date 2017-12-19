using UnityEditor;
using UnityEngine;

// IngredientDrawer
[CustomPropertyDrawer(typeof(CellBuff))]
public class CellBuffPropertyDrawer : PropertyDrawer
{
	// Draw the property inside the given rect
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{


		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty(position, label, property);
		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		// Calculate rects
		var amountRect = new Rect(position.x, position.y, 30, position.height);
		var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
		var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);


		// Draw fields - passs GUIContent.none to each so they are drawn without labels
		EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("radius"), GUIContent.none);
		EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("buffType"), GUIContent.none);


		// Set indent back to what it was
		EditorGUI.indentLevel = indent;


		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
		return 45;
	}
}