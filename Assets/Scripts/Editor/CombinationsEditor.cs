using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

    public class CombinationsEditor : EditorWindow
    {

    private Vector2 screenDelta = Vector2.zero;

	private bool showSprites = false;

	private Dictionary<CombineModel.Skills, Color> skillsCollors = new Dictionary<CombineModel.Skills, Color> () {
		{CombineModel.Skills.None, Color.white/2},
		{CombineModel.Skills.Song, Color.green},
		{CombineModel.Skills.Spell, Color.yellow},
		{CombineModel.Skills.Formula, Color.blue}
	};

	private Dictionary<CombineModel.ResourceType, string> typesIcons = new Dictionary<CombineModel.ResourceType, string> () {
		//{CombineModel.ResourceType.BigAnimal, "Sprites/CellTypesIcons/BigAnimal"},
		//{CombineModel.ResourceType.Fish, "Sprites/CellTypesIcons/Fish"},
		//{CombineModel.ResourceType.GrassAnimal, "Sprites/CellTypesIcons/GrassAnimal"},
		//{CombineModel.ResourceType.GrassBird, "Sprites/CellTypesIcons/Bird"},
		//{CombineModel.ResourceType.Liquids, "Sprites/CellTypesIcons/Liquid"},
		//{CombineModel.ResourceType.Minerals, "Sprites/CellTypesIcons/Mineral"},
		//{CombineModel.ResourceType.PredatorAnimal, "Sprites/CellTypesIcons/PredatorAnimal"},
		//{CombineModel.ResourceType.PredatorBird, "Sprites/CellTypesIcons/PredatorBird"},
		{CombineModel.ResourceType.None, "Sprites/CellTypesIcons/None"}
		//{CombineModel.ResourceType.SmallPlant, "Sprites/CellTypesIcons/SmallPlant"},
		//{CombineModel.ResourceType.Tree, "Sprites/CellTypesIcons/Tree"}
	};

	private Dictionary<CombineModel.Biom, Color> biomsColors = new Dictionary<CombineModel.Biom, Color>(){
		{CombineModel.Biom.Water, new Color(0.5f,0.5f,0.8f)},
		{CombineModel.Biom.Forest, new Color(0.4f,0.8f,0.4f)},
		{CombineModel.Biom.Mountains, new Color(0.5f,0.5f,0.5f)}
	};

    private Combination selectedPath;
	private Vector2 lastMousePosition;

	private List<KeyValuePair<CellState, GUIDraggableObject>> statesPositions = new List<KeyValuePair<CellState, GUIDraggableObject>> ();
	private List<KeyValuePair<CellState, GUIDraggableObject>> StatesPositions
	{
		get
		{
			if(statesPositions.Count == 0)
			{


				int i = 0;
				foreach (CellState state in ProjectStates())
					{
					KeyValuePair<CellState, GUIDraggableObject> kvp = new KeyValuePair<CellState, GUIDraggableObject> (state, new GUIDraggableObject (new Vector2(state.X, state.Y)));
					statesPositions.Add (kvp);
					kvp.Value.onDrag += kvp.Key.Drag;
					i++;
				}
			}
			return statesPositions;
		}
	}

	private static Texture2D backgroundTexture;

      
        private static Texture2D BackgroundTexture
        {
            get
            {
                if (backgroundTexture == null)
                {
                    backgroundTexture = (Texture2D)Resources.Load("Icons/background") as Texture2D;
                    backgroundTexture.wrapMode = TextureWrapMode.Repeat;
                }
                return backgroundTexture;
            }
        }


	[MenuItem("Window/CombinationsEditor")]
	static CombinationsEditor Init()
        {
			CombinationsEditor window = (CombinationsEditor)EditorWindow.GetWindow<CombinationsEditor>("Combinations editor", true, new Type[3] { typeof(Animator), typeof(Console), typeof(SceneView) });
            window.minSize = new Vector2(600, 400);
            window.ShowAuxWindow();
            return window;
        }

        void OnGUI()
        {


	
		Event currentEvent = Event.current;
        if (currentEvent.button == 1 || currentEvent.button == 2)
        {
            if (currentEvent.type == EventType.MouseDown)
            {

                selectedPath = null;
                    lastMousePosition = currentEvent.mousePosition;          
            }
            else if (currentEvent.type == EventType.MouseDrag)
            {

                Vector2 mouseMovementDifference = (currentEvent.mousePosition - lastMousePosition);

               screenDelta += new Vector2(mouseMovementDifference.x, mouseMovementDifference.y);

                lastMousePosition = currentEvent.mousePosition;
                currentEvent.Use();
            }
            
        }

      
		if(Event.current.isKey && Event.current.keyCode == KeyCode.Tab)
		{
			if(Event.current.type == EventType.KeyDown)
			{
				showSprites = true;
				Repaint ();
			}
			if(Event.current.type == EventType.KeyUp)
			{
				showSprites = false;
				Repaint ();
			}
		}

        DrowChainsWindow();
            DrawCreatingLine();
        }

    private void DrawCreatingLine()
    {
      

        if (selectedPath!=null && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Delete)
        {

            foreach (KeyValuePair<CellState, GUIDraggableObject> p in StatesPositions)
            {
                if (p.Key.Combinations.Contains(selectedPath))
                {
                    p.Key.RemoveCombination(p.Key.Combinations.ToList().IndexOf(selectedPath));
                }
            }

            selectedPath= null;
            Repaint();
        }

        if (selectedPath != null)
        {
           
            Rect start = new Rect();
            foreach (KeyValuePair<CellState, GUIDraggableObject> p in StatesPositions)
            {
                float s = 70;
                int i = 0;
                foreach (Combination c in p.Key.Combinations)
                {
                    float offset = s * i / (p.Key.Combinations.Count() - 1);
                    if (p.Key.Combinations.Count() == 1)
                    {
                        offset = 100f / 2 - 5;
                    }
                    if (c== selectedPath)
                    {         
                        start = new Rect(p.Value.Position.x + screenDelta.x - 100f / 2 + 5 + offset, p.Value.Position.y + screenDelta.y + 115 / 2f, 100.0f, 115.0f);
                    }
                   
                    i++;
                }
            }
            Handles.BeginGUI();
            DrawNodeCurve(start, new Rect(Event.current.mousePosition, Vector2.one), skillsCollors[selectedPath.skill], 2);
            Handles.EndGUI();
        }

      
    }

    void DrowChainsWindow()
        {
		

            Rect fieldRect = new Rect(0, 0, position.width, position.height);
            GUI.DrawTextureWithTexCoords(fieldRect, BackgroundTexture, new Rect(0, 0, fieldRect.width / BackgroundTexture.width, fieldRect.height / BackgroundTexture.height));

			DrawPathes();
            
            BeginWindows();

        CellState manipulatingState = null;

		for (int i = 0; i<=StatesPositions.Count-1; i++) 
		{
			Rect drawRect = new Rect (StatesPositions[i].Value.Position.x + screenDelta.x, StatesPositions[i].Value.Position.y + screenDelta.y, 140.0f,  155.0f);
			if(StatesPositions[i].Value.Click(drawRect))
			{
				manipulatingState = StatesPositions [i].Key;
				Selection.activeObject = StatesPositions[i].Key;

			}
		}
			

		for (int i = 0; i<=StatesPositions.Count-1; i++)
			{
			
				DrawStateBox (StatesPositions [i]);
			}

		if(manipulatingState!=null)
		{
			KeyValuePair<CellState, GUIDraggableObject> kvp = StatesPositions.Find(k=>k.Key== manipulatingState);
			StatesPositions.Remove (kvp);
			StatesPositions.Add(kvp);
			Repaint ();
		}



            EndWindows();
    }

    private bool DrawButton(KeyValuePair<CellState, GUIDraggableObject>  state)
    {
        bool containCursor = false;

            Combination[] combinations = state.Key.Combinations;

            float s = 70f;
            int i = 0;

            if (combinations == null)
            {
                combinations = new Combination[0];
            }

        if (GUI.Button(new Rect(state.Value.Position.x + screenDelta.x + 85f, state.Value.Position.y + screenDelta.y + 100, 20, 20), "+"))
        {
            state.Key.AddCombination();
        }


        Rect aim = new Rect(state.Value.Position.x + screenDelta.x + 100f / 2-15f, state.Value.Position.y + screenDelta.y- 18, 30, 30);
		Rect aim2 = new Rect(state.Value.Position.x + screenDelta.x + 100f / 2-10f, state.Value.Position.y + screenDelta.y- 13, 20, 20);

            GUI.Label(aim, new GUIContent(Resources.Load("Icons/button") as Texture2D));
		GUI.color = Color.black;
			GUI.Label(aim2, new GUIContent(Resources.Load(typesIcons[state.Key.type]) as Texture2D));
		GUI.color = Color.white;

            if (Event.current.type == EventType.MouseUp && aim.Contains(Event.current.mousePosition) && selectedPath != null)
            {
        

            foreach (KeyValuePair<CellState, GUIDraggableObject> kvp in statesPositions)
                {
                    if (kvp.Key.Combinations.Contains(selectedPath))
                    {
                    selectedPath.ResultState = state.Key;
                    }
                }

            selectedPath = null;
            }

            if (aim.Contains(Event.current.mousePosition))
            {
            containCursor = true;
            }

            foreach (Combination c in combinations)
            {
                float offset = s * i / (combinations.Count() - 1);
                if (combinations.Count() == 1)
                {
                    offset = 100f / 2;
                }

                float size = 15;
                if (selectedPath == c)
                {
                    size = 20;
                }
                Rect start = new Rect(state.Value.Position.x + screenDelta.x + offset-size/4, state.Value.Position.y + screenDelta.y + 110, size, size);

                Vector3 aimPosition = start.position + Vector2.down * 5f;

                GUI.color = skillsCollors[c.skill];

            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1);

            GUI.Label(start, new GUIContent(Resources.Load("Icons/button") as Texture2D));

                if (start.Contains(Event.current.mousePosition))
                {

                
               /* if (Event.current.type == EventType.MouseDrag)
                {
                    selectedPath = c;
                }*/
                 if (Event.current.type == EventType.MouseDown)
                {
                    if (selectedPath!=c)
                    {
                        selectedPath = c;
                        Repaint();
                    }
                    else
                    {
                        selectedPath = null;
                        Repaint();
                    }
                    
                }

            }


               

                GUI.color = Color.white;
                i++;

                if (start.Contains(Event.current.mousePosition))
                {
                    containCursor = true;
                }
        }

        return containCursor;
    }

    void DrawStateBox(KeyValuePair<CellState, GUIDraggableObject> state)
	{

			
		GUI.backgroundColor = biomsColors[state.Key.Biom];


		if (Selection.activeObject == state.Key)
		{
			GUI.backgroundColor = GUI.backgroundColor * 1.3f;
		}


		Rect drawRect = new Rect (state.Value.Position.x + screenDelta.x, state.Value.Position.y + screenDelta.y, 100.0f, 115.0f);//, dragRect;

        

        GUILayout.BeginArea (drawRect, GUI.skin.GetStyle ("Box"));
		GUILayout.BeginVertical ();
		GUILayout.Label (state.Key.name, GUILayout.ExpandWidth(true));
		if (showSprites) {
			Texture2D texture = new Texture2D(1,1);
			if(state.Key.Sprite)
			{
				texture = state.Key.Sprite.texture;
			}
			GUILayout.Label (texture, GUILayout.ExpandWidth (true));
		} else {
			GUILayout.Label (AssetPreview.GetAssetPreview (state.Key.prefab), GUILayout.ExpandWidth (true));
		}
		GUILayout.EndVertical ();

      

       // dragRect = GUILayoutUtility.GetLastRect ();
	   // dragRect = new Rect (dragRect.x + state.Value.Position.x, dragRect.y + state.Value.Position.y, dragRect.width, dragRect.height);
		GUILayout.EndArea ();

		if (!DrawButton(state) && Selection.activeObject == state.Key) {
			state.Value.Drag (drawRect);
            //selectedPath = null;
			Repaint ();
		}


		GUI.backgroundColor = Color.white;
        
	}

	void DrawPathes()
	{
		foreach (KeyValuePair<CellState, GUIDraggableObject> state in StatesPositions)
		{
			Combination[] combinations = state.Key.Combinations;

			float s =  70;
			int i = 0;

		
			if (combinations == null) 
			{
				combinations = new Combination[0];
			}

			foreach(Combination c in combinations)
			{
				float offset =  s* i/(combinations.Count()-1);
				if(combinations.Count() == 1)
				{
					offset =  48;
				}

				Rect start = new Rect( state.Value.Position.x + screenDelta.x - 100f/2+5 +offset, state.Value.Position.y + screenDelta.y + 115/2f,  100.0f, 115.0f);

				

				try 
				{
					CellState aim = StatesPositions.First (k => k.Key == c.ResultState).Key; 
					Vector3 aimPosition = StatesPositions.Find (cell => cell.Key == aim).Value.Position + screenDelta;
                    Rect end = new Rect(aimPosition.x, aimPosition.y - 115 / 2f, 100.0f, 115.0f); ;
                    Handles.BeginGUI();
                    Color color = skillsCollors[c.skill];
                    if (c == selectedPath)
                    {
                        color = color / 2;
                    }
                    DrawNodeCurve(start, end, color, c.skillLevel);
                    Handles.EndGUI();
                }
				catch
				{
				}
		
				i++;
			}
		}
	}

		void DrawNodeCurve(Rect start, Rect end, Color c, float width)
        {
            float force = 1f;
            Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height / 2, 0);
            Vector3 endPos = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);
            float distanceY = Mathf.Abs(startPos.y - endPos.y);
            float distanceX = Mathf.Abs(startPos.x - endPos.x);
            Vector3 middlePoint = (startPos + endPos) / 2;

            Vector3 startTan1 = startPos;
            Vector3 endTan2 = endPos;
            Vector3 startTan2 = middlePoint;
            Vector3 endTan1 = middlePoint;

            if (startPos.y > endPos.y)
            {
                startTan1 -= Vector3.down * 150;
                endTan2 -= Vector3.up * 150;
			if (startPos.y > endPos.y)
                {
                    endTan1 += Vector3.up * Mathf.Max(distanceY, 50);
                    startTan2 -= Vector3.up * Mathf.Max(distanceY, 50);
                }
                else
                {
                    endTan1 += Vector3.down * Mathf.Max(distanceY, 50);
                    startTan2 -= Vector3.down * Mathf.Max(distanceY, 50);
                }
            }
            else
            {
                startTan1 -= distanceY * Vector3.down / force / 2;
                endTan2 -= distanceY * Vector3.up / force / 2;
                if (startPos.x > endPos.x)
                {
                    endTan1 += distanceX * Vector3.right / force / 2;
                    startTan2 -= distanceX * Vector3.right / force / 2;
                }
                else
                {
                    endTan1 += distanceX * Vector3.left / force / 2;
                    startTan2 -= distanceX * Vector3.left / force / 2;
                }
            }

            Color shadowCol = new Color(0, 0, 0, 0.06f);

            // Draw a shadow
            for (int i = 0; i < 2; i++)
            {
			Handles.DrawBezier(startPos, middlePoint, startTan1, endTan1, shadowCol, null, (i + 1) * 7 *width);
            }
		Handles.DrawBezier(startPos, middlePoint, startTan1, endTan1, c, null, 3*width);

            for (int i = 0; i < 2; i++)
            {
			Handles.DrawBezier(middlePoint, endPos, startTan2, endTan2, shadowCol, null, (i + 1) * 7 *width);
            }
		Handles.DrawBezier(middlePoint, endPos, startTan2, endTan2, c, null, 3*width);
        }

		
	public static List<CellState> ProjectStates()
	{
		List<CellState> st = new List<CellState>();
		string[] guids = AssetDatabase.FindAssets("t:CellState");
		foreach(string s in guids)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(s);
			CellState asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(CellState)) as CellState;
			st.Add (asset);
		}
		if(st.Count==0)
		{
			EditorWindow.GetWindow<CombinationsEditor> ().Close ();
		}
		return st;
	}

	private void OnDisable()
	{
		foreach(KeyValuePair<CellState, GUIDraggableObject> kvp in StatesPositions)
		{
			kvp.Key.Drag (kvp.Value.Position);
            EditorUtility.SetDirty(kvp.Key);
        }
	}

  }