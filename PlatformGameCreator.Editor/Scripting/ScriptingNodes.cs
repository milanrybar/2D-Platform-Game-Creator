/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Class for working with the scripting data that we get during runtime.
    /// </summary>
    class ScriptingNodes
    {
        /// <summary>
        /// Gets the root category of the scripting data.
        /// </summary>
        public static CategoryData Root
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new ScriptingNodes();
                }

                return singleton.root;
            }
        }

        private static ScriptingNodes singleton;
        private CategoryData root;

        private ScriptingNodes()
        {
            CreateRoot();
        }

        /// <summary>
        /// Loads all scripting data and stores them sorted at the tree structure.
        /// </summary>
        private void CreateRoot()
        {
            root = new CategoryData();

            bool isAction, isEvent;

            foreach (Type type in typeof(GameEngine.Scripting.ScriptNode).Assembly.GetTypes())
            {
                isAction = isEvent = false;

                if (type.IsSubclassOf(typeof(GameEngine.Scripting.Actions.ActionNode))) isAction = true;
                else if (type.IsSubclassOf(typeof(GameEngine.Scripting.Events.EventNode))) isEvent = true;

                // get all subclasses from ActionNode or EventNode
                if ((isAction || isEvent) && !type.IsAbstract && type.IsPublic)
                {
                    // script node (event or action)
                    NodeData scriptNode = new NodeData();

                    scriptNode.Name = GetFriendlyName(type, type.Name);
                    scriptNode.RealName = type.FullName;
                    scriptNode.Description = GetDescription(type);
                    scriptNode.Type = isAction ? NodeType.Action : NodeType.Event;

                    // add script node to the correct category
                    CreateCategory(root, GetCategory(type)).Items.Add(scriptNode);

                    // get all public non-static fields from class
                    foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                    {
                        // variable socket is an array of generic class GameEngine.Variable<T>
                        // or generic variable GameEngine.Variable<T>
                        if ((fieldInfo.FieldType.IsArray && fieldInfo.FieldType.HasElementType && fieldInfo.FieldType.GetElementType().IsGenericType &&
                            fieldInfo.FieldType.GetElementType().GetGenericTypeDefinition() == typeof(GameEngine.Scripting.Variable<>)) ||
                            (fieldInfo.FieldType.IsGenericType && fieldInfo.FieldType.GetGenericTypeDefinition() == typeof(GameEngine.Scripting.Variable<>)))
                        {
                            // get argument for GameEngine.Variable<T>
                            Type[] arguments = fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType().GetGenericArguments() : fieldInfo.FieldType.GetGenericArguments();

                            if (arguments != null && arguments.Length == 1)
                            {
                                // try to get VariableSocketTypeAttribute
                                GameEngine.Scripting.VariableSocketAttribute variableSocketAttribute = (GameEngine.Scripting.VariableSocketAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(GameEngine.Scripting.VariableSocketAttribute));

                                if (variableSocketAttribute != null)
                                {
                                    // variable socket
                                    NodeSocketData variableSocket = new NodeSocketData();

                                    variableSocket.Name = GetFriendlyName(fieldInfo, fieldInfo.Name);
                                    variableSocket.RealName = fieldInfo.Name;
                                    variableSocket.Description = GetDescription(fieldInfo);
                                    variableSocket.Type = variableSocketAttribute.Type == GameEngine.Scripting.VariableSocketType.In ? NodeSocketType.VariableIn : NodeSocketType.VariableOut;
                                    variableSocket.VariableType = VariableTypeHelper.FromType(arguments[0]);
                                    variableSocket.IsArray = fieldInfo.FieldType.IsArray;
                                    variableSocket.DefaultValue = GetDefaultValue(fieldInfo);
                                    variableSocket.Visible = variableSocketAttribute.Visible;
                                    variableSocket.CanBeEmpty = variableSocketAttribute.CanBeEmpty;

                                    // variable out socket must be an array
                                    if (variableSocket.Type == NodeSocketType.VariableOut && !variableSocket.IsArray) continue;

                                    scriptNode.Sockets.Add(variableSocket);
                                }
                            }
                        }
                        // signal out socket is ScriptSocketHandler delegate
                        else if (fieldInfo.FieldType.IsSubclassOf(typeof(Delegate)) && fieldInfo.FieldType == typeof(GameEngine.Scripting.ScriptSocketHandler))
                        {
                            // signal out socket
                            NodeSocketData signalOutSocket = new NodeSocketData();

                            signalOutSocket.Name = GetFriendlyName(fieldInfo, fieldInfo.Name);
                            signalOutSocket.RealName = fieldInfo.Name;
                            signalOutSocket.Description = GetDescription(fieldInfo);
                            signalOutSocket.Type = NodeSocketType.SignalOut;

                            scriptNode.Sockets.Add(signalOutSocket);
                        }
                    }

                    // get all public non-static method from class
                    foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                    {
                        // signal in socket is method with none parameter, returns void, 
                        // is defined in script node (event or action, not base classes) and it is not Update and Connect method (from event)
                        if (methodInfo.GetParameters().Length == 0 && methodInfo.ReturnType == typeof(void) && methodInfo.Name != "Update" && methodInfo.Name != "Connect")
                        {
                            // signal in socket
                            NodeSocketData signalInSocket = new NodeSocketData();

                            signalInSocket.Name = GetFriendlyName(methodInfo, methodInfo.Name);
                            signalInSocket.RealName = methodInfo.Name;
                            signalInSocket.Description = GetDescription(methodInfo);
                            signalInSocket.Type = NodeSocketType.SignalIn;

                            scriptNode.Sockets.Add(signalInSocket);
                        }
                    }
                }
            }

            SortCategory(root);
        }

        /// <summary>
        /// Creates the category on the specified path from the specified category.
        /// </summary>
        private CategoryData CreateCategory(CategoryData baseCategory, string categoryPath)
        {
            if (categoryPath == null || categoryPath == "") return baseCategory;

            string[] categories = categoryPath.Split('/');

            return CreateCategory(baseCategory, categories, 0);
        }

        /// <summary>
        /// Creates the category on the specified path from the specified category.
        /// </summary>
        private CategoryData CreateCategory(CategoryData baseCategory, string[] path, int level)
        {
            // end of path
            if (level >= path.Length) return baseCategory;

            // try to find existing category
            foreach (ScriptData baseNode in baseCategory.Items)
            {
                CategoryData category = baseNode as CategoryData;
                // category found
                if (category != null && category.Name == path[level])
                {
                    // move to the next level
                    return CreateCategory(category, path, level + 1);
                }
            }

            // create remaining category path
            CategoryData lastCategory = baseCategory;
            for (int i = level; i < path.Length; ++i)
            {
                // new category
                CategoryData newCategory = new CategoryData();
                newCategory.Name = path[i];

                // add new category to last category
                lastCategory.Items.Add(newCategory);

                // set new as last category
                lastCategory = newCategory;
            }

            return lastCategory;
        }

        /// <summary>
        /// Gets the value from <see cref="GameEngine.Scripting.FriendlyNameAttribute"/> of the specified type.
        /// If not exists returns the specified alternative friendly name.
        /// </summary>
        private string GetFriendlyName(Type type, string alternativeName)
        {
            // try to get FriendlyNameAttribute
            Attribute friendlyName = Attribute.GetCustomAttribute(type, typeof(GameEngine.Scripting.FriendlyNameAttribute));

            if (friendlyName != null) return ((GameEngine.Scripting.FriendlyNameAttribute)friendlyName).Name;
            else return alternativeName;
        }

        /// <summary>
        /// Gets the value from <see cref="GameEngine.Scripting.FriendlyNameAttribute"/> of the specified field.
        /// If not exists returns the specified alternative friendly name.
        /// </summary>
        private string GetFriendlyName(FieldInfo fieldInfo, string alternativeName)
        {
            // try to get FriendlyNameAttribute
            Attribute friendlyName = Attribute.GetCustomAttribute(fieldInfo, typeof(GameEngine.Scripting.FriendlyNameAttribute));

            if (friendlyName != null) return ((GameEngine.Scripting.FriendlyNameAttribute)friendlyName).Name;
            else return alternativeName;
        }

        /// <summary>
        /// Gets the value from <see cref="GameEngine.Scripting.FriendlyNameAttribute"/> of the specified method.
        /// If not exists returns the specified alternative friendly name.
        /// </summary>
        private string GetFriendlyName(MethodInfo methodInfo, string alternativeName)
        {
            // try to get FriendlyNameAttribute
            Attribute friendlyName = Attribute.GetCustomAttribute(methodInfo, typeof(GameEngine.Scripting.FriendlyNameAttribute));

            if (friendlyName != null) return ((GameEngine.Scripting.FriendlyNameAttribute)friendlyName).Name;
            else return alternativeName;
        }

        /// <summary>
        /// Gets the value from <see cref="GameEngine.Scripting.DescriptionAttribute"/> of the specified type.
        /// If not exists returns <c>null</c>.
        /// </summary>
        private string GetDescription(Type type)
        {
            // try to get DescriptionAttribute
            Attribute description = Attribute.GetCustomAttribute(type, typeof(GameEngine.Scripting.DescriptionAttribute));

            if (description != null) return ((GameEngine.Scripting.DescriptionAttribute)description).Description;
            else return null;
        }

        /// <summary>
        /// Gets the value from <see cref="GameEngine.Scripting.DescriptionAttribute"/> of the specified field.
        /// If not exists returns <c>null</c>.
        /// </summary>
        private string GetDescription(FieldInfo fieldInfo)
        {
            // try to get DescriptionAttribute
            Attribute description = Attribute.GetCustomAttribute(fieldInfo, typeof(GameEngine.Scripting.DescriptionAttribute));

            if (description != null) return ((GameEngine.Scripting.DescriptionAttribute)description).Description;
            else return null;
        }

        /// <summary>
        /// Gets the value from <see cref="GameEngine.Scripting.DescriptionAttribute"/> of the specified method.
        /// If not exists returns <c>null</c>.
        /// </summary>
        private string GetDescription(MethodInfo methodInfo)
        {
            // try to get DescriptionAttribute
            Attribute description = Attribute.GetCustomAttribute(methodInfo, typeof(GameEngine.Scripting.DescriptionAttribute));

            if (description != null) return ((GameEngine.Scripting.DescriptionAttribute)description).Description;
            else return null;
        }

        /// <summary>
        /// Gets the value from <see cref="GameEngine.Scripting.DefaultValueAttribute"/> of the specified field.
        /// If not exists returns <c>null</c>.
        /// </summary>
        private object GetDefaultValue(FieldInfo fieldInfo)
        {
            // try to get DefaultValueAttribute
            Attribute defaultValue = Attribute.GetCustomAttribute(fieldInfo, typeof(GameEngine.Scripting.DefaultValueAttribute));

            if (defaultValue != null) return ((GameEngine.Scripting.DefaultValueAttribute)defaultValue).DefaultValue;
            else return null;
        }

        /// <summary>
        /// Gets the value from <see cref="GameEngine.Scripting.CategoryAttribute"/> of the specified type.
        /// If not exists returns <c>null</c>.
        /// </summary>
        private string GetCategory(Type type)
        {
            // try to get CategoryAttribute
            Attribute category = Attribute.GetCustomAttribute(type, typeof(GameEngine.Scripting.CategoryAttribute));

            if (category != null) return ((GameEngine.Scripting.CategoryAttribute)category).Category;
            else return null;
        }

        /// <summary>
        /// Finds the script node by the specified realname and type of the node.
        /// </summary>
        /// <param name="realName">Realname in the assembly of the script node.</param>
        /// <param name="nodeType">Type of the script node.</param>
        /// <returns>Script node if found; otherwise <c>null</c>.</returns>
        public static NodeData FindNode(string realName, NodeType nodeType)
        {
            return (NodeData)FindScriptData(Root, scriptData => scriptData is NodeData && ((NodeData)scriptData).RealName == realName && ((NodeData)scriptData).Type == nodeType);
        }

        /// <summary>
        /// Determines whether the specified script data matches the defined expression.
        /// </summary>
        private delegate bool FindExpression(ScriptData scriptData);

        /// <summary>
        /// Finds the script node by the specified expression at the specified category and its children.
        /// </summary>
        private static ScriptData FindScriptData(CategoryData category, FindExpression findExpression)
        {
            foreach (ScriptData scriptData in category.Items)
            {
                if (findExpression(scriptData)) return scriptData;

                if (scriptData is CategoryData)
                {
                    ScriptData data = FindScriptData((CategoryData)scriptData, findExpression);
                    if (data != null) return data;
                }
            }

            return null;
        }

        /// <summary>
        /// Sorts items of the specified catetegory.
        /// </summary>
        private static void SortCategory(CategoryData category)
        {
            foreach (ScriptData scriptData in category.Items)
            {
                if (scriptData is CategoryData)
                {
                    SortCategory((CategoryData)scriptData);
                }
            }

            category.Items.Sort(CompareScriptData);
        }

        /// <summary>
        /// Compares two script data.
        /// </summary>
        private static int CompareScriptData(ScriptData x, ScriptData y)
        {
            CategoryData categoryX = x as CategoryData;
            CategoryData categoryY = y as CategoryData;
            NodeData nodeX = x as NodeData;
            NodeData nodeY = y as NodeData;

            if (categoryX != null && categoryY != null) return categoryX.Name.CompareTo(categoryY.Name);
            else if (categoryX != null && categoryY == null) return -1;
            else if (categoryX == null && categoryY != null) return 1;
            else if (nodeX != null && nodeY != null) return nodeX.Name.CompareTo(nodeY.Name);
            else
            {
                Debug.Assert(true, "Not supported types.");
                return 0;
            }
        }
    }
}
