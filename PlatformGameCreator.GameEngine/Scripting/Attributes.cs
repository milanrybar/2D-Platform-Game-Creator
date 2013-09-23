/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformGameCreator.GameEngine.Scripting
{
    /// <summary>
    /// Defines a friendly name for the target.
    /// </summary>
    /// <example>
    /// Example code from the action node:
    /// <code>
    /// [FriendlyName("Draw Text")]
    /// [Description("Draws text on the specified position for the specified duration.")]
    /// [Category("Actions/Misc")]
    /// public class DrawTextAction : ActionNode
    /// {
    ///     [FriendlyName("Out")]
    ///     [Description("Fires when the action is completed.")]
    ///     public ScriptSocketHandler Out;
    ///     
    ///     [FriendlyName("Position")]
    ///     [Description("Position to draw the specified text on.")]
    ///     [VariableSocket(VariableSocketType.In)]
    ///     public Variable&lt;Vector2> Position;
    ///     
    ///     [FriendlyName("In")]
    ///     [Description("Draws text on the specified position for the specified duration.")]
    ///     public void In()
    ///     {
    ///     }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class FriendlyNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the friendly name.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }
        private string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyNameAttribute"/> class.
        /// </summary>
        /// <param name="name">Friendly name.</param>
        public FriendlyNameAttribute(string name)
        {
            _name = name;
        }
    }

    /// <summary>
    /// Defines default value for the target.
    /// </summary>
    /// <remarks>
    /// Attribute is used to define default values for the scripting variables in the scripting nodes.
    /// For actor and vector are used special attributes <see cref="DefaultValueActorOwnerInstanceAttribute"/> and <see cref="DefaultValueVector2Attribute"/>.
    /// </remarks>
    /// <example>
    /// Example code from the action node:
    /// <code>
    /// public class BasicMovementAction : ActionNode
    /// {
    ///     [VariableSocket(VariableSocketType.In, Visible = false)]
    ///     [DefaultValue(0.9f)]
    ///     public Variable&lt;float> SlowingCoefficient;
    ///     
    ///     [VariableSocket(VariableSocketType.In)]
    ///     [DefaultValue(false)]
    ///     public Variable&lt;bool> MovingRight;
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DefaultValueAttribute : Attribute
    {
        /// <summary>
        /// Gets the default value.
        /// </summary>
        public object DefaultValue
        {
            get { return _defaultValue; }
        }
        private object _defaultValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueAttribute"/> class.
        /// </summary>
        /// <param name="defaultValue">Default value.</param>
        public DefaultValueAttribute(object defaultValue)
        {
            _defaultValue = defaultValue;
        }
    }

    /// <summary>
    /// Defines default value for the actor variable as Owner instance.
    /// </summary>
    /// <example>
    /// Example code from the action node:
    /// <code>
    /// public class SetTextureAction : ActionNode
    /// {
    ///     [VariableSocket(VariableSocketType.In)]
    ///     [DefaultValueActorOwnerInstance]
    ///     public Variable&lt;Actor>[] Instance;
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DefaultValueActorOwnerInstanceAttribute : DefaultValueAttribute
    {
        /// <summary>
        /// Internal default value indicating Owner instance for the actor variable.
        /// </summary>
        public static readonly string ActorOwnerInstance = "OwnerInstance";

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueActorOwnerInstanceAttribute"/> class.
        /// </summary>
        public DefaultValueActorOwnerInstanceAttribute()
            : base(ActorOwnerInstance)
        {
        }
    }

    /// <summary>
    /// Defines default value for the vector variable.
    /// </summary>
    /// <example>
    /// Example code from the action node:
    /// <code>
    /// public class MultiplyVectorAction : ActionNode
    /// {
    ///     [VariableSocket(VariableSocketType.In)]
    ///     [DefaultValueVector2(1f, 1f)]
    ///     public Variable&lt;Vector2>[] A;
    /// }
    /// </code>
    /// </example>
    public class DefaultValueVector2Attribute : DefaultValueAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueVector2Attribute"/> class.
        /// </summary>
        /// <param name="x">X component for vector.</param>
        /// <param name="y">Y component for vector.</param>
        public DefaultValueVector2Attribute(float x, float y)
            : base(new Microsoft.Xna.Framework.Vector2(x, y))
        {
        }
    }

    /// <summary>
    /// Defines a description for the target.
    /// </summary>
    /// <example>
    /// Example code from the action node:
    /// <code>
    /// [FriendlyName("Draw Text")]
    /// [Description("Draws text on the specified position for the specified duration.")]
    /// [Category("Actions/Misc")]
    /// public class DrawTextAction : ActionNode
    /// {
    ///     [Description("Fires when the action is completed.")]
    ///     public ScriptSocketHandler Out;
    ///     
    ///     [FriendlyName("Position")]
    ///     [Description("Position to draw the specified text on.")]
    ///     [VariableSocket(VariableSocketType.In)]
    ///     public Variable&lt;Vector2> Position;
    ///     
    ///     [Description("Draws text on the specified position for the specified duration.")]
    ///     public void In()
    ///     {
    ///     }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class DescriptionAttribute : Attribute
    {
        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description
        {
            get { return _description; }
        }
        private string _description;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionAttribute"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        public DescriptionAttribute(string description)
        {
            _description = description;
        }
    }

    /// <summary>
    /// Defines a category for the scripting node.
    /// </summary>
    /// <remarks>
    /// Character '/' split part of the category.
    /// </remarks>
    /// <example>
    /// Example code from the action node:
    /// <code>
    /// [FriendlyName("Draw Text")]
    /// [Description("Draws text on the specified position for the specified duration.")]
    /// [Category("Actions/Misc")]
    /// public class DrawTextAction : ActionNode
    /// {
    /// }
    /// </code>
    /// DrawTextAction is in Actions -> Misc.
    /// </example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CategoryAttribute : Attribute
    {
        /// <summary>
        /// Gets the category.
        /// </summary>
        public string Category
        {
            get { return _category; }
        }
        private string _category;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryAttribute"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        public CategoryAttribute(string category)
        {
            _category = category;
        }
    }

    /// <summary>
    /// Type of the socket type.
    /// </summary>
    /// <remarks>
    /// Used for differentiation variable socket in the scripting node if it is in or out variable socket in <see cref="VariableSocketAttribute"/>.
    /// </remarks>
    public enum VariableSocketType
    {
        /// <summary>
        /// In variable socket.
        /// </summary>
        In,

        /// <summary>
        /// Out variable socket. 
        /// </summary>
        Out
    };

    /// <summary>
    /// Defines settings for the variable socket in the scripting node.
    /// </summary>
    /// <example>
    /// <code>
    /// public class SomeAction : ActionNode
    /// {
    ///     [VariableSocket(VariableSocketType.In, Visible = false)]
    ///     public Variable&lt;float> SlowingCoefficient;
    ///     
    ///     [VariableSocket(VariableSocketType.In)]
    ///     public Variable&lt;bool> MovingRight;
    ///     
    ///     [VariableSocket(VariableSocketType.In, CanBeEmpty = true)]
    ///     public Variable&lt;bool>[] Bool;
    /// 
    ///     [VariableSocket(VariableSocketType.Out)]
    ///     public Variable&lt;int>[] Result;
    ///     
    ///     [VariableSocket(VariableSocketType.Out, Visible = false)]
    ///     public Variable&lt;float>[] FloatResult;
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class VariableSocketAttribute : Attribute
    {
        /// <summary>
        /// Gets the type of the variable socket.
        /// </summary>
        public VariableSocketType Type
        {
            get { return _type; }
        }
        private VariableSocketType _type;

        /// <summary>
        /// Gets or sets a value indicating whether this variable socket is visible by default at the editor.
        /// Default is true.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this variable socket can be empty. If true, only connections are possible at the editor.
        /// Default is false.
        /// </summary>
        public bool CanBeEmpty { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableSocketAttribute"/> class.
        /// </summary>
        /// <param name="type">The type of the variable socket.</param>
        public VariableSocketAttribute(VariableSocketType type)
        {
            _type = type;
            Visible = true;
            CanBeEmpty = false;
        }
    }
}
