/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using PlatformGameCreator.Editor.Assets;
using PlatformGameCreator.Editor.Common;
using PlatformGameCreator.Editor.GameObjects.Actors;
using System.Diagnostics;
using PlatformGameCreator.Editor.Scripting;
using PlatformGameCreator.Editor.Assets.Textures;
using PlatformGameCreator.Editor.Assets.Animations;
using PlatformGameCreator.Editor.Scenes;

namespace PlatformGameCreator.Editor.Building
{
    /// <summary>
    /// Generates the source code in C# for the actor. Only used by <see cref="SceneGenerator"/>.
    /// </summary>
    class ActorGenerator
    {
        private TextWriter writer;
        private Actor actor;

        /// <summary>
        /// Generates the specified vector.
        /// </summary>
        /// <param name="vector">The vector to generate.</param>
        protected void GenerateVector2(Vector2 vector)
        {
            writer.Write("new Vector2({0}f, {1}f)", vector.X, vector.Y);
        }

        /// <summary>
        /// Generates the specified vector in the simulation units.
        /// </summary>
        /// <param name="vector">The vector to generate.</param>
        protected void GenerateVector2InSimulationUnits(Vector2 vector)
        {
            writer.Write("new Vector2({0}f, {1}f)",
                GameEngine.ConvertUnits.ToSimUnits(vector.X),
                GameEngine.ConvertUnits.ToSimUnits(vector.Y));
        }

        /// <summary>
        /// Gets a name of class for the specified actor.
        /// </summary>
        /// <param name="actor">The actor to get class name for.</param>
        /// <returns>Name of class for the specified actor.</returns>
        public static string GetActorClassName(Actor actor)
        {
            return String.Format("Actor{0}", actor.Id);
        }

        /// <summary>
        /// Generates the fixture for the current actor for the specified polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        private void GenerateActorFixture(Polygon polygon)
        {
            Debug.Assert(polygon.Vertices.IsCounterClockWise(), "Polygon is not counter clock wise.");
            Debug.Assert(polygon.Vertices.Count >= 3, "Polygon is not correct.");
            Debug.Assert(!polygon.Vertices.CheckPolygon(), "Invalid polygon.");

            writer.WriteLine("{");

            writer.Write("Vertices polygonVertices = new Vertices(new Vector2[] { ");
            for (int i = 0; i < polygon.Vertices.Count; ++i)
            {
                GenerateVector2InSimulationUnits(polygon.Vertices[i]);
                if (i != polygon.Vertices.Count - 1) writer.Write(", ");
            }
            writer.WriteLine("});");

            // FixtureFactory.AttachPolygon(Vertices vertices, float density, Body body, object userData)
            writer.WriteLine("FixtureFactory.AttachPolygon(polygonVertices, {0}f, Body, this);", actor.Physics.Density);

            writer.WriteLine("}");
        }

        /// <summary>
        /// Generates the fixture for the current actor for the specified circle.
        /// </summary>
        /// <param name="circle">The circle.</param>
        private void GenerateActorFixture(Circle circle)
        {
            writer.WriteLine("{");

            // FixtureFactory.AttachCircle(float radius, float density, Body body, Vector2 offset, object userData)
            writer.Write("FixtureFactory.AttachCircle({0}f, {1}f, Body, ",
                GameEngine.ConvertUnits.ToSimUnits(circle.Radius), actor.Physics.Density);
            GenerateVector2InSimulationUnits(circle.Origin);
            writer.WriteLine(", this);");

            writer.WriteLine("}");
        }

