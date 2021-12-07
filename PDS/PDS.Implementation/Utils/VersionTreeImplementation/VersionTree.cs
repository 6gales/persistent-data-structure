using System.Collections.Generic;

namespace PDS.Implementation.Utils.VersionTreeImplementation
{
    public class VersionTree<T>
        where T: IEqualityComparer<T>
    {
        public VersionTreeNode<T> Root { get; }

        public VersionTree()
        {
            
        }
    }
}