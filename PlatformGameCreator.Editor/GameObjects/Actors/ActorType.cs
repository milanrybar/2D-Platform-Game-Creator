/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.GameObjects.Actors
{
    /// <summary>
    /// Represents type of <see cref="Actor"/>.
    /// </summary>
    [Serializable]
    class ActorType : IName, ISerializable
    {
        /// <summary>
        /// Gets the manager of actor types where is stored.
        /// </summary>
        public ActorTypesManager ActorTypes
        {
            get { return _actorTypes; }
        }
        private ActorTypesManager _actorTypes;

        /// <summary>
        /// Gets or sets the name of the actor type.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (NameChanged != null) NameChanged(this, EventArgs.Empty);
            }
        }
        private string _name;

        /// <summary>
        /// Gets the name including level information of the actor type.
        /// </summary>
        public string NameWithLevelInfo
        {
            get
            {
                StringBuilder nameWithLevel = new StringBuilder(Name.Length);
                for (int i = 0; i < Level; ++i) nameWithLevel.Append("- ");
                nameWithLevel.Append(Name);

                return nameWithLevel.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the value of the actor type.
        /// </summary>
        public uint Value { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets the parent of the ator type.
        /// </summary>
        public ActorType Parent
        {
            get { return _parent; }
        }
        private ActorType _parent;

        /// <summary>
        /// Gets the children of the actor type.
        /// </summary>
        public ChildrenList Children
        {
            get { return _children; }
        }
        private ChildrenList _children;

        /// <summary>
        /// Occurs when the <see cref="Name"/> property value changes.
        /// </summary>
        public event EventHandler NameChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorType"/> class.
        /// </summary>
        /// <param name="actorTypes">The manager of actor types.</param>
        public ActorType(ActorTypesManager actorTypes)
        {
            _actorTypes = actorTypes;
            _children = new ChildrenList(this);
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        private ActorType(SerializationInfo info, StreamingContext ctxt)
        {
            _actorTypes = (ActorTypesManager)info.GetValue("ActorTypes", typeof(ActorTypesManager));
            _name = info.GetString("Name");
            _parent = (ActorType)info.GetValue("Parent", typeof(ActorType));
            _children = (ChildrenList)info.GetValue("Children", typeof(ChildrenList));
        }

        /// <inheritdoc />
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ActorTypes", ActorTypes);
            info.AddValue("Name", Name);
            info.AddValue("Parent", Parent);
            info.AddValue("Children", Children);
        }

        /// <inheritdoc />
        /// <summary>
        /// Returns the name of the actor type.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// List for storing an actor type children.
        /// </summary>
        [Serializable]
        public class ChildrenList : ObservableList<ActorType>
        {
            /// <summary>
            /// Gets the actor type that represents the parent for the children.
            /// </summary>
            public ActorType ActorType
            {
                get { return _actorType; }
            }
            private ActorType _actorType;

            /// <summary>
            /// Initializes a new instance of the <see cref="ChildrenList"/> class.
            /// </summary>
            /// <param name="actorType">Type of the actor.</param>
            public ChildrenList(ActorType actorType)
            {
                _actorType = actorType;
            }

            /// <inheritdoc />
            protected ChildrenList(SerializationInfo info, StreamingContext ctxt)
                : base(info, ctxt)
            {
                _actorType = (ActorType)info.GetValue("ActorType", typeof(ActorType));
            }

            /// <inheritdoc />
            public override void Add(ActorType item)
            {
                Debug.Assert(item != null, "ActorType cannot be null.");

                item._parent = ActorType;
                base.Add(item);

                UpdateActorTypes();
            }

            /// <inheritdoc />
            public override void Insert(int index, ActorType item)
            {
                Debug.Assert(item != null, "ActorType cannot be null.");

                item._parent = ActorType;
                base.Insert(index, item);

                UpdateActorTypes();
            }

            /// <inheritdoc />
            public override void RemoveAt(int index)
            {
                Debug.Assert(this[index].Parent == ActorType, "ActorType does not belong to this collection.");

                this[index]._parent = null;
                base.RemoveAt(index);

                UpdateActorTypes();
            }

            /// <summary>
            /// Updates the actor types values.
            /// </summary>
            private void UpdateActorTypes()
            {
                ActorType.ActorTypes.Update();
            }

            /// <inheritdoc />
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);

                info.AddValue("ActorType", ActorType);
            }
        }
    }

    /// <summary>
    /// Manager of <see cref="ActorType"/>. Actor types are stored in a tree structure.
    /// </summary>
    [Serializable]
    class ActorTypesManager : ISerializable, IDeserializationCallback
    {
        /// <summary>
        /// Gets the root of actor types.
        /// </summary>
        public ActorType Root
        {
            get { return _root; }
        }
        private ActorType _root;

        /// <summary>
        /// Gets all actor types in a single <see cref="List{ActorType}"/>.
        /// </summary>
        public List<ActorType> Items
        {
            get { return _items; }
        }
        private List<ActorType> _items = new List<ActorType>();

        /// <summary>
        /// Occurs when the actor types changes (actor type is added or removed).
        /// </summary>
        public event EventHandler ItemsChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorTypesManager"/> class.
        /// </summary>
        public ActorTypesManager()
        {
            _root = new ActorType(this) { Name = "Root" };

            Update();
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        private ActorTypesManager(SerializationInfo info, StreamingContext ctxt)
        {
            _root = (ActorType)info.GetValue("Root", typeof(ActorType));

            Debug.Assert(_root != null, "Unable to get root actor type.");
        }

        /// <summary>
        /// Number of the leaves in the tree of actor types.
        /// </summary>
        private int countLeaves = 0;

        /// <summary>
        /// Updates all actor types values and updates <see cref="Items"/> property.
        /// Fires <see cref="ItemsChanged"/> event.
        /// </summary>
        public void Update()
        {
            countLeaves = 0;
            _items.Clear();

            Update(Root, 0, _items);

            if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Updates the specified actor type value, stores the specified actor types in the specified list and calls the same method on its children.
        /// </summary>
        /// <param name="actorType">Type of the actor to update</param>
        /// <param name="level">The level in tree of actor types.</param>
        /// <param name="items">The items where to store all actor types.</param>
        private void Update(ActorType actorType, int level, List<ActorType> items)
        {
            actorType.Level = level;
            actorType.Value = 0;

            items.Add(actorType);

            foreach (ActorType child in actorType.Children)
            {
                Update(child, level + 1, items);
                actorType.Value |= child.Value;
            }

            if (actorType.Value == 0) actorType.Value = (uint)(1 << countLeaves++);
        }

        /// <summary>
        /// Finds the actor type by the specified name.
        /// </summary>
        /// <param name="name">The name of the actor type to find.</param>
        /// <returns>Actor type if found; otherwise <c>null</c></returns>
        public ActorType Find(string name)
        {
            foreach (ActorType actorType in Items)
            {
                if (actorType.Name == name) return actorType;
            }

            return null;
        }

        /// <summary>
        /// Finds the actor type by the specified value.
        /// </summary>
        /// <param name="value">The value of the actor type to find.</param>
        /// <returns>Actor type if found; otherwise <c>null</c></returns>
        public ActorType Find(uint value)
        {
            foreach (ActorType actorType in Items)
            {
                if (actorType.Value == value) return actorType;
            }

            return null;
        }

        /// <inheritdoc />
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Root", Root);
        }

        /// <inheritdoc />
        /// <remarks>
        /// Updates all actor types.
        /// </remarks>
        public void OnDeserialization(object sender)
        {
            Update();
        }
    }
}