        /// <summary>
        /// Generates the fixture for the current actor for the specified edge.
        /// </summary>
        /// <param name="edge">The edge.</param>
        private void GenerateActorFixture(Edge edge)
        {
            Debug.Assert(edge.Vertices.Count >= 2, "Edge is not correct.");

            for (int i = 1; i < edge.Vertices.Count; ++i)
            {
                writer.WriteLine("{");

                writer.Write("EdgeShape edgeShape = new EdgeShape(");
                GenerateVector2InSimulationUnits(edge.Vertices[i - 1]);
                writer.Write(", ");
                GenerateVector2InSimulationUnits(edge.Vertices[i]);
                writer.WriteLine(");");

                if (i - 2 >= 0)
                {
                    writer.WriteLine("edgeShape.HasVertex0 = true;");
                    writer.Write("edgeShape.Vertex0 = ");
                    GenerateVector2InSimulationUnits(edge.Vertices[i - 2]);
                    writer.WriteLine(";");
                }

                if (i + 1 < edge.Vertices.Count)
                {
                    writer.WriteLine("edgeShape.HasVertex3 = true;");
                    writer.Write("edgeShape.Vertex3 = ");
                    GenerateVector2InSimulationUnits(edge.Vertices[i + 1]);
                    writer.WriteLine(";");
                }

                writer.WriteLine("edgeShape.Density = {0}f;", actor.Physics.Density);

                writer.WriteLine("Body.CreateFixture(edgeShape, this);");

                writer.WriteLine("}");
            }
        }

        /// <summary>
        /// Generates fixtures for the current actor.
        /// </summary>
        private void GenerateActorFixtures()
        {
            if (actor.Shapes == null) return;

            foreach (Shape shape in actor.Shapes)
            {
                switch (shape.Type)
                {
                    case Shape.ShapeType.Polygon:
                        Debug.Assert(shape is Polygon, "Invalid polygon class.");

                        List<Polygon> convexPolygons = ((Polygon)shape).ConvexDecomposition();
                        if (convexPolygons == null)
                        {
                            convexPolygons = new List<Polygon>();
                            convexPolygons.Add((Polygon)shape.Clone());
                        }

                        foreach (Polygon polygon in convexPolygons)
                        {
                            polygon.Vertices.ForceCounterClockWise();
                            polygon.Scale(actor.ScaleFactor, new Vector2());

                            GenerateActorFixture(polygon);
                        }

                        break;

                    case Shape.ShapeType.Circle:
                        Debug.Assert(shape is Circle, "Invalid circle class.");

                        Circle circle = (Circle)shape.Clone();
                        circle.Scale(actor.ScaleFactor, new Vector2());

                        GenerateActorFixture(circle);

                        break;

                    case Shape.ShapeType.Edge:
                        Debug.Assert(shape is Edge, "Invalid edge class.");

                        Edge edge = (Edge)shape.Clone();
                        edge.Scale(actor.ScaleFactor, new Vector2());

                        GenerateActorFixture(edge);

                        break;

                    default:
                        Debug.Assert(true, "Not supported shape.");
                        break;
                }
            }
        }

        /// <summary>
        /// Generates source code for setting the physics for the current actor.
        /// </summary>
        private void GenerateActorPhysics()
        {
            // only for physics body
            if (actor.Physics.Type == PhysicsComponent.BodyPhysicsType.None || (actor.Layer != null && actor.Layer.IsParallax)) return;

            // create body
            writer.Write("_body = BodyFactory.CreateBody(Screen.World, ");
            GenerateVector2InSimulationUnits(actor.Position);
            writer.WriteLine(", this);");

            // set body type
            switch (actor.Physics.Type)
            {
                case PhysicsComponent.BodyPhysicsType.Dynamic:
                    writer.WriteLine("Body.BodyType = BodyType.Dynamic;");
                    break;

                case PhysicsComponent.BodyPhysicsType.Static:
                    writer.WriteLine("Body.BodyType = BodyType.Static;");
                    break;

                case PhysicsComponent.BodyPhysicsType.Kinematic:
                    writer.WriteLine("Body.BodyType = BodyType.Kinematic;");
                    break;

                default:
                    Debug.Assert(true, "Not supported physics type.");
                    break;
            }

            // set rotation
            writer.WriteLine("Body.Rotation = {0}f;", actor.Angle);

            // set fixtures
            GenerateActorFixtures();

            // set physics variables
            writer.WriteLine("Body.AngularDamping = {0}f;", actor.Physics.AngularDamping);
            writer.WriteLine("Body.IsBullet = {0};", GetBool(actor.Physics.Bullet));
            writer.WriteLine("Body.FixedRotation = {0};", GetBool(actor.Physics.FixedRotation));
            writer.WriteLine("Body.Friction = {0}f;", actor.Physics.Friction);
            writer.WriteLine("Body.LinearDamping = {0}f;", actor.Physics.LinearDamping);
            writer.WriteLine("Body.Restitution = {0}f;", actor.Physics.Restitution);
            writer.WriteLine("Body.IsSensor = {0};", GetBool(actor.Physics.Sensor));
            writer.WriteLine("OneWayPlatform = {0};", GetBool(actor.Physics.OneWayPlatform));
        }

