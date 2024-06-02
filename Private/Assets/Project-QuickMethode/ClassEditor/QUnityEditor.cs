using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using Object = UnityEngine.Object;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif

#if UNITY_EDITOR

///<summary>Used for any Unity Editor</summary>
///<remarks><b>UnityEditorOnly</b></remarks>
public class QUnityEditor
{
    #region ==================================== GUI Group

    #region ------------------------------------ Chance Check

    public static void SetChanceCheckBegin()
    {
        EditorGUI.BeginChangeCheck();
    }

    public static bool SetChanceCheckEnd()
    {
        return EditorGUI.EndChangeCheck();
    }

    #endregion

    #region ------------------------------------ Disable Group

    public static void SetDisableGroupBegin(bool Disable = true)
    {
        EditorGUI.BeginDisabledGroup(Disable);
    }

    public static void SetDisableGroupEnd()
    {
        EditorGUI.EndDisabledGroup();
    }

    #endregion

    #region ------------------------------------ Horizontal

    public static void SetHorizontalBegin()
    {
        GUILayout.BeginHorizontal();
    }

    public static void SetHorizontalEnd()
    {
        GUILayout.EndHorizontal();
    }

    #endregion

    #region ------------------------------------ Vertical

    public static void SetVerticalBegin()
    {
        GUILayout.BeginVertical();
    }

    public static void SetVerticalEnd()
    {
        GUILayout.EndVertical();
    }

    #endregion

    #region ------------------------------------ Scroll View

    public static Vector2 SetScrollViewBegin(Vector2 ScrollPos, params GUILayoutOption[] GUILayoutOption)
    {
        return EditorGUILayout.BeginScrollView(ScrollPos, GUILayoutOption);
    }

    public static void SetScrollViewEnd()
    {
        EditorGUILayout.EndScrollView();
    }

    #endregion

    #endregion

    #region ==================================== GUI Primary

    #region ------------------------------------ Indent Level : Can be understand at TAB in inspector!!

    public static int IndentLevel
    {
        get => EditorGUI.indentLevel;
        set => EditorGUI.indentLevel = value;
    }

    #endregion

    #region ------------------------------------ Dirty: Modify an object without creating an undo entry, but still ensure the change is registered and not lost...

    //Data in 'ScriptableObject' will not be saved if not called 'Dirty' at the end of 'OnInspectorGUI()' methode!

    public static void SetDirty(Object Target)
    {
        EditorUtility.SetDirty(Target);
    }

    #endregion

    #region ------------------------------------ Label

    public static void SetLabel(string Label, GUIStyle GUIStyle = null, params GUILayoutOption[] GUILayoutOption)
    {
        if (GUIStyle == null)
            GUILayout.Label(Label, GUILayoutOption);
        else
            GUILayout.Label(Label, GUIStyle, GUILayoutOption);
    }

    public static void SetLabel(string Label, Rect Rect)
    {
        EditorGUI.LabelField(Rect, Label);
    }

    public static void SetLabel(Sprite Sprite, params GUILayoutOption[] GUILayoutOption)
    {
        GUILayout.Label(GetGUIContentImage(Sprite), GUILayoutOption);
    }

    #endregion

    #region ------------------------------------ Button

    //Button Normal

    public static bool SetButton(string Label, GUIStyle GUIStyle = null, params GUILayoutOption[] GUILayoutOption)
    {
        if (GUIStyle == null)
            return GUILayout.Button(Label, GUILayoutOption);
        else
            return GUILayout.Button(Label, GUIStyle, GUILayoutOption);
    }

    public static bool SetButton(Sprite Sprite, params GUILayoutOption[] GUILayoutOption)
    {
        return GUILayout.Button(GetGUIContentImage(Sprite), GUILayoutOption);
    }

    //Button Hold

    public static bool SetButtonHold(string Label, GUIStyle GUIStyle = null, params GUILayoutOption[] GUILayoutOption)
    {
        if (GUIStyle == null)
            return GUILayout.RepeatButton(Label, GUILayoutOption);
        else
            return GUILayout.RepeatButton(Label, GUIStyle, GUILayoutOption);
    }

    public static bool SetButtonHold(Sprite Sprite, params GUILayoutOption[] GUILayoutOption)
    {
        return GUILayout.RepeatButton(GetGUIContentImage(Sprite), GUILayoutOption);
    }

