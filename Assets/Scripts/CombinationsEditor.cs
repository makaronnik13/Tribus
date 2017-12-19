using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

    public class CombinationsEditor : EditorWindow
    {
	private Dictionary<CombineModel.Skills, Color> skillsCollors = new Dictionary<CombineModel.Skills, Color> () {
		{CombineModel.Skills.None, Color.white/2},
		{CombineModel.Skills.Flora, Color.green},
		{CombineModel.Skills.Fauna, Color.yellow},
		{CombineModel.Skills.Minerals, Color.magenta}
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


                foreach (KeyValuePair<CellState, GUIDraggableObject> node in StatesPositions)
                {
                    node.Value.Position += new Vector2(mouseMovementDifference.x, mouseMovementDifference.y);
                }

                lastMousePosition = currentEvent.mousePosition;
                currentEvent.Use();
            }
            
        }

      
        DrowChainsWindow();
            DrawCreatingLine();
        }

    private void DrawCreatingLine()
    {
      

        if (selectedPath!=null && Event.current.type == EventType.keyDown && Event.current.keyCode == KeyCode.Delete)
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
                        start = new Rect(p.Value.Position.x - 100f / 2 + 5 + offset, p.Value.Position.y + 115 / 2f, 100.0f, 115.0f);
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
			Rect drawRect = new Rect (StatesPositions[i].Value.Position.x, StatesPositions[i].Value.Position.y, 140.0f,  155.0f);
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

        if (GUI.Button(new Rect(state.Value.Position.x + 85f, state.Value.Position.y +100, 20, 20), "+"))
        {
            state.Key.AddCombination();
        }


        Rect aim = new Rect(state.Value.Position.x + 100f / 2-8f, state.Value.Position.y-6, 15, 15);

            GUI.Label(aim, new GUIContent(Resources.Load("Icons/button") as Texture2D));

            if (Event.current.type == EventType.mouseUp && aim.Contains(Event.current.mousePosition) && selectedPath != null)
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
                Rect start = new Rect(state.Value.Position.x + offset-size/4, state.Value.Position.y + 110, size, size);

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

			
		GUI.backgroundColor = Color.white * 0.8f;


		if (Selection.activeObject == state.Key)
		{
			GUI.backgroundColor = GUI.backgroundColor * 1.3f;
		}


		Rect drawRect = new Rect (state.Value.Position.x, state.Value.Position.y, 100.0f, 115.0f), dragRect;

        

        GUILayout.BeginArea (drawRect, GUI.skin.GetStyle ("Box"));
		GUILayout.BeginVertical ();
		GUILayout.Label (state.Key.name, GUILayout.ExpandWidth(true));
		GUILayout.Label (AssetPreview.GetAssetPreview(state.Key.prefab),GUILayout.ExpandWidth(true));
		GUILayout.EndVertical ();

      

        dragRect = GUILayoutUtility.GetLastRect ();
		dragRect = new Rect (dragRect.x + state.Value.Position.x, dragRect.y + state.Value.Position.y, dragRect.width, dragRect.height);
		GUILayout.EndArea ();

		if (!DrawButton(state) && Selection.activeObject == state.Key) {
			state.Value.Drag (dragRect);
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

				Rect start = new Rect( state.Value.Position.x - 100f/2+5 +offset, state.Value.Position.y +115/2f,  100.0f, 115.0f);

				

				try 
				{
					CellState aim = StatesPositions.First (k => k.Key == c.ResultState).Key; 
					Vector3 aimPosition = StatesPositions.Find (cell => cell.Key == aim).Value.Position;
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


  }