        /// <summary>
        /// Gets a value of the specified bool in string.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Value of the specified bool in strin.</returns>
        public static string GetBool(bool value)
        {
            return value ? "true" : "false";
        }

        /// <summary>
        /// Gets the index of the layer for the specified actor.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <returns>Index of the layer.</returns>
        public static float GetLayerIndex(Actor actor)
        {
            return ((actor.GetLayer().Index + 1) << 16) + (actor.Index + 1);
        }

        /// <summary>
        /// Generates the source code for setting the settings of the current actor.
        /// </summary>
        private void GenerateActorSettings()
        {
            // set active
            writer.WriteLine("Active = true;");
            // set visible
            writer.WriteLine("Visible = {0};", GetBool(actor.DrawableAssetVisible));
            // set layer
            writer.WriteLine("Layer = {0}f;", GetLayerIndex(actor));
            // set position
            writer.Write("Position = ");
            GenerateVector2InSimulationUnits(actor.Position);
            writer.WriteLine(";");
            // set angle
            writer.WriteLine("Angle = {0}f;", actor.Angle);
            // set scale factor
            writer.Write("_scale = ");
            GenerateVector2(actor.ScaleFactor);
            writer.WriteLine(";");
            // set texture/animation
            if (actor.DrawableAsset != null) writer.WriteLine("if(Screen.GraphicsAssets.Get({0}) != null) Renderer = Screen.GraphicsAssets.Get({0}).CreateActorRenderer(this);", actor.DrawableAsset.Id);
            // set actor type
            writer.WriteLine("ActorType = {0};", actor.Type.Value);
            // actor on parallax layer
            if (actor.Layer != null && actor.Layer.IsParallax)
            {
                writer.WriteLine("IsParallax = true;");
                writer.Write("ParallaxCoefficient = ");
                GenerateVector2InSimulationUnits(actor.Layer.ParallaxCoefficient);
                writer.WriteLine(";");
                if (actor.Layer.GraphicsEffect != SceneElementEffect.None) writer.WriteLine("GraphicsEffect = (GraphicsEffect){0};", (int)actor.Layer.GraphicsEffect);
            }

            // set physics
            GenerateActorPhysics();

            // actor variables
            GenerateActorVariables(actor.Scripting);
            // actor events
            GenerateActorEvents(actor.Scripting);

            // init children
            for (int i = actor.Children.Count - 1; i >= 0; --i)
            {
                writer.Write("Children.Add(new {0}()", GetActorClassName(actor.Children[i]));
                writer.Write(" { ");
                writer.Write("ActorId = {0}", actor.Children[i].Id);
                writer.WriteLine(" });");
            }
        }

        /// <summary>
        /// Generates the source code for initializing actor variables from the specified scripting.
        /// </summary>
        /// <param name="scripting">The scripting to get actor variables from.</param>
        private void GenerateActorVariables(ScriptingComponent scripting)
        {
            // init variables
            foreach (NamedVariable variable in scripting.Variables)
            {
                writer.WriteLine(@"variables.Add(""{0}"", new Variable<{1}>());",
                   variable.Name, GetVariableGameEngineType(variable.VariableType));
            }
        }

