using System;
using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PDS.Collections;
using PDS.Implementation.Collections;

namespace PDS.Tests
{
    [TestFixture]
    public class GenericImmutableStackTests
    {
        public static Type[] GetImmutableStackTypes()
        {
            var t = typeof(PersistentStack<>);
            var type = typeof(IPersistentStack<>);
            
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => t.IsAssignableFrom(p))
                //.Where(p => !p.IsInterface)
                .ToArray();

            return types;
        }

        private void ImmutableStackTest(IImmutableStack<int> a)
        {
            a.IsEmpty.Should().Be(true);
            var b = a.Push(0);
 
            a.IsEmpty.Should().Be(true);
            b.IsEmpty.Should().Be(false);
            b.Peek().Should().Be(0);

            var c = b.Pop();
        
            b.IsEmpty.Should().Be(false);
            b.Peek().Should().Be(0);
            c.IsEmpty.Should().Be(true);
            
            var d = b.Push(1);
        
            b.IsEmpty.Should().Be(false);
            b.Peek().Should().Be(0);

            d.IsEmpty.Should().Be(false);
            d.Peek().Should().Be(1);

            d.Clear().IsEmpty.Should().Be(true);
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