    #endregion

    #region ------------------------------------ Field

    #region Field Special

    public static T SetFieldScriptableObject<T>(T Value, params GUILayoutOption[] GUILayoutOption) where T : ScriptableObject
    {
        return (T)EditorGUILayout.ObjectField("", Value, typeof(T), true, GUILayoutOption);
    }

    public static T SetFieldMonoBehaviour<T>(T Value, params GUILayoutOption[] GUILayoutOption) where T : MonoBehaviour
    {
        return (T)EditorGUILayout.ObjectField("", Value, typeof(T), true, GUILayoutOption);
    }

    public static T SetFieldTextAsset<T>(T Value, params GUILayoutOption[] GUILayoutOption) where T : TextAsset
    {
        return (T)EditorGUILayout.ObjectField("", Value, typeof(T), true, GUILayoutOption);
    }

    #endregion

    #region Field Text

    //String

    public static string SetField(string Value, GUIStyle GUIStyle = null, params GUILayoutOption[] GUILayoutOption)
    {
        if (GUIStyle == null)
            return EditorGUILayout.TextField("", Value, GUILayoutOption);
        else
            return EditorGUILayout.TextField("", Value, GUIStyle, GUILayoutOption);
    }

    public static string SetFieldPassword(string Value, GUIStyle GUIStyle = null, params GUILayoutOption[] GUILayoutOption)
    {
        if (GUIStyle == null)
            return EditorGUILayout.PasswordField("", Value, GUILayoutOption);
        else
            return EditorGUILayout.PasswordField("", Value, GUIStyle, GUILayoutOption);
    }

    //Value

    public static int SetField(int Value, GUIStyle GUIStyle = null, params GUILayoutOption[] GUILayoutOption)
    {
        if (GUIStyle == null)
            return EditorGUILayout.IntField("", Value, GUILayoutOption);
        else
            return EditorGUILayout.IntField("", Value, GUIStyle, GUILayoutOption);
    }

    public static long SetField(long Value, GUIStyle GUIStyle = null, params GUILayoutOption[] GUILayoutOption)
    {
        if (GUIStyle == null)
            return EditorGUILayout.LongField("", Value, GUILayoutOption);
        else
            return EditorGUILayout.LongField("", Value, GUIStyle, GUILayoutOption);
    }

    public static float SetField(float Value, GUIStyle GUIStyle = null, params GUILayoutOption[] GUILayoutOption)
    {
        if (GUIStyle == null)
            return EditorGUILayout.FloatField("", Value, GUILayoutOption);
        else
            return EditorGUILayout.FloatField("", Value, GUIStyle, GUILayoutOption);
    }

    public static double SetField(double Value, GUIStyle GUIStyle = null, params GUILayoutOption[] GUILayoutOption)
    {
        if (GUIStyle == null)
            return EditorGUILayout.DoubleField("", Value, GUILayoutOption);
        else
            return EditorGUILayout.DoubleField("", Value, GUIStyle, GUILayoutOption);
    }

    public static bool SetField(bool Value, params GUILayoutOption[] GUILayoutOption)
    {
        return EditorGUILayout.Toggle(Value, GUILayoutOption);
    } //Toggle

    #endregion

    #region Field Vector

    //Vector2

    public static Vector2 SetField(Vector2 Value, params GUILayoutOption[] GUILayoutOption)
    {
        return EditorGUILayout.Vector2Field("", Value, GUILayoutOption);
    }

    public static Vector2Int SetField(Vector2Int Value, params GUILayoutOption[] GUILayoutOption)
    {
        return EditorGUILayout.Vector2IntField("", Value, GUILayoutOption);
    }

    //Vector3

    public static Vector3 SetField(Vector3 Value, params GUILayoutOption[] GUILayoutOption)
    {
        return EditorGUILayout.Vector2Field("", Value, GUILayoutOption);
    }

    public static Vector3Int SetField(Vector3Int Value, params GUILayoutOption[] GUILayoutOption)
    {
        return EditorGUILayout.Vector3IntField("", Value, GUILayoutOption);
    }

    #endregion

    #region Field Other