        /// <summary>
        /// Generates the source code for initializing actor events from the specified scripting.
        /// </summary>
        /// <param name="scripting">The scripting to get actor events from.</param>
        private void GenerateActorEvents(ScriptingComponent scripting)
        {
            // init events
            foreach (Event scriptEvent in scripting.EventsOut)
            {
                writer.WriteLine(@"events.Add(""{0}"", new EventWrapper());", scriptEvent.Name);
            }
        }

        /// <summary>
        /// Converts the specified variable type from the editor type to the game (GameEngine) type.
        /// </summary>
        /// <param name="variableType">Type of the variable to convert.</param>
        /// <returns>Name of type in the game (GameEngine).</returns>
        private string GetVariableGameEngineType(VariableType variableType)
        {
            switch (variableType)
            {
                case VariableType.Actor:
                    return "Actor";

                case VariableType.ActorType:
                    return "uint";

                case VariableType.Bool:
                    return "bool";

                case VariableType.Float:
                    return "float";

                case VariableType.Int:
                    return "int";

                case VariableType.String:
                    return "string";

                case VariableType.Vector2:
                    return "Vector2";

                case VariableType.Sound:
                    return "SoundEffect";

                case VariableType.Song:
                    return "Song";

                case VariableType.Path:
                    return "Path";

                case VariableType.Texture:
                    return "TextureData";

                case VariableType.Animation:
                    return "AnimationData";

                case VariableType.Scene:
                    return "Screen";

                case VariableType.Key:
                    return "Keys";

                default:
                    Debug.Assert(true, "Not supported variable type.");
                    return null;
            }
        }

        private List<Actor> actorParents = new List<Actor>();
        private List<Actor> actorValueParents = new List<Actor>();
        private List<int> actorHierarchyPath = new List<int>();
        private StringBuilder str = new StringBuilder();

        /// <summary>
        /// Computes the hierarchy path that represents the path from the current actor to the specified actor.
        /// Path: value of -1 represents parent and positive value represents the index of the child.
        /// Hierarchy path will be stored at <see cref="actorHierarchyPath"/>. 
        /// When the <paramref name="value"/> is not found then the hiearchy path is empty.
        /// </summary>
        /// <param name="value">The actor to find.</param>
        private void ComputeActorHierarchyPath(Actor value)
        {
            actorParents.Clear();
            actorValueParents.Clear();
            actorHierarchyPath.Clear();

            Actor parent = actor;
            actorParents.Add(actor);
            while (parent.Parent != null)
            {
                parent = parent.Parent;

                if (parent == value)
                {
                    for (int i = actorParents.Count - 1; i >= 0; --i)
                    {
                        actorHierarchyPath.Add(-1);
                    }

                    return;
                }
                else
                {
                    actorParents.Add(parent);
                }
            }

            parent = value;
            int parentIndex = -1;
            actorValueParents.Add(value);
            while (parent.Parent != null)
            {
                parent = parent.Parent;

                if ((parentIndex = actorParents.IndexOf(parent)) != -1)
                {
                    for (int i = parentIndex - 1; i >= 0; --i)
                    {
                        actorHierarchyPath.Add(-1);
                    }

                    for (int i = actorValueParents.Count - 1; i >= 0; --i)
                    {
                        actorHierarchyPath.Add(actorValueParents[i].Parent.Children.Count - 1 - actorValueParents[i].Index);
                    }

                    return;
                }
                else
                {
                    actorValueParents.Add(parent);
                }
            }
        }

        /// <summary>
        /// Gets the specified actor value in string that can be relative to the current actor.
        /// The specified actor can be: null, current actor, relative to the current actor or not relative then it is actor at the scene.
        /// </summary>
        /// <param name="actorValue">The actor to get.</param>
        /// <returns>Value in string of the specified value.</returns>
        private string GetActorValue(Actor actorValue)
        {
            if (actorValue != null)
            {
                if (actorValue == actor) return "this";
                else
                {
                    if (actor != null) ComputeActorHierarchyPath(actorValue);

                    if (actorHierarchyPath.Count == 0 || actor == null)
                    {
                        return String.Format("Screen.Actors.Get({0})", actorValue.Id);
                    }
                    else
                    {
                        str.Clear();

                        str.Append("GetActorInHierarchy(new int[] { ");
                        for (int i = 0; i < actorHierarchyPath.Count; ++i)
                        {
                            if (i != 0) str.Append(", ");
                            str.Append(actorHierarchyPath[i]);
                        }
                        str.Append(" }, ");
                        str.Append(actorValue.Id);
                        str.Append(")");

                        return str.ToString();
                    }
                }
            }
            else return "null";
        }

