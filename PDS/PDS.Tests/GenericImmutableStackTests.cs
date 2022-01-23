using System;
using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace PDS.Tests
{
    [TestFixture]
    public class GenericImmutableStackTests
    {
        public static Type[] GetImmutableStackTypes()
        {
            var type = typeof(IImmutableStack<>);
            
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p))
                .Where(p => !p.IsInterface);

            return types.ToArray();
        }

        [Test]
        [TestCaseSource(nameof(GetImmutableStackTypes))]
        public void AddTest(Type stackType)
        {
            var classType = stackType.MakeGenericType(typeof(int));
            var stack = (IImmutableStack<int>)Activator.CreateInstance(classType)!;

            false.Should().BeTrue();
        }
        
        
        
    }
}