    public static GameObject SetField(GameObject Value, params GUILayoutOption[] GUILayoutOption)
    {
        return (GameObject)EditorGUILayout.ObjectField("", Value, typeof(GameObject), true, GUILayoutOption);
    }

    public static Sprite SetField(Sprite Value, params GUILayoutOption[] GUILayoutOption)
    {
        return (Sprite)EditorGUILayout.ObjectField("", Value, typeof(Sprite), true, GUILayoutOption);
    }

    public static Color SetField(Color Value, params GUILayoutOption[] GUILayoutOption)
    {
        return EditorGUILayout.ColorField(Value, GUILayoutOption);
    }

    #endregion

    #endregion

    #region ------------------------------------ Popup

    //

    public static int SetPopup(int IndexChoice, string[] ListChoice, params GUILayoutOption[] GUILayoutOption)
    {
        return EditorGUILayout.Popup("", IndexChoice, ListChoice, GUILayoutOption);
    }

    public static int SetPopup(int IndexChoice, List<string> ListChoice, params GUILayoutOption[] GUILayoutOption)
    {
        return EditorGUILayout.Popup("", IndexChoice, ListChoice.ToArray(), GUILayoutOption);
    }

    public static int SetPopup<T>(int IndexChoice, params GUILayoutOption[] GUILayoutOption) where T : Enum
    {
        return EditorGUILayout.Popup("", IndexChoice, QEnum.GetListName<T>().ToArray(), GUILayoutOption);
    }

    //

    public static string SetPopup(string DataChoice, string[] ListChoice, params GUILayoutOption[] GUILayoutOption)
    {
        if (!ListChoice.ToList().Exists(t => t == DataChoice))
            return ListChoice[SetPopup(0, ListChoice, GUILayoutOption)];
        else
            return ListChoice[SetPopup(ListChoice.ToList().FindIndex(t => t == DataChoice), ListChoice, GUILayoutOption)];
    }

    public static string SetPopup(string DataChoice, List<string> ListChoice, params GUILayoutOption[] GUILayoutOption)
    {
        if (!ListChoice.Exists(t => t == DataChoice))
            return ListChoice[SetPopup(0, ListChoice, GUILayoutOption)];
        else
            return ListChoice[SetPopup(ListChoice.FindIndex(t => t == DataChoice), ListChoice, GUILayoutOption)];
    }

    public static string SetPopup<T>(string DataChoice, params GUILayoutOption[] GUILayoutOption) where T : Enum
    {
        List<string> ListChoice = QEnum.GetListName<T>();

        if (!ListChoice.Exists(t => t == DataChoice))
            return ListChoice[SetPopup(0, ListChoice, GUILayoutOption)];
        else
            return ListChoice[SetPopup(ListChoice.FindIndex(t => t == DataChoice), ListChoice, GUILayoutOption)];
    }

    #endregion

    #region ------------------------------------ Else

    public static void SetBackground(Color Color)
    {
        GUI.backgroundColor = Color;
    }

    public static void SetSpace(float Space = 10)
    {
        GUILayout.Space(Space);
    }

    #endregion

    #endregion

    #region ==================================== GUI Varible

    #region ------------------------------------ GUI Layout Option

    public static GUILayoutOption GetGUILayoutWidth(float Width = 20f)
    {
        return GUILayout.Width(Width);
    }

    public static GUILayoutOption GetGUILayoutHeight(float Height = 20)
    {
        return GUILayout.Height(Height);
    }

    public static GUILayoutOption[] GetGUILayoutSize(float Width = 20f, float Height = 20)
    {
        return new GUILayoutOption[2] { GUILayout.Width(Width), GUILayout.Height(Height) };
    }

    //

    public static GUILayoutOption[] GetGUILayoutSizeSprite(float Width = 125f, float Height = 18f)
    {
        return new GUILayoutOption[2] { GUILayout.Width(Width), GUILayout.Height(Height) };
    }

    public static GUILayoutOption GetGUILayoutSizeToggle(float Width = 25f)
    {
        return GUILayout.Width(Width);
    }

    #endregion

    #region ------------------------------------ GUI Style

    public static GUIStyle GetGUIStyleLabel(FontStyle FontStyle = FontStyle.Normal, TextAnchor Alignment = TextAnchor.MiddleCenter, int? FontSize = null)
    {
        GUIStyle GUIStyle;

        if (FontSize.HasValue)
        {
            GUIStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle,
                alignment = Alignment,
                fontSize = FontSize.Value,
            };
        }
        else
        {
            GUIStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle,
                alignment = Alignment,
            };
        }

        return GUIStyle;
    }

    public static GUIStyle GetGUIStyleTextField(FontStyle FontStyle = FontStyle.Normal, TextAnchor Alignment = TextAnchor.MiddleCenter, int? FontSize = null)
    {
        GUIStyle GUIStyle;

        if (FontSize.HasValue)
        {
            GUIStyle = new GUIStyle(GUI.skin.textField)
            {
                fontStyle = FontStyle,
                alignment = Alignment,
                fontSize = FontSize.Value,
            };
        }
        else
        {
            GUIStyle = new GUIStyle(GUI.skin.textField)
            {
                fontStyle = FontStyle,
                alignment = Alignment,
            };
        }

        return GUIStyle;
    }

    public static GUIStyle GetGUIStyleTextArea(FontStyle FontStyle = FontStyle.Normal, TextAnchor Alignment = TextAnchor.MiddleCenter, int? FontSize = null)
    {
        GUIStyle GUIStyle;

        if (FontSize.HasValue)
        {
            GUIStyle = new GUIStyle(GUI.skin.textArea)
            {
                fontStyle = FontStyle,
                alignment = Alignment,
                fontSize = FontSize.Value,
            };
        }
        else
        {
            GUIStyle = new GUIStyle(GUI.skin.textArea)
            {
                fontStyle = FontStyle,
                alignment = Alignment,
            };
        }

        return GUIStyle;
    }

    public static GUIStyle GetGUIStyleButton(FontStyle FontStyle = FontStyle.Normal, TextAnchor Alignment = TextAnchor.MiddleCenter, int? FontSize = null)
    {
        GUIStyle GUIStyle;

        if (FontSize.HasValue)
        {
            GUIStyle = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle,
                alignment = Alignment,
                fontSize = FontSize.Value,
            };
        }
        else
        {
            GUIStyle = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle,
                alignment = Alignment,
            };
        }

        return GUIStyle;
    }

    #endregion

    #region ------------------------------------ GUI Content

    public static GUIContent GetGUIContentImage(Sprite Sprite)
    {
        Texture2D Texture = QSprite.GetTextureConvert(Sprite);

        if (Texture != null)
            return new GUIContent("", Texture);
        else
            return new GUIContent("");
    }

    #endregion

    #endregion

    #region ==================================== GUI Control

    public static void SetUnFocus()
    {
        //NOTE: Call will Lost Focus when Editor Focus on Typing or etc!!
        GUIUtility.keyboardControl = 0;
    }

    #endregion

    #region ==================================== GUI Quick

    #region ------------------------------------ GUI Group Number

    //Group Number - Primary

    public static int SetGroupNumberChange(string Label, int Value)
    {
        SetHorizontalBegin();
        SetLabel(Label);
        Value = SetField(Value, null, GetGUILayoutWidth(50));
        if (SetButton("+", null, GetGUILayoutSize()))
            Value++;
        if (SetButton("-", null, GetGUILayoutSize()))
            Value--;
        SetHorizontalEnd();

        return Value;
    }

    public static int SetGroupNumberChangeLimit(string Label, int Value, int Max, int Min)
    {
        SetHorizontalBegin();
        SetLabel(Label);
        Value = SetField(Value, null, GetGUILayoutWidth(50));
        if (SetButton("+", null, GetGUILayoutSize()))
            if (Value < Max)
                Value++;
        if (SetButton("-", null, GetGUILayoutSize()))
            if (Value > Min)
                Value--;
        SetHorizontalEnd();

        return Value;
    }

    public static int SetGroupNumberChangeLimitMin(string Label, int Value, int Limit)
    {
        SetHorizontalBegin();
        SetLabel(Label);
        Value = SetField(Value, null, GetGUILayoutWidth(50));
        if (SetButton("+", null, GetGUILayoutSize()))
            Value++;
        if (SetButton("-", null, GetGUILayoutSize()))
            if (Value > Limit)
                Value--;
        SetHorizontalEnd();

        return Value;
    }

    public static int SetGroupNumberChangeLimitMax(string Label, int Value, int Limit)
    {
        SetHorizontalBegin();
        SetLabel(Label);
        Value = SetField(Value, null, GetGUILayoutWidth(50));
        if (SetButton("+", null, GetGUILayoutSize()))
            if (Value < Limit)
                Value++;
        if (SetButton("-", null, GetGUILayoutSize()))
            Value--;
        SetHorizontalEnd();

        return Value;
    }

    //Group Number - Switch

    public static (int Value, bool Switch) SetGroupNumberChange(string Label, bool Switch, int Value)
    {
        SetHorizontalBegin();
        if (SetButton(Label, GetGUIStyleLabel(Switch ? FontStyle.Bold : FontStyle.Normal, TextAnchor.MiddleLeft)))
            Switch = !Switch;
        Value = SetField(Value, null, GetGUILayoutWidth(50));
        if (SetButton("+", null, GetGUILayoutSize()))
            Value++;
        if (SetButton("-", null, GetGUILayoutSize()))
            Value--;
        SetHorizontalEnd();

        return (Value, Switch);
    }

    public static (int Value, bool Switch) SetGroupNumberChangeLimit(string Label, bool Switch, int Value, int Max, int Min)
    {
        SetHorizontalBegin();
        if (SetButton(Label, GetGUIStyleLabel(Switch ? FontStyle.Bold : FontStyle.Normal, TextAnchor.MiddleLeft)))
            Switch = !Switch;
        Value = SetField(Value, null, GetGUILayoutWidth(50));
        if (SetButton("+", null, GetGUILayoutSize()))
            if (Value < Max)
                Value++;
        if (SetButton("-", null, GetGUILayoutSize()))
            if (Value > Min)
                Value--;
        SetHorizontalEnd();

        return (Value, Switch);
    }

    public static (int Value, bool Switch) SetGroupNumberChangeLimitMin(string Label, bool Switch, int Value, int Limit)
    {
        SetHorizontalBegin();
        if (SetButton(Label, GetGUIStyleLabel(Switch ? FontStyle.Bold : FontStyle.Normal, TextAnchor.MiddleLeft)))
            Switch = !Switch;
        Value = SetField(Value, null, GetGUILayoutWidth(50));
        if (SetButton("+", null, GetGUILayoutSize()))
            Value++;
        if (SetButton("-", null, GetGUILayoutSize()))
            if (Value > Limit)
                Value--;
        SetHorizontalEnd();

        return (Value, Switch);
    }

    public static (int Value, bool Switch) SetGroupNumberChangeLimitMax(string Label, bool Switch, int Value, int Limit)
    {
        SetHorizontalBegin();
        if (SetButton(Label, GetGUIStyleLabel(Switch ? FontStyle.Bold : FontStyle.Normal, TextAnchor.MiddleLeft)))
            Switch = !Switch;
        Value = SetField(Value, null, GetGUILayoutWidth(50));
        if (SetButton("+", null, GetGUILayoutSize()))
            if (Value < Limit)
                Value++;
        if (SetButton("-", null, GetGUILayoutSize()))
            Value--;
        SetHorizontalEnd();

        return (Value, Switch);
    }

    #endregion

    #region ------------------------------------ ...

    #endregion

    #endregion
}

///<summary>Used for Unity Window Panel (EditorWindow) only</summary>
///<remarks><b>UnityEditorOnly</b></remarks>
public class QUnityEditorWindow
{
    #region ==================================== GUI Primary

    #region ------------------------------------ GUI Layout Option

    private static readonly float WIDTH_OFFSET = 4f;

    public static GUILayoutOption GetGUILayoutWidth(EditorWindow This, float WidthPercent = 1, float WidthOffset = 0)
    {
        return QUnityEditor.GetGUILayoutWidth(GetWindowWidth(This) * WidthPercent - WidthOffset - WIDTH_OFFSET);
    }

    public static GUILayoutOption GetGUILayoutWidthBaseHeight(EditorWindow This, float HeightPercent = 1, float HeightOffset = 0)
    {
        return QUnityEditor.GetGUILayoutWidth(GetWindowHeight(This) * HeightPercent - HeightOffset - WIDTH_OFFSET);
    }

    public static GUILayoutOption GetGUILayoutHeight(EditorWindow This, float HeightPercent = 1, float HeightOffset = 0)
    {
        return QUnityEditor.GetGUILayoutHeight(GetWindowHeight(This) * HeightPercent - HeightOffset);
    }

    public static GUILayoutOption GetGUILayoutHeightBaseWidth(EditorWindow This, float WidthPercent = 1, float WidthOffset = 0)
    {
        return QUnityEditor.GetGUILayoutHeight(GetWindowWidth(This) * WidthPercent - WidthOffset);
    }