        /// <summary>
        /// Gets the specified variable value in string.
        /// </summary>
        /// <param name="variable">The variable to get value from.</param>
        /// <returns>Value of the specified variable.</returns>
        private string GetVariableValue(IVariable variable)
        {
            switch (variable.VariableType)
            {
                case VariableType.Actor:
                    Debug.Assert(variable is ActorVar, "Invalid actor variable class.");
                    ActorVar actorVar = variable as ActorVar;

                    if (actorVar.Value != null) return GetActorValue(actorVar.Value);
                    else return actorVar.Owner ? "this" : "null";

                case VariableType.ActorType:
                    Debug.Assert(variable is ActorTypeVar, "Invalid actor type variable class.");

                    return ((ActorTypeVar)variable).Value.Value.ToString();

                case VariableType.Bool:
                    Debug.Assert(variable is BoolVar, "Invalid bool variable class.");

                    return ((BoolVar)variable).Value ? "true" : "false";

                case VariableType.Float:
                    Debug.Assert(variable is FloatVar, "Invalid float variable class.");

                    return String.Format("{0}f", ((FloatVar)variable).Value);

                case VariableType.Int:
                    Debug.Assert(variable is IntVar, "Invalid int variable class.");

                    return ((IntVar)variable).Value.ToString();

                case VariableType.String:
                    Debug.Assert(variable is StringVar, "Invalid string variable class.");

                    return String.Format(@"""{0}""", ((StringVar)variable).Value);

                case VariableType.Vector2:
                    Debug.Assert(variable is Vector2Var, "Invalid Vector2 variable class.");

                    return String.Format("new Vector2({0}f, {1}f)", ((Vector2Var)variable).Value.X, ((Vector2Var)variable).Value.Y);

                case VariableType.Sound:
                    Debug.Assert(variable is SoundVar, "Invalid Sound variable class.");

                    if (((SoundVar)variable).Value == null) return "null";
                    else return String.Format(@"Screen.Content.Load<SoundEffect>(""{0}"")", ((SoundVar)variable).Value.Id);

                case VariableType.Song:
                    Debug.Assert(variable is SoundVar, "Invalid Song variable class.");

                    if (((SoundVar)variable).Value == null) return "null";
                    else return String.Format(@"Screen.Content.Load<Song>(""{0}S"")", ((SoundVar)variable).Value.Id);

                case VariableType.Path:
                    Debug.Assert(variable is PathVar, "Invalid Path variable class.");

                    if (((PathVar)variable).Value == null) return "null";
                    else return String.Format("Screen.Paths.Get({0})", ((PathVar)variable).Value.Id);

                case VariableType.Texture:
                    Debug.Assert(variable is TextureVar, "Invalid Texture variable class.");

                    if (((TextureVar)variable).Value == null) return "null";
                    else return String.Format("(TextureData)Screen.GraphicsAssets.Get({0})", ((TextureVar)variable).Value.Id);

                case VariableType.Animation:
                    Debug.Assert(variable is AnimationVar, "Invalid Animation variable class.");

                    if (((AnimationVar)variable).Value == null) return "null";
                    else return String.Format("(AnimationData)Screen.GraphicsAssets.Get({0})", ((AnimationVar)variable).Value.Id);

                case VariableType.Scene:
                    Debug.Assert(variable is SceneVar, "Invalid scene variable class.");

                    if (((SceneVar)variable).Value == null) return "null";
                    else return String.Format("new {0}()", SceneGenerator.GetSceneFullClassName(((SceneVar)variable).Value));

                case VariableType.Key:
                    Debug.Assert(variable is KeyVar, "Invalid key variable class.");

                    return String.Format("Keys.{0}", ((KeyVar)variable).Value.ToString());

                default:
                    Debug.Assert(true, "Not supported variable type.");
                    return null;
            }
        }

