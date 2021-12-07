using System.Collections.Generic;

namespace PDS.Implementation.Utils.VersionTreeImplementation
{
    public class VersionTreeNode<T>
        where T: IEqualityComparer<T>
    {
        public VersionTreeNode<T>? Parent { get; set; }
        public List<VersionTreeNode<T>> Children { get; set; } = new List<VersionTreeNode<T>>();
        public T Version { get; }
        
        
        public VersionTreeNode(T version) : this(null, version) { }
        
        public VersionTreeNode(VersionTreeNode<T>? parent, T version)
        {
            Parent = parent;
            Version = version;
        }

        public VersionTreeNode<T> AddChildVersion(T childVersion)
        {
            var newNode = new VersionTreeNode<T>(this, childVersion);
            Children.Add(newNode);

            return newNode;
        }

        public VersionTreeNode<T>? GetVersionTreeNode(T version)
        {
            if (Version.Equals(version))
            {
                return this;
            }

            return Parent?.GetVersionTreeNode(version);
        }
    }
}