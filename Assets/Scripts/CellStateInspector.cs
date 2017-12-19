using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor (typeof(CellState))]
public class CellStateInspector : Editor
{

	private ReorderableList combinationsList, buffsList, incomesList;
	private SerializedObject _serializedObject;
	private CellState state;

	private void OnEnable ()
	{
		state = (CellState)target;
		_serializedObject = new SerializedObject (state);
		combinationsList = new ReorderableList (_serializedObject, _serializedObject.FindProperty ("combinations"), true, true, true, true);
		combinationsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
					
			rect.y += 2;
			((CellState)target).Combinations [index].ResultState = (CellState)EditorGUI.ObjectField (new Rect (rect.x + 3 * rect.width / 5 + 10, rect.y, 2 * rect.width / 5 - 10, EditorGUIUtility.singleLineHeight), ((CellState)target).Combinations [index].ResultState, typeof(CellState), false);
			((CellState)target).Combinations [index].skill = (CombineModel.Skills)EditorGUI.EnumPopup (new Rect (rect.x + rect.width / 5, rect.y, 2 * rect.width / 5, EditorGUIUtility.singleLineHeight), ((CellState)target).Combinations [index].skill);
			((CellState)target).Combinations [index].skillLevel = EditorGUI.IntField (new Rect (rect.x, rect.y, rect.width / 5 - 5, EditorGUIUtility.singleLineHeight), ((CellState)target).Combinations [index].skillLevel);
		};
		combinationsList.drawHeaderCallback = (Rect r) => {
			EditorGUI.LabelField (new Rect (r.x, r.y, r.width, EditorGUIUtility.singleLineHeight), "combninations");
		};
		combinationsList.onAddCallback = (ReorderableList l) => {
			state.AddCombination ();
			Repaint ();
			SearchableEditorWindow.GetWindow (typeof(EditorWindow)).Repaint ();
		};
		combinationsList.onRemoveCallback = (ReorderableList l) => {
			state.RemoveCombination (l.index);
		};

		incomesList = new ReorderableList (_serializedObject, _serializedObject.FindProperty ("income"), true, true, true, true);
		incomesList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {

			rect.y += 2;
			((CellState)target).income [index].resource = (CombineModel.GameResource)EditorGUI.EnumPopup (new Rect (rect.x, rect.y, rect.width / 2 - 5, EditorGUIUtility.singleLineHeight), ((CellState)target).income [index].resource);
			((CellState)target).income [index].value = EditorGUI.IntField (new Rect (rect.x + rect.width / 2, rect.y, rect.width / 2, EditorGUIUtility.singleLineHeight), ((CellState)target).income [index].value);
		};
		incomesList.drawHeaderCallback = (Rect r) => {
			EditorGUI.LabelField (new Rect (r.x, r.y, r.width, EditorGUIUtility.singleLineHeight), "income");
		};
		incomesList.onAddCallback = (ReorderableList l) => {
			state.AddIncome ();
			_serializedObject.Update ();
			Repaint ();
		};
		incomesList.onRemoveCallback = (ReorderableList l) => {
			state.RemoveIncome (l.index);
		};
			

		buffsList = new ReorderableList (_serializedObject, _serializedObject.FindProperty ("buffs"), true, true, true, true);


		buffsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			EditorGUI.PropertyField(rect, _serializedObject.FindProperty("buffs").GetArrayElementAtIndex(index));


			state.buffs[index].characteristic = EditorGUILayout.EnumPopup(state.buffs[index].characteristic);

		
			switch(state.buffs[index].characteristic)
		{
		case CellBuff.CellCharacteristic.Biom:
				EditorGUILayout.PropertyField(new SerializedObject(state.buffs[index]).FindProperty("bioms"), GUIContent.none);
			
			break;
		case CellBuff.CellCharacteristic.State:
			//EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("states"), GUIContent.none);
			
			break;
		case CellBuff.CellCharacteristic.Resource:
			//EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("resoureTypes"), GUIContent.none);
			
			break;
			}
		};
		buffsList.drawHeaderCallback = (Rect r) => {
			EditorGUI.LabelField (new Rect (r.x, r.y, r.width, EditorGUIUtility.singleLineHeight), "buffs");
		};
		buffsList.onAddCallback = (ReorderableList l) => {
			state.AddBuff ();
			Repaint ();
			SearchableEditorWindow.GetWindow (typeof(EditorWindow)).Repaint ();
		};
		buffsList.onRemoveCallback = (ReorderableList l) => {
			state.RemoveBuff (l.index);
		};

	}

	public override void OnInspectorGUI ()
	{
		//state.combinations = new Combination[0];
		//state.income.incomeValues = new Inkome.SimpleIncome[0];

		EditorGUI.BeginChangeCheck ();
				
		state.name = EditorGUILayout.TextField (state.name, GUILayout.Height (15));
		state.prefab = (GameObject)EditorGUILayout.ObjectField (state.prefab, typeof(GameObject), false);
				
		GUILayout.Label (AssetPreview.GetAssetPreview (state.prefab), GUILayout.Width (150), GUILayout.Height (150));

		GUILayout.Space (EditorGUIUtility.singleLineHeight);
		_serializedObject.Update ();
		combinationsList.DoLayoutList ();
		incomesList.DoLayoutList ();
		buffsList.elementHeight = EditorGUI.GetPropertyHeight (_serializedObject.FindProperty ("buffs").GetArrayElementAtIndex (0));
		buffsList.DoLayoutList ();
		_serializedObject.ApplyModifiedProperties ();
	}
}