        /// <summary>
        /// Generates the source code for initializing scripting nodes for the specified state.
        /// </summary>
        /// <param name="state">The state to generate scripting nodes from.</param>
        /// <param name="stateVariableName">Name of the state variable.</param>
        private void GenerateScriptNodes(State state, string stateVariableName)
        {
            Dictionary<BaseNode, string> baseNodes = new Dictionary<BaseNode, string>();

            // init script nodes and variables
            for (int i = 0; i < state.Nodes.Count; ++i)
            {
                if (state.Nodes[i] is Node)
                {
                    Node node = state.Nodes[i] as Node;

                    writer.WriteLine("{0} script{1} = new {0}();", node.NodeData.RealName, i);
                    writer.WriteLine("script{0}.Container = {1};", i, stateVariableName);

                    baseNodes[node] = String.Format("script{0}", i);
                }
                else if (state.Nodes[i] is Variable)
                {
                    Variable variable = state.Nodes[i] as Variable;

                    if (variable.NamedVariable == null)
                    {
                        writer.WriteLine("Variable<{0}> variable{1} = new Variable<{0}>({2});",
                            GetVariableGameEngineType(variable.VariableType), i,
                            GetVariableValue(variable.Value));

                        baseNodes[variable] = String.Format("variable{0}", i);
                    }
                }
            }

            // set script nodes settings and create connections
            for (int i = 0; i < state.Nodes.Count; ++i)
            {
                if (state.Nodes[i] is Node)
                {
                    Node node = state.Nodes[i] as Node;

                    foreach (NodeSocket socket in node.Sockets)
                    {
                        if (socket.Type == NodeSocketType.SignalOut)
                        {
                            Debug.Assert(socket is SignalNodeSocket, "Invalid node signal out socket class.");
                            SignalNodeSocket signalSocket = socket as SignalNodeSocket;

                            // connect to node in sockets
                            foreach (SignalNodeSocket inSocket in signalSocket.Connections)
                            {
                                writer.WriteLine("{0}.{1} += {2}.{3};",
                                    baseNodes[node], signalSocket.NodeSocketData.RealName,
                                    baseNodes[inSocket.Node], inSocket.NodeSocketData.RealName);
                            }
                        }
                        else if (socket.Type == NodeSocketType.VariableIn || socket.Type == NodeSocketType.VariableOut)
                        {
                            Debug.Assert(socket is VariableNodeSocket, "Invalid node variable socket class.");
                            VariableNodeSocket variableSocket = socket as VariableNodeSocket;

                            // no connection is made so use default value if possible
                            if (variableSocket.Connections.Count == 0)
                            {
                                // no value is possible
                                if (variableSocket.NodeSocketData.CanBeEmpty || variableSocket.Type == NodeSocketType.VariableOut || GetVariableValue(variableSocket.Value) == "null")
                                {
                                    writer.WriteLine("{0}.{1} = null;", baseNodes[node],
                                      variableSocket.NodeSocketData.RealName);
                                }
                                else
                                {
                                    // default value in array
                                    if (variableSocket.NodeSocketData.IsArray)
                                    {
                                        writer.WriteLine("{0}.{1} = new Variable<{2}>[1];", baseNodes[node],
                                            variableSocket.NodeSocketData.RealName,
                                            GetVariableGameEngineType(variableSocket.VariableType));

                                        writer.WriteLine("{0}.{1}[0] = new Variable<{2}>({3});", baseNodes[node],
                                            variableSocket.NodeSocketData.RealName,
                                            GetVariableGameEngineType(variableSocket.VariableType),
                                            GetVariableValue(variableSocket.Value));
                                    }
                                    // default value for variable
                                    else
                                    {
                                        writer.WriteLine("{0}.{1} = new Variable<{2}>({3});", baseNodes[node],
                                            variableSocket.NodeSocketData.RealName,
                                            GetVariableGameEngineType(variableSocket.VariableType),
                                            GetVariableValue(variableSocket.Value));
                                    }
                                }
                            }
                            // connect to variables
                            else
                            {
                                if (variableSocket.NodeSocketData.IsArray)
                                {
                                    // initialize array
                                    writer.WriteLine("{0}.{1} = new Variable<{2}>[{3}];", baseNodes[node],
                                        variableSocket.NodeSocketData.RealName,
                                        GetVariableGameEngineType(variableSocket.VariableType),
                                        variableSocket.Connections.Count);
                                }

                                for (int j = 0; j < variableSocket.Connections.Count; ++j)
                                {
                                    if (variableSocket.Connections[j].NamedVariable == null)
                                    {
                                        writer.WriteLine(variableSocket.NodeSocketData.IsArray ? "{0}.{1}[{2}] = {3};" : "{0}.{1} = {3};",
                                            baseNodes[node], variableSocket.NodeSocketData.RealName,
                                            j, baseNodes[variableSocket.Connections[j]]);
                                    }
                                    // local variable
                                    else if ((actor == null && variableSocket.Connections[j].NamedVariable.ScriptingComponent.Actor == null) || (actor != null && variableSocket.Connections[j].NamedVariable.ScriptingComponent == actor.Scripting))
                                    {
                                        writer.WriteLine(variableSocket.NodeSocketData.IsArray ? @"{0}.{1}[{2}] = GetVariable<{3}>(""{4}"");" : @"{0}.{1} = GetVariable<{3}>(""{4}"");",
                                            baseNodes[node], variableSocket.NodeSocketData.RealName, j,
                                            GetVariableGameEngineType(variableSocket.VariableType),
                                            variableSocket.Connections[j].NamedVariable.Name);
                                    }
                                    // other actor variable
                                    else
                                    {
                                        writer.WriteLine(variableSocket.NodeSocketData.IsArray ? @"{0}.{1}[{2}] = {3}.GetVariable<{4}>(""{5}"");" : @"{0}.{1} = {3}.GetVariable<{4}>(""{5}"");",
                                            baseNodes[node], variableSocket.NodeSocketData.RealName, j,
                                            variableSocket.Connections[j].NamedVariable.ScriptingComponent.Actor != null ? GetActorValue(variableSocket.Connections[j].NamedVariable.ScriptingComponent.Actor) : "Screen.Actors.Get(0)",
                                            GetVariableGameEngineType(variableSocket.VariableType),
                                            variableSocket.Connections[j].NamedVariable.Name);
                                    }
                                }
                            }
                        }
                    }

                    // event node
                    if (node.NodeData.Type == NodeType.Event)
                    {
                        writer.WriteLine("{0}.Connect();", baseNodes[node]);
                    }
                }
            }
        }

