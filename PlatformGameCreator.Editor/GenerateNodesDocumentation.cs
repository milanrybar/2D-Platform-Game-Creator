/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.Editor.Scripting;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Xml;

namespace PlatformGameCreator.Editor
{
#if DEBUG

    /// <summary>
    /// Generate XML documentation for scripting nodes.
    /// Documentation contains all important information and node thumbnail.
    /// Only internal usage during development.
    /// </summary>
    class GenerateNodesDocumentation
    {
        private ScriptingComponent scriptingComponent;
        private ScriptingScreen scriptingScreen;
        private Rectangle screenRect = new Rectangle(0, 0, 500, 500);
        private Bitmap image;
        private XmlDocument doc;
        private string outputDir;

        internal void Generate(string outputDirectory)
        {
            if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDirectory);

            outputDir = outputDirectory;

            scriptingComponent = new ScriptingComponent(null);
            scriptingScreen = new ScriptingScreen();
            scriptingScreen.State = scriptingComponent.StateMachines[0].StartingState;
            scriptingScreen.Width = screenRect.Width;
            scriptingScreen.Height = screenRect.Height;
            image = new Bitmap(screenRect.Width, screenRect.Height);

            // create xml dcument
            doc = new XmlDocument();
            XmlElement root = (XmlElement)doc.AppendChild(doc.CreateElement("nodes"));

            // generate nodes info and images
            GenerateNodes(ScriptingNodes.Root, root);

            // save xml document
            doc.Save(Path.Combine(outputDir, "nodes.xml"));
        }

        private void GenerateNodes(CategoryData category, XmlElement element)
        {
            foreach (ScriptData baseNode in category.Items)
            {
                // category
                CategoryData cat = baseNode as CategoryData;
                if (cat != null)
                {
                    XmlElement categoryElement = (XmlElement)element.AppendChild(doc.CreateElement("category"));
                    categoryElement.SetAttribute("id", Guid.NewGuid().ToString());
                    categoryElement.SetAttribute("name", cat.Name);

                    GenerateNodes(cat, categoryElement);

                    continue;
                }

                // node
                NodeData node = baseNode as NodeData;
                if (node != null)
                {
                    Node guiNode = (Node)new Node(null, node);

                    GenerateNode(guiNode, element);

                    continue;
                }

                Debug.Assert(true);
            }
        }

