using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WinUIApp.Tests.TestHelpers
{
    public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable)
        {
            Provider = new TestAsyncQueryProvider<T>(this);
        }

        public TestAsyncEnumerable(Expression expression) : base(expression)
        {
            Provider = new TestAsyncQueryProvider<T>(this);
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            => new TestAsyncEnumerator<T>(((IEnumerable<T>)this).GetEnumerator());

        public IQueryProvider Provider { get; }

        // Fix for CS0117: Replace the incorrect Expression property implementation  
        public new Expression Expression => ((IQueryable)this).Expression;

        public Type ElementType => typeof(T);

        // Fix for CS0117: Use explicit cast to IEnumerable<T> to get an enumerator  
        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)this).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