        /// <summary>
        /// Generates the source code for initializing state machines from the specified scripting.
        /// </summary>
        /// <param name="scripting">Scripting to get state machines from.</param>
        private void GenerateStateMachines(ScriptingComponent scripting)
        {
            Debug.Assert(scripting.StateMachines.Count != 0, "One state machine is required.");

            writer.WriteLine("stateMachines = new State[{0}];", scripting.StateMachines.Count);

            Dictionary<State, string> states = new Dictionary<State, string>();

            // init states
            for (int i = 0; i < scripting.StateMachines.Count; ++i)
            {
                Debug.Assert(scripting.StateMachines[i].States.Count != 0, "One state is required.");

                for (int j = 0; j < scripting.StateMachines[i].States.Count; ++j)
                {
                    State state = scripting.StateMachines[i].States[j];
                    states[state] = String.Format("stateMachine{0}state{1}", i, j);

                    writer.WriteLine("State {0} = new State(this, {1});", states[state], i);
                }

                writer.WriteLine("stateMachines[{0}] = {1};", i,
                    scripting.StateMachines[i].StartingState != null ? states[scripting.StateMachines[i].StartingState] : states[scripting.StateMachines[i].States[0]]);
            }

            // create connections
            for (int i = 0; i < scripting.StateMachines.Count; ++i)
            {
                for (int j = 0; j < scripting.StateMachines[i].States.Count; ++j)
                {
                    State state = scripting.StateMachines[i].States[j];

                    foreach (Transition transition in state.Transitions)
                    {
                        Debug.Assert(state == transition.StateFrom, "Incorrect format of transition.");

                        if (transition.Event != null && transition.StateTo != null)
                        {
                            writer.WriteLine(@"{0}.AddTransition(""{1}"", {2});", states[state],
                                transition.Event.Name, states[transition.StateTo]);
                        }
                    }
                }
            }

            // create script nodes
            for (int i = 0; i < scripting.StateMachines.Count; ++i)
            {
                for (int j = 0; j < scripting.StateMachines[i].States.Count; ++j)
                {
                    State state = scripting.StateMachines[i].States[j];

                    writer.WriteLine("{");
                    GenerateScriptNodes(state, states[state]);
                    writer.WriteLine("}");
                }
            }
        }

        private void GenerateActorScripting(ScriptingComponent scripting)
        {
            // set local variables values
            foreach (NamedVariable variable in scripting.Variables)
            {
                writer.WriteLine(@"GetVariable<{0}>(""{1}"").Value = {2};",
                   GetVariableGameEngineType(variable.VariableType), variable.Name,
                   GetVariableValue(variable.Value));
            }

            // generate state machines
            GenerateStateMachines(scripting);
        }

        /// <summary>
        /// Generates the source code in C# for the specified actor.
        /// </summary>
        /// <param name="actor">The actor to generate the source code for.</param>
        /// <param name="textWriter">The <see cref="TextWriter"/> for writing the source code of the scene.</param>
        public void Generate(Actor actor, TextWriter textWriter)
        {
            this.actor = actor;
            writer = textWriter;

            writer.WriteLine("class {0} : Actor", GetActorClassName(actor));
            writer.WriteLine("{");

            writer.WriteLine("public override void InitializeActor()");
            writer.WriteLine("{");
            GenerateActorSettings();
            writer.WriteLine("base.InitializeActor();");
            writer.WriteLine("}");

            writer.WriteLine("public override void InitializeComponents()");
            writer.WriteLine("{");
            GenerateActorScripting(actor.Scripting);
            writer.WriteLine("base.InitializeComponents();");
            writer.WriteLine("}");

            writer.WriteLine("public override Actor Create()");
            writer.WriteLine("{");
            writer.WriteLine("return new {0}();", GetActorClassName(actor));
            writer.WriteLine("}");

            writer.WriteLine("}");
        }


        /// <summary>
        /// Generates the source code in C# for the specified scripting that represents scene scripting.
        /// </summary>
        /// <param name="globalScripting">The scripting to generate the source code for.</param>
        /// <param name="textWriter">The <see cref="TextWriter"/> for writing the source code of the scene.</param>
        public void GenerateGlobalSceneActor(ScriptingComponent globalScripting, TextWriter textWriter)
        {
            actor = null;
            writer = textWriter;

            writer.WriteLine("class GlobalSceneActor : Actor");
            writer.WriteLine("{");

            writer.WriteLine("public override void InitializeActor()");
            writer.WriteLine("{");

            writer.WriteLine("Active = true;");

            GenerateActorVariables(globalScripting);
            GenerateActorEvents(globalScripting);

            writer.WriteLine("base.InitializeActor();");
            writer.WriteLine("}");

            writer.WriteLine("public override void InitializeComponents()");
            writer.WriteLine("{");
            GenerateActorScripting(globalScripting);
            writer.WriteLine("base.InitializeComponents();");
            writer.WriteLine("}");

            writer.WriteLine("public override Actor Create()");
            writer.WriteLine("{");
            writer.WriteLine("throw new NotImplementedException();");
            writer.WriteLine("}");

            writer.WriteLine("}");
        }
    }
}
