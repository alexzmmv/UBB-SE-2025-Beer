using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace WinUIApp.Tests.UnitTests.TestHelpers
{
    internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider innerQueryProvider;

        internal TestAsyncQueryProvider(IQueryProvider innerQueryProvider)
        {
            this.innerQueryProvider = innerQueryProvider;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return this.innerQueryProvider.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return this.innerQueryProvider.Execute<TResult>(expression);
        }

        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            return new TestAsyncEnumerable<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            Type expectedResultType = typeof(TResult).GetGenericArguments()[0];
            object executionResult = typeof(IQueryProvider)
                .GetMethod(
                    name: nameof(IQueryProvider.Execute),
                    genericParameterCount: 1,
                    types: new[] { typeof(Expression) })
                .MakeGenericMethod(expectedResultType)
                .Invoke(this, new[] { expression });

            return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
                .MakeGenericMethod(expectedResultType)
                .Invoke(null, new[] { executionResult });
        }
    }

    internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        private readonly IQueryProvider provider;
        private readonly Expression expression;

        public TestAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        {
            this.provider = new TestAsyncQueryProvider<T>(enumerable.AsQueryable().Provider);
            this.expression = enumerable.AsQueryable().Expression;
        }

        public TestAsyncEnumerable(Expression expression)
            : base(expression)
        {
            this.provider = new TestAsyncQueryProvider<T>(this);
            this.expression = expression;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IQueryProvider IQueryable.Provider => this.provider;
        public Expression Expression => this.expression;
    }

    internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> innerEnumerator;

        public TestAsyncEnumerator(IEnumerator<T> innerEnumerator)
        {
            this.innerEnumerator = innerEnumerator;
        }

        public T Current => this.innerEnumerator.Current;

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(this.innerEnumerator.MoveNext());
        }

        public ValueTask DisposeAsync()
        {
            this.innerEnumerator.Dispose();
            return new ValueTask();
        }
    }

    public static class AsyncQueryableHelper
    {
        public static Mock<DbSet<T>> CreateDbSetMock<T>(IEnumerable<T> elements) where T : class
        {
            IQueryable<T> elementsAsQueryable = elements.AsQueryable();
            Mock<DbSet<T>> dbSetMock = new Mock<DbSet<T>>();

            TestAsyncEnumerable<T> asyncEnumerable = new TestAsyncEnumerable<T>(elements);
            dbSetMock.As<IAsyncEnumerable<T>>()
                .Setup(mockDbSet => mockDbSet.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<T>(elements.GetEnumerator()));

            dbSetMock.As<IQueryable<T>>().Setup(mockDbSet => mockDbSet.Provider).Returns(new TestAsyncQueryProvider<T>(elements.AsQueryable().Provider));
            dbSetMock.As<IQueryable<T>>().Setup(mockDbSet => mockDbSet.Expression).Returns(asyncEnumerable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(mockDbSet => mockDbSet.ElementType).Returns(elementsAsQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(mockDbSet => mockDbSet.GetEnumerator()).Returns(elements.GetEnumerator());

            return dbSetMock;
        }
    }
} 