    #endregion

    #region ------------------------------------ GUI Panel Size

    public static float GetWindowWidth(EditorWindow This)
    {
        return This.position.width;
    }

    public static float GetWindowHeight(EditorWindow This)
    {
        return This.position.height;
    }

    #endregion

    #endregion
}

///<summary>
///Used for Script (CustomEditor) only<br></br>
///If "override void OnGUI" is not running on editor, go to Project Settings/Editor and turn on "Use IMGUI Default Inspector"!
///</summary>
///<remarks><b>UnityEditorOnly</b></remarks>
public class QUnityEditorCustom
{
    //IMPORTANCE: If "override void OnGUI" is not running on editor, go to Project Settings/Editor and turn on "Use IMGUI Default Inspector"!

    #region ==================================== GUI Primary

    #region ------------------------------------ Get Field

    public static SerializedProperty GetField(Editor This, string FieldName)
    {
        return This.serializedObject.FindProperty(FieldName);
    }

    #endregion

    #region ------------------------------------ Set Field

    public static void SetUpdate(Editor This)
    {
        This.serializedObject.Update();
    }

    public static void SetField(SerializedProperty Field)
    {
        EditorGUILayout.PropertyField(Field);
    }

    public static void SetField(SerializedProperty Field, params GUILayoutOption[] GUILayoutOption)
    {
        EditorGUILayout.PropertyField(Field, GUILayoutOption);
    }

    public static void SetField(SerializedProperty Field, Rect Rect, bool FieldName = false)
    {
        if (FieldName)
            EditorGUI.PropertyField(Rect, Field);
        else
            EditorGUI.PropertyField(Rect, Field, GUIContent.none);
    }

    public static void SetApply(Editor This)
    {
        This.serializedObject.ApplyModifiedProperties();
    }

    #endregion

    #endregion
}

///<summary>Used for Custom Class or Struct (PropertyDrawer) only</summary>
///<remarks><b>UnityEditorOnly</b></remarks>
public class QUnityEditorObject
{
    #region ==================================== GUI Primary

    #region ------------------------------------ Get Field

    public static PropertyField GetField(SerializedProperty Property, string FieldName)
    {
        return new PropertyField(Property.FindPropertyRelative(FieldName));
    } //Use in 'public override VisualElement CreatePropertyGUI(SerializedProperty property)' methode!!

    public static VisualElement GetContainer(SerializedProperty Property, params string[] FieldName)
    {
        VisualElement Container = new VisualElement();

        foreach (string FieldNameChild in FieldName)
        {
            PropertyField Field = GetField(Property, FieldNameChild);
            Container.Add(Field);
        }
        //
        return Container;
    } //Use in 'public override VisualElement CreatePropertyGUI(SerializedProperty property)' methode!!

    #endregion

    #region ------------------------------------ Chance Property

    //NOTE: Should called first to change logic workflow of Unity!!

    public static void SetPropertyBegin(Rect Position, SerializedProperty Property, GUIContent Label)
    {
        EditorGUI.BeginProperty(Position, Label, Property);
    } //Use in 'public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)' methode!!

    public static void SetPropertyEnd()
    {
        EditorGUI.EndProperty();
    } //Use in 'public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)' methode!!

    #endregion

    #region ------------------------------------ Set Field

    public static void SetField(SerializedProperty Property, string NameField, Rect Rect, bool FieldName = false)
    {
        if (FieldName)
            EditorGUI.PropertyField(Rect, Property.FindPropertyRelative(NameField));
        else
            EditorGUI.PropertyField(Rect, Property.FindPropertyRelative(NameField), GUIContent.none);
    }

    #endregion

    #endregion
}

#endif