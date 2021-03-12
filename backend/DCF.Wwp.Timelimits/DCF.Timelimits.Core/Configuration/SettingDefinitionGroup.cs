using System;
using System.Collections.Generic;
using DCF.Common.Exceptions;

namespace DCF.Core.Configuration
{
    /// <summary>
    /// A setting group is used to group some settings togehter.
    /// A group can be child of another group and can have child groups.
    /// </summary>
    public class SettingDefinitionGroup : ISettingDefinitionGroup
    {
        /// <summary>
        /// Unique name of the setting group.
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// Display name of the setting.
        /// This can be used to show setting to the user.
        /// </summary>
        public String DisplayName { get; private set; }

        /// <summary>
        /// Gets parent of this group.
        /// </summary>
        public ISettingDefinitionGroup Parent { get; set; }

        /// <summary>
        /// Gets a list of all children of this group.
        /// </summary>
        public IReadOnlyList<ISettingDefinitionGroup> Children
        {
            get { return this._children.AsReadOnly(); }
        }
        private readonly List<ISettingDefinitionGroup> _children;

        /// <summary>
        /// Creates a new <see cref="SettingDefinitionGroup"/> object.
        /// </summary>
        /// <param name="name">Unique name of the setting group</param>
        /// <param name="displayName">Display name of the setting</param>
        public SettingDefinitionGroup(String name, String displayName)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name parameter is invalid! It can not be null or empty or whitespace", "name"); //TODO: Simpify throwing such exceptions
            }

            this.Name = name;
            this.DisplayName = displayName;
            this._children = new List<ISettingDefinitionGroup>();
        }

        /// <summary>
        /// Adds a <see cref="SettingDefinitionGroup"/> as child of this group.
        /// </summary>
        /// <param name="child">Child to be added</param>
        /// <returns>This child group to be able to add more child</returns>
        public ISettingDefinitionGroup AddChild(ISettingDefinitionGroup child)
        {
            if (child.Parent != null)
            {
                throw new DCFApplicationException("Setting group " + child.Name + " has already a Parent (" + child.Parent.Name + ").");
            }

            this._children.Add(child);
            child.Parent = this;
            return this;
        }
    }
}