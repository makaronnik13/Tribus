using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Grids/Resource")]
public class GameResource : ScriptableObject {
    public string resName;
    [AssetsOnly, InlineEditor(InlineEditorModes.LargePreview)]
    public Sprite sprite;
    public bool incoming;
    public bool showInPanel;
    public Color color = Color.white;
}
