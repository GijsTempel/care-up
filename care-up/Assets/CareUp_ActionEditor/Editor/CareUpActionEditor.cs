using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace CareUp.ActionEditor
{
    public enum ActionType
    {
        combine,
        use,
        talk,
        useOn,
        examine,
        pickUp,
        sequenceStep,
        drop,
        movement,
        general
    };

    [XmlRoot("ActionList")]
    public class ActionList : List<Action>
    {
    
    }

    [XmlRoot("action")]
    public class Action
    {
        [XmlIgnore]
        public bool selected = false;
        [XmlIgnore]
        public bool unfold = false;

        [XmlAttribute]
        public int index;
        [XmlAttribute]
        public string description = "";
        [XmlAttribute]
        public string posID = null;
        [XmlAttribute]
        public ActionType type;
        [XmlAttribute]
        public string comment = null;
        [XmlAttribute]
        public string commentUA = null;
        [XmlAttribute]
        public string place = null;
        [XmlAttribute]
        public string secondPlace = null;
        [XmlAttribute]
        public string item = null;
        [XmlAttribute]
        public int points = 1;
        [XmlAttribute]
        public string quiz = null;
        [XmlAttribute]
        public string buttonText = null;
        [XmlAttribute]
        public string optional = null;
        [XmlAttribute]
        public string messageTitle = null;
        [XmlAttribute]
        public string messageContent = null;
        [XmlAttribute]
        public string decombineText = null;
        [XmlAttribute]
        public string encounter = null;
        [XmlAttribute]
        public string left = null;
        [XmlAttribute]
        public string right = null;
        [XmlAttribute]
        public string value = null;
        [XmlAttribute]
        public string audioHint = null;
        [XmlAttribute]
        public string action = null;
        [XmlAttribute]
        public string target = null;
        [XmlAttribute]
        public string topic = null;
        [XmlAttribute]
        public string blockUnlock = null;
        [XmlAttribute]
        public string blockRequired = null;
        [XmlAttribute]
        public string blockLock = null;
        [XmlAttribute]
        public string blockTitle = null;
        [XmlAttribute]
        public string blockMessage = null;
        [XmlAttribute]
        public string expected = null;
        [XmlAttribute]
        public string hidden = null;

        public Action Copy()
        {
            Action newCopy = new Action();
            newCopy.selected = this.selected;
            newCopy.unfold = this.unfold;
            newCopy.index = this.index;
            newCopy.description = this.description;
            newCopy.posID = this.posID;
            newCopy.type = this.type;
            newCopy.comment = this.comment;
            newCopy.commentUA = this.commentUA;
            newCopy.place = this.place;
            newCopy.secondPlace = this.secondPlace;
            newCopy.item = this.item;
            newCopy.points = this.points;
            newCopy.quiz = this.quiz;
            newCopy.buttonText = this.buttonText;
            newCopy.optional = this.optional;
            newCopy.messageTitle = this.messageTitle;
            newCopy.messageContent = this.messageContent;
            newCopy.decombineText = this.decombineText;
            newCopy.encounter = this.encounter;
            newCopy.left = this.left;
            newCopy.right = this.right;
            newCopy.value = this.value;
            newCopy.audioHint = this.audioHint;
            newCopy.action = this.action;
            newCopy.target = this.target;
            newCopy.topic = this.topic;
            newCopy.blockUnlock = this.blockUnlock;
            newCopy.blockRequired = this.blockRequired;
            newCopy.blockLock = this.blockLock;
            newCopy.blockTitle = this.blockTitle;
            newCopy.blockMessage = this.blockMessage;
            newCopy.expected = this.expected;
            newCopy.hidden = this.hidden;
            return newCopy;
        }

        public string GetAttributeByName(string _name)
        {
            switch (_name)
            {
                case "comment":
                    return comment;
                case "place":
                    return place;
                case "secondPlace":
                    return secondPlace;
                case "item":
                    return item;
                case "target":
                    return target;
                case "quiz":
                    return quiz;
                case "messageTitle":
                    return messageTitle;
                case "messageContent":
                    return messageContent;
                case "optional":
                    return optional;
                case "buttonText":
                    return buttonText;
                case "encounter":
                    return encounter;
                case "decombineText":
                    return decombineText;
                case "left":
                    return left;
                case "right":
                    return right;
                case "value":
                    return value;
                case "expected":
                    return expected;
                case "blockRequired":
                    return blockRequired;
                case "blockTitle":
                    return blockTitle;
                case "blockMessage":
                    return blockMessage;
                case "blockUnlock":
                    return blockUnlock;
                case "blockLock":
                    return blockLock;
                case "topic":
                    return topic;
                case "action":
                    return action;
                case "posID":
                    return posID;
                case "audioHint":
                    return audioHint;
                case "hidden":
                    return hidden;
                case "description":
                    return description;
            }
            return null;
        }

        public void SetAttributeByName(string _name, string _value)
        {
            switch (_name)
            {
                case "comment":
                    comment = _value;
                    break;
                case "place":
                    place = _value;
                    break;
                case "secondPlace":
                    secondPlace = _value;
                    break;
                case "item":
                    item = _value;
                    break;
                case "target":
                    target = _value;
                    break;
                case "quiz":
                    quiz = _value;
                    break;
                case "messageTitle":
                    messageTitle = _value;
                    break;
                case "messageContent":
                    messageContent = _value;
                    break;
                case "optional":
                    optional = _value;
                    break;
                case "buttonText":
                    buttonText = _value;
                    break;
                case "encounter":
                    encounter = _value;
                    break;
                case "decombineText":
                    decombineText = _value;
                    break;
                case "left":
                    left = _value;
                    break;
                case "right":
                    right = _value;
                    break;
                case "value":
                    value = _value;
                    break;
                case "expected":
                    expected = _value;
                    break;
                case "blockRequired":
                    blockRequired = _value;
                    break;
                case "blockTitle":
                    blockTitle = _value;
                    break;
                case "blockMessage":
                    blockMessage = _value;
                    break;
                case "blockUnlock":
                    blockUnlock = _value;
                    break;
                case "blockLock":
                    blockLock = _value;
                    break;
                case "topic":
                    topic = _value;
                    break;
                case "action":
                    action = _value;
                    break;
                case "posID":
                    posID = _value;
                    break;
                case "audioHint":
                    audioHint = _value;
                    break;
                case "hidden":
                    hidden = _value;
                    break;
                case "description":
                    description = _value;
                    break;
            }
        }
    }

    class ActionEditor : EditorWindow
    {
        enum ActionGrouping
        {
            none,
            toGtoup,
            toUngroup
        };

        bool toInsertAction = false;
        List<string> ActionTypeNames = new List<string>{
            "combine",
            "use",
            "talk",
            "useOn",
            "examine",
            "pickUp",
            "sequenceStep",
            "drop",
            "movement",
            "general"
        };

        List<string> ActionAttributeNames = new List<string>{
            "__Add Attribute__",
            "comment",
            "place",
            "secondPlace",
            "item",
            "target",
            "quiz",
            "messageTitle",
            "messageContent",
            "optional",
            "buttonText",
            "encounter",
            "decombineText",
            "left",
            "right",
            "value",
            "expected",
            "blockRequired",
            "blockTitle",
            "blockMessage",
            "blockUnlock",
            "blockLock",
            "topic",
            "action",
            "posID",
            "audioHint",
            "hidden"
        };

        int ActionsHash = -1;
        public Object actionsFile;
        Vector2 scrollPos;
        string loadedActionFilePath = "";
        ActionList actions = new ActionList();
        ActionList actionsToStay = null;
        ActionList actionsToMove = null;
        bool toMoveSelectedActions = false;
        bool automateIndexes = false;
        int lastSelected = -1;
        Action actionToEditInTE = null;
        string attributeToEditInTE = "";
        string TextEditorValue = "";
        Vector2 TextEditorScroll = new Vector2();

        [MenuItem("Tools/Actions Editor")]
        static void Init()
        {
            EditorWindow.GetWindow<ActionEditor>();
        }

        void ExitTextEdit(bool toSave = false)
        {
            if (toSave)
                actionToEditInTE.SetAttributeByName(attributeToEditInTE, TextEditorValue);
            actionToEditInTE = null;
        }

        void LoadActionsData(string actionFilePath, bool toReload = false)
        {
            actionToEditInTE = null;
            if (actionFilePath == loadedActionFilePath && !toReload)
                return;
            actions = new ActionList();
            string shortPath = "";
            int i = 0;
            foreach (string p in actionFilePath.Split('/'))
            {
                if (i > 1)
                {
                    shortPath += p + '/';
                }
                i++;
            }
            if (shortPath.Length > 0)
            {
                shortPath = shortPath.Substring(0, shortPath.Length - 5);
                TextAsset textAsset = (TextAsset)Resources.Load(shortPath);
                if (textAsset != null)
                {
                    XmlDocument xmlFile = new XmlDocument();
                    xmlFile.LoadXml(textAsset.text);
                    XmlNodeList xmlActions = xmlFile.FirstChild.NextSibling.ChildNodes;
                    foreach (XmlNode xmlAction in xmlActions)
                    {
                        Action action = new Action();

                        int.TryParse(xmlAction.Attributes["index"].Value, out action.index);
                        int.TryParse(xmlAction.Attributes["points"].Value, out action.points);
                        string desc = xmlAction.Attributes["description"].Value;
                        //Debug.Log(index.ToString() + " " + desc);
                        action.description = desc;
                        if (xmlAction.Attributes["posID"] != null)
                            action.posID = xmlAction.Attributes["posID"].Value;
                        action.type = (ActionType)System.Enum.Parse(typeof(ActionType), xmlAction.Attributes["type"].Value);

                        if (xmlAction.Attributes["comment"] != null)
                            action.comment = xmlAction.Attributes["comment"].Value;
                        if (xmlAction.Attributes["commentUA"] != null)
                            action.commentUA = xmlAction.Attributes["commentUA"].Value;
                        if (xmlAction.Attributes["place"] != null)
                            action.place = xmlAction.Attributes["place"].Value;
                        if (xmlAction.Attributes["secondPlace"] != null)
                            action.secondPlace = xmlAction.Attributes["secondPlace"].Value;
                        if (xmlAction.Attributes["item"] != null)
                            action.item = xmlAction.Attributes["item"].Value;
                        if (xmlAction.Attributes["quiz"] != null)
                            action.quiz = xmlAction.Attributes["quiz"].Value;
                        if (xmlAction.Attributes["buttonText"] != null)
                            action.buttonText = xmlAction.Attributes["buttonText"].Value;
                        if (xmlAction.Attributes["optional"] != null)
                            action.optional = xmlAction.Attributes["optional"].Value;
                        if (xmlAction.Attributes["messageTitle"] != null)
                            action.messageTitle = xmlAction.Attributes["messageTitle"].Value;
                        if (xmlAction.Attributes["messageContent"] != null)
                            action.messageContent = xmlAction.Attributes["messageContent"].Value;
                        if (xmlAction.Attributes["decombineText"] != null)
                            action.decombineText = xmlAction.Attributes["decombineText"].Value;
                        if (xmlAction.Attributes["encounter"] != null)
                            action.encounter = xmlAction.Attributes["encounter"].Value;
                        if (xmlAction.Attributes["left"] != null)
                            action.left = xmlAction.Attributes["left"].Value;
                        if (xmlAction.Attributes["right"] != null)
                            action.right = xmlAction.Attributes["right"].Value;
                        if (xmlAction.Attributes["value"] != null)
                            action.value = xmlAction.Attributes["value"].Value;
                        if (xmlAction.Attributes["audioHint"] != null)
                            action.audioHint = xmlAction.Attributes["audioHint"].Value;
                        if (xmlAction.Attributes["action"] != null)
                            action.action = xmlAction.Attributes["action"].Value;
                        if (xmlAction.Attributes["target"] != null)
                            action.target = xmlAction.Attributes["target"].Value;
                        if (xmlAction.Attributes["topic"] != null)
                            action.topic = xmlAction.Attributes["topic"].Value;
                        if (xmlAction.Attributes["blockUnlock"] != null)
                            action.blockUnlock = xmlAction.Attributes["blockUnlock"].Value;
                        if (xmlAction.Attributes["blockRequired"] != null)
                            action.blockRequired = xmlAction.Attributes["blockRequired"].Value;
                        if (xmlAction.Attributes["blockLock"] != null)
                            action.blockLock = xmlAction.Attributes["blockLock"].Value;
                        if (xmlAction.Attributes["blockTitle"] != null)
                            action.blockTitle = xmlAction.Attributes["blockTitle"].Value;
                        if (xmlAction.Attributes["blockMessage"] != null)
                            action.blockMessage = xmlAction.Attributes["blockMessage"].Value;
                        if (xmlAction.Attributes["hidden"] != null)
                            action.hidden = xmlAction.Attributes["hidden"].Value;
                        if (xmlAction.Attributes["expected"] != null)
                            action.expected = xmlAction.Attributes["expected"].Value;

                        actions.Add(action);
                    }
                    loadedActionFilePath = actionFilePath;
                    ActionsHash = actions.GetHashCode();
                }
            }
        }

        void InsertNewActionAt(int _position)
        {
            ActionList newList = new ActionList();
            foreach(Action _act in actions)
            {
                newList.Add(_act);
            }
            Action a = new Action();
            newList.Insert(_position, a);
            toInsertAction = false;
            actions = newList;
            UnfoldAction(actions[_position], true);
        }

        void DeleteActionRange(int startFrom, int endOn)
        {
            ActionList newList = new ActionList();
            foreach(Action _act in actions)
            {
                int currentIndex = actions.IndexOf(_act);
                if (currentIndex >= startFrom && currentIndex <= endOn)
                    newList.Add(_act);

                actions = newList;
            }
        }

        void DeleteAction(int _index)
        {
            DeleteActionRange(_index, _index);
        }

        void DeleteSelectedActions()
        {
            ActionList newList = new ActionList();
            foreach(Action _act in actions)
            {
                if (!_act.selected)
                    newList.Add(_act);
            }
            actions = newList;
        }

        void MoveSingleAction(Action _action, int _position)
        {
            ActionList newList = new ActionList();
            int j = 0;
            for(int i = 0; i < actions.Count + 1; i++)
            {
                if (i == _position)
                {
                    newList.Add(_action);
                }
                else
                {
                    if (j < actions.Count)
                    {
                        if(actions[j] != _action)
                            newList.Add(actions[j]);
                    }
                    j++;
                }
            }
            actions = newList;
            if (automateIndexes)
                RecalculateIndexes();
        }

        int InitMoveSelectedActions(ActionGrouping toGroup = ActionGrouping.none)
        {
            int baseIndex = -1;
            int groupPosition = -1;
            if (toMoveSelectedActions)
            {
                toMoveSelectedActions = false;
                return groupPosition;
            }

            actionsToMove = new ActionList();
            actionsToStay = new ActionList();
            foreach(Action act in actions)
            {
                if (act.selected)
                {
                    if (baseIndex == -1)
                    {
                        groupPosition = actions.IndexOf(act);
                        baseIndex = act.index;
                    }
                    else if (toGroup == ActionGrouping.toGtoup)
                        act.index = baseIndex;
                    if (toGroup == ActionGrouping.toUngroup)
                        act.index = actions.Count + actions.IndexOf(act);
                    actionsToMove.Add(act);
                }
                else
                    actionsToStay.Add(act);
            }
            if (actionsToMove.Count == 0 || actionsToStay.Count == 0)
                return groupPosition;
            toMoveSelectedActions = true;
            return groupPosition;
        }

        void MoveSelectedActionsTo(int _position)
        {
            ActionList newList = new ActionList();
            for(int i = 0; i <= actionsToStay.Count; i++)
            {
                if (i == _position)
                {
                    foreach(Action actToMove in actionsToMove)
                    {
                        newList.Add(actToMove);
                    }
                }
                if (i < actionsToStay.Count)
                {
                    newList.Add(actionsToStay[i]);
                }
            }
            actions = newList;
            toMoveSelectedActions = false;
            if (automateIndexes)
                RecalculateIndexes();
        }

        void AddNewAction()
        {
            Action a = new Action();
            actions.Add(a);
            toInsertAction = false;
            UnfoldAction(actions[actions.Count - 1], true);
            if (automateIndexes)
                RecalculateIndexes();
        }

        void SaveData()
        {
            if (actionsFile != null)
            {
                string actionFilePath = AssetDatabase.GetAssetPath(actionsFile);
                XmlSerializer xmls = new XmlSerializer(typeof(ActionList));
                StringWriter sw = new StringWriter();
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineOnAttributes = true;

                var stringBuilder = new StringBuilder();
                using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
                    xmls.Serialize(xmlWriter, actions, ns);
                stringBuilder.Replace("&#xA;", "\n");
                // stringBuilder.Replace("&#xA;", "    ");
                using (StreamWriter swriter = new StreamWriter(actionFilePath))
                    swriter.Write(stringBuilder.ToString());
            }
        }

        public void UnfoldAction(Action actionToUnfold, bool force = false)
        {
            if (force)
                actionToUnfold.unfold = true;
            foreach (Action a in actions)
            {
                if (a != actionToUnfold)
                    a.unfold = false;
            }
        }

        int GetActionGroupID(Action act)
        {
            int currentActionPos = actions.IndexOf(act);
            if (currentActionPos > 0)
            {
                if (act.index == actions[actions.IndexOf(act) - 1].index)
                    return act.index;
            }
            if (currentActionPos < actions.Count - 1)
            {
                if (act.index == actions[actions.IndexOf(act) + 1].index)
                    return act.index;
            }
            return -1;
        }

        void RecalculateIndexes()
        {
            ActionList newList = new ActionList();
            int ind = 0;
            foreach(Action act in actions)
            {
                Action newAction = act.Copy();
                if (act.hidden != null)
                {
                    newList.Add(newAction);
                    continue;
                }
                newAction.index = ind;

                newList.Add(newAction);
                bool toIncIndex = true;
                if (actions.IndexOf(act) < actions.Count - 1)
                {
                    if (act.index == actions[actions.IndexOf(act) + 1].index)
                        toIncIndex = false;
                }
                if(toIncIndex)
                    ind++;
            }
            actions = newList;
        }

        void HideSelected(bool toHide = true)
        {
            foreach(Action act in actions)
            {
                if (act.selected)
                {
                    if (toHide)
                        act.hidden = toHide.ToString();
                    else
                        act.hidden = null;
                }
            }
        }

        void SelectRange(int _from, int _to, bool toSelect)
        {
            for(int i = _from; i <= _to; i++)
            {
                actions[i].selected = toSelect;
            }
        }

        string testText = "";
        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            actionsFile = EditorGUILayout.ObjectField(actionsFile, typeof(TextAsset), true);
            if (EditorGUI.EndChangeCheck())
                LoadActionsData(AssetDatabase.GetAssetPath(actionsFile));
            if (GUILayout.Button("Reload", GUILayout.Width(100)))
                LoadActionsData(AssetDatabase.GetAssetPath(actionsFile), true);

            EditorGUILayout.EndHorizontal();
            if (actions.Count > 0)
            {
                if (actionToEditInTE != null && attributeToEditInTE != "")
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Cancel Edit"))
                        ExitTextEdit();
                    if (GUILayout.Button("Save Edit"))
                        ExitTextEdit(true);
                    EditorGUILayout.EndHorizontal();

                    TextEditorScroll = EditorGUILayout.BeginScrollView(TextEditorScroll);
                    TextEditorValue = EditorGUILayout.TextArea(TextEditorValue, GUILayout.Height(position.height - 30));
                    EditorGUILayout.EndScrollView();
                    // TextEditorValue = EditorGUI.TextArea(descRect, TextEditorValue);
                }
                else
                {
                    int actionsSelected = 0;
                    automateIndexes = EditorGUILayout.Toggle("Automate indexes:", automateIndexes);
                    EditorGUILayout.BeginHorizontal();
                    if (automateIndexes)
                    {
                        if (GUILayout.Button("Recalculate Indexes", GUILayout.Width(200)));
                        {
                            RecalculateIndexes();
                        }
                        if (GUILayout.Button("--Ungroup selected actions--"))
                        {
                            int groupPisition = InitMoveSelectedActions(ActionGrouping.toUngroup);
                            if (groupPisition != -1)
                                RecalculateIndexes();
                            toMoveSelectedActions = false;
                        }
                        if (GUILayout.Button("++Group selected actions++", GUILayout.Width(200)))
                        {
                            int groupPisition = InitMoveSelectedActions(ActionGrouping.toGtoup);
                            if (groupPisition != -1)
                                MoveSelectedActionsTo(groupPisition);
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Deselect All", GUILayout.Width(200)))
                    {
                        foreach (Action a in actions)
                            a.selected = false;
                    }
                    if (GUILayout.Button("Insert New Action"))
                    {
                        toInsertAction = !toInsertAction;
                    }
                    if (GUILayout.Button("!!Delete Selected Actions!!", GUILayout.Width(200)))
                    {
                        int selectedNum = 0;
                        foreach(Action _act in actions)
                        {
                            if (_act.selected)
                                selectedNum++;
                        }
                        if (selectedNum > 0 && EditorUtility.DisplayDialog("Deleting Selected Actions", 
                            "Are you sure you want to delete\n[ " + 
                            selectedNum.ToString() + " ] selected actions?","Yes", "No" ))
                        {
                            DeleteSelectedActions();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Hide selected actions", GUILayout.Width(200)))
                    {
                        HideSelected();
                        // InitMoveSelectedActions();
                    }
                    if (GUILayout.Button("Unhide selected actions"))
                    {
                        HideSelected(false);
                        // InitMoveSelectedActions();
                    }
                    string moveSelectedButtonText = "Move selected to..";
                    if (toMoveSelectedActions)
                        moveSelectedButtonText = "Cancel move selected to..";
                    if (GUILayout.Button(moveSelectedButtonText, GUILayout.Width(200)))
                    {
                        InitMoveSelectedActions();
                    }
                    EditorGUILayout.EndHorizontal();

                    string saveButtonText = "Save";
                    if (GUILayout.Button(saveButtonText))
                        SaveData();

                    GUIStyle horizontalLine;
                    horizontalLine = new GUIStyle();
                    horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
                    horizontalLine.margin = new RectOffset(0, 0, 4, 4);
                    horizontalLine.fixedHeight = 1;
                    EditorStyles.textField.wordWrap = true;
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                    ActionList currentActions = actions;
                    if (toMoveSelectedActions && actionsToStay != null)
                        currentActions = actionsToStay;

                    Texture emptyTexture = Resources.Load("CareUp_ActionEditor_Icons/empty") as Texture;
                    GUIContent emptyIco = new GUIContent(emptyTexture);
                    Texture groupTexture = Resources.Load("CareUp_ActionEditor_Icons/group") as Texture;
                    GUIContent groupIco = new GUIContent(groupTexture);
                    Texture hiddenTexture = Resources.Load("CareUp_ActionEditor_Icons/hidden") as Texture;
                    foreach (Action a in currentActions)
                    {
                        if (a.selected)
                            actionsSelected++;
                        int groupID = GetActionGroupID(a);

                        if (toInsertAction)
                        {
                            if (GUILayout.Button("Insert Action Here"))
                                InsertNewActionAt(currentActions.IndexOf(a));
                        }
                        if (toMoveSelectedActions && actionsToStay != null)
                        {
                            if (GUILayout.Button("Move Actions Here"))
                                MoveSelectedActionsTo(currentActions.IndexOf(a));
                        }
                        GUILayout.Box(GUIContent.none, horizontalLine);
                        EditorGUILayout.BeginHorizontal();

                        Texture texture = Resources.Load("CareUp_ActionEditor_Icons/" + a.type.ToString()) as Texture;
                        GUIContent iconGUIContent = new GUIContent(texture);

                        GUIStyle labelStyle = new GUIStyle();
                        if (a.hidden != null)
                        {
                            labelStyle.normal.textColor = Color.gray;
                            iconGUIContent = new GUIContent(hiddenTexture);
                        }
                        EditorGUILayout.LabelField(iconGUIContent, GUILayout.Width(25));

                        EditorGUI.BeginChangeCheck();

                        a.unfold = EditorGUILayout.Foldout(a.unfold, a.type.ToString());

                        if (EditorGUI.EndChangeCheck())
                        {
                            UnfoldAction(a);
                        }
                        foreach(string attr in ActionAttributeNames)
                        {
                            if (a.GetAttributeByName(attr) != null)
                            {
                                Texture t = Resources.Load("CareUp_ActionEditor_Icons/" + attr) as Texture;
                                GUIContent ico = new GUIContent(t, attr + " = " + a.GetAttributeByName(attr));
                                EditorGUILayout.LabelField(ico, GUILayout.Width(15));
                            }
                        }
                        EditorGUILayout.LabelField("  ", GUILayout.Width(10));
                        if (groupID == -1)
                        {
                            EditorGUILayout.LabelField(emptyIco, GUILayout.Width(15));
                        }
                        else
                        {
                            EditorGUILayout.LabelField(groupIco, GUILayout.Width(15));
                        }

                        if (!automateIndexes)
                            a.index = EditorGUILayout.IntField(a.index, GUILayout.Width(25));
                        else
                        {
                            string indexStr = a.index.ToString();
                            if (a.hidden != null)
                                indexStr = "_" + indexStr + "_";
                            EditorGUILayout.LabelField(indexStr, GUILayout.Width(25));
                        }


                        if (GUILayout.Button("▲", GUILayout.Width(22)) && currentActions.IndexOf(a) != 0)
                        {
                            MoveSingleAction(a, currentActions.IndexOf(a) - 1);
                        }
                        if (GUILayout.Button("▼", GUILayout.Width(22)) && currentActions.IndexOf(a) < (currentActions.Count - 1))
                        {
                            MoveSingleAction(a, currentActions.IndexOf(a) + 2);
                        } 

                        EditorGUI.BeginChangeCheck();
                        a.selected = EditorGUILayout.Toggle(a.selected, GUILayout.Width(30));
                        if (EditorGUI.EndChangeCheck())
                        {
                            Event e = Event.current;
                            if (e.shift)
                            {
                                if (lastSelected >= 0 && lastSelected < actions.Count)
                                {
                                    int _from = lastSelected;
                                    int _to = actions.IndexOf(a);
                                    if (_from > _to)
                                    {
                                        int __from = _from;
                                        _from = _to;
                                        _to = __from;
                                    }
                                    SelectRange(_from, _to, a.selected);
                                }
                            }
                            lastSelected = actions.IndexOf(a);
                        }
                        EditorGUILayout.EndHorizontal();
                        
                        if (!a.unfold)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("", GUILayout.Width(30));
                            EditorGUILayout.LabelField(a.description.Split('\n')[0], labelStyle);
                            EditorGUILayout.EndHorizontal();
                        }
                        else
                        {
                            Rect descRect = GUILayoutUtility.GetLastRect();
                            descRect.x = 150;
                            descRect.width = position.width - 192;
                            descRect.y += 24;
                            descRect.height = 50;
                            a.description = EditorGUI.TextArea(descRect, a.description);
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Description :", GUILayout.Height(55));
                            if (GUILayout.Button("E", GUILayout.Width(22)))
                            {
                                TextEditorScroll = new Vector2();
                                TextEditorValue = a.description;
                                actionToEditInTE = a;
                                attributeToEditInTE = "description";
                            }
                            EditorGUILayout.EndHorizontal();

                            // a.description = EditorGUILayout.TextField("Description: ", a.description, GUILayout.Height(50));

                            foreach (string attr in ActionAttributeNames)
                            {
                                if (a.GetAttributeByName(attr) != null)
                                {
                                    EditorGUILayout.BeginHorizontal();

                                    Texture t = Resources.Load("CareUp_ActionEditor_Icons/" + attr) as Texture;
                                    GUIContent ico = new GUIContent(t, attr + " = " + a.GetAttributeByName(attr));
                                    EditorGUILayout.LabelField(ico, GUILayout.Width(25));
                                    a.SetAttributeByName(attr, EditorGUILayout.TextField(attr + ": ", a.GetAttributeByName(attr)));
                                    if (GUILayout.Button("E", GUILayout.Width(22)))
                                    {
                                        TextEditorScroll = new Vector2();
                                        TextEditorValue = a.GetAttributeByName(attr);
                                        actionToEditInTE = a;
                                        attributeToEditInTE = attr;
                                    }
                                    if (GUILayout.Button("-", GUILayout.Width(22)))
                                    {
                                        a.SetAttributeByName(attr, null);
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }
                            }
                            EditorGUILayout.BeginHorizontal();
                            a.type = (ActionType)EditorGUILayout.Popup((int)a.type, ActionTypeNames.ToArray());

                            int v = 0;
                            List<string> notListedAttributs = new List<string>();
                            foreach (string attr in ActionAttributeNames)
                            {
                                if (a.GetAttributeByName(attr) == null)
                                    notListedAttributs.Add(attr);
                            }
                            EditorGUILayout.LabelField("Points: ", GUILayout.Width(50));
                            a.points = EditorGUILayout.IntField(a.points, GUILayout.Width(50));

                            EditorGUI.BeginChangeCheck();
                            v = EditorGUILayout.Popup(v, notListedAttributs.ToArray(), GUILayout.Width(150));
                            if (EditorGUI.EndChangeCheck())
                            {
                                a.SetAttributeByName(notListedAttributs[v], "");
                            }

                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.Space();
                    }
                    if (toInsertAction)
                    {
                        if (GUILayout.Button("Insert Action Here"))
                                AddNewAction();
                    }
                    if (toMoveSelectedActions && actionsToStay != null)
                    {
                        if (GUILayout.Button("Move Actions Here"))
                            MoveSelectedActionsTo(currentActions.Count);
                    }
                    EditorGUILayout.EndScrollView();
                    EditorGUILayout.LabelField("Selected [ " + actionsSelected.ToString() + " ] Actions");
                }
            }
        }
        void OnInspectorUpdate()
        {
            this.Repaint();
        }
    }

}