        private void GenerateNode(Node node, XmlElement category)
        {
            // generate image
            SceneNode newNode = node.Clone(scriptingComponent.StateMachines[0].StartingState).CreateView();
            scriptingScreen.AddSceneNodeToCenter(newNode);
            newNode.Location = new PointF(20, 10);

            bool hasVariables = false;
            foreach (NodeSocket nodeSocket in node.Sockets)
            {
                if (nodeSocket is VariableNodeSocket && ((VariableNodeSocket)nodeSocket).Visible)
                {
                    hasVariables = true;
                    break;
                }
            }

            scriptingScreen.DrawToBitmap(image, screenRect);

            string filenameImage = Path.Combine(outputDir, node.NodeData.RealName + ".png");
            image.Clone(new RectangleF(0, 0, 20 + newNode.Bounds.Width + 20, 10 + newNode.Bounds.Height + (hasVariables ? 40 : 10)), image.PixelFormat).Save(filenameImage);

            ((NodeView)newNode).Node.Remove();

            // generate xml element for node
            XmlElement nodeElement = (XmlElement)category.AppendChild(doc.CreateElement("node"));
            nodeElement.SetAttribute("id", Guid.NewGuid().ToString());
            ElementAppendChild(nodeElement, "name", node.NodeData.Name);
            ElementAppendChild(nodeElement, "realName", node.NodeData.RealName);
            ElementAppendChild(nodeElement, "description", node.NodeData.Description);
            ElementAppendChild(nodeElement, "type", node.NodeData.Type.ToString());
            ElementAppendChild(nodeElement, "image", node.NodeData.RealName + ".png");

            XmlElement inSocketsElement = null;
            XmlElement outSocketsElement = null;
            XmlElement variableInSocketsElement = null;
            XmlElement variableOutSocketsElement = null;

            // generate sockets
            foreach (NodeSocket nodeSocket in node.Sockets)
            {
                // in socket
                if (nodeSocket is SignalNodeSocket && nodeSocket.Type == NodeSocketType.SignalIn)
                {
                    if (inSocketsElement == null) inSocketsElement = (XmlElement)nodeElement.AppendChild(doc.CreateElement("socketsIn"));
                    XmlElement socketElement = (XmlElement)inSocketsElement.AppendChild(doc.CreateElement("socket"));

                    SignalNodeSocket signalNodeSocket = nodeSocket as SignalNodeSocket;

                    ElementAppendChild(socketElement, "name", signalNodeSocket.NodeSocketData.Name);
                    ElementAppendChild(socketElement, "realName", signalNodeSocket.NodeSocketData.RealName);
                    ElementAppendChild(socketElement, "description", signalNodeSocket.NodeSocketData.Description);
                }
                // out socket
                else if (nodeSocket is SignalNodeSocket && nodeSocket.Type == NodeSocketType.SignalOut)
                {
                    if (outSocketsElement == null) outSocketsElement = (XmlElement)nodeElement.AppendChild(doc.CreateElement("socketsOut"));
                    XmlElement socketElement = (XmlElement)outSocketsElement.AppendChild(doc.CreateElement("socket"));

                    SignalNodeSocket signalNodeSocket = nodeSocket as SignalNodeSocket;

                    ElementAppendChild(socketElement, "name", signalNodeSocket.NodeSocketData.Name);
                    ElementAppendChild(socketElement, "realName", signalNodeSocket.NodeSocketData.RealName);
                    ElementAppendChild(socketElement, "description", signalNodeSocket.NodeSocketData.Description);
                }
                // variable in socket
                else if (nodeSocket is VariableNodeSocket && nodeSocket.Type == NodeSocketType.VariableIn)
                {
                    if (variableInSocketsElement == null) variableInSocketsElement = (XmlElement)nodeElement.AppendChild(doc.CreateElement("socketsVariableIn"));
                    XmlElement socketElement = (XmlElement)variableInSocketsElement.AppendChild(doc.CreateElement("socket"));

                    VariableNodeSocket variableNodeSocket = nodeSocket as VariableNodeSocket;

                    ElementAppendChild(socketElement, "name", variableNodeSocket.NodeSocketData.Name);
                    ElementAppendChild(socketElement, "realName", variableNodeSocket.NodeSocketData.RealName);
                    ElementAppendChild(socketElement, "description", variableNodeSocket.NodeSocketData.Description);
                    ElementAppendChild(socketElement, "isArray", variableNodeSocket.NodeSocketData.IsArray.ToString());
                    ElementAppendChild(socketElement, "type", VariableTypeHelper.FriendlyName(variableNodeSocket.NodeSocketData.VariableType));
                    ElementAppendChild(socketElement, "defaultValue", variableNodeSocket.Value.ToString());
                }
                // variable out socket
                else if (nodeSocket is VariableNodeSocket && nodeSocket.Type == NodeSocketType.VariableOut)
                {
                    if (variableOutSocketsElement == null) variableOutSocketsElement = (XmlElement)nodeElement.AppendChild(doc.CreateElement("socketsVariableOut"));
                    XmlElement socketElement = (XmlElement)variableOutSocketsElement.AppendChild(doc.CreateElement("socket"));

                    VariableNodeSocket variableNodeSocket = nodeSocket as VariableNodeSocket;

                    ElementAppendChild(socketElement, "name", variableNodeSocket.NodeSocketData.Name);
                    ElementAppendChild(socketElement, "realName", variableNodeSocket.NodeSocketData.RealName);
                    ElementAppendChild(socketElement, "description", variableNodeSocket.NodeSocketData.Description);
                    ElementAppendChild(socketElement, "type", VariableTypeHelper.FriendlyName(variableNodeSocket.NodeSocketData.VariableType));
                }
                else Debug.Assert(true);
            }
        }

        private void ElementAppendChild(XmlElement element, string name, string value)
        {
            element.AppendChild(doc.CreateElement(name)).InnerText = value;
        }
    }

#endif
}
