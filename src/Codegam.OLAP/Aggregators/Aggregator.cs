using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Codegam.OLAP.Aggregators
{
    public abstract class Aggregator<T> : IAggregator
    {        
        protected Func<IOlapDataVector, object> ValueSelector { get; private set; }
        protected Func<IOlapDataVector, bool> AggregatePred { get; private set; }

        protected Aggregator(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred)
        {            
            ValueSelector = valueSelector;
            AggregatePred = aggregatePred ?? (v => true);
            ValueT = default(T);
        }

        protected V GetDataVectorValue<V>(IOlapDataVector dataVector)
        {
            if (ValueSelector == null)
                throw new ArgumentNullException("Value selector should be specified for aggregator of type:" + GetType());
            return Cast<V>(ValueSelector(dataVector));
        }

        protected V Cast<V>(object value)
        {
            return (V)Convert.ChangeType(value, typeof(V));
        }

        protected T ValueT { get; set; }

        public object Value
        {
            get { return ValueT; }
        }

        public IAggregator CleanClone()
        {
            return CleanCloneT();
        }

        protected abstract Aggregator<T> CleanCloneT();
        
        protected abstract void AggregateValue(IOlapDataVector dataVector);

        public void Aggregate(IOlapDataVector dataVector)
        {
            if (AggregatePred(dataVector))
                AggregateValue(dataVector);
        }

        public static U Add<U>(U a, U b)
        {
            ParameterExpression paramA = Expression.Parameter(typeof(U), "a");
            ParameterExpression paramB = Expression.Parameter(typeof(U), "b");
            BinaryExpression body = Expression.Add(paramA, paramB);
            Func<U, U, U> add = Expression.Lambda<Func<U, U, U>>(body, paramA, paramB).Compile();
            return add(a, b);
        }

        public static U Subtract<U>(U a, U b)
        {
            ParameterExpression paramA = Expression.Parameter(typeof(U), "a");
            ParameterExpression paramB = Expression.Parameter(typeof(U), "b");
            BinaryExpression body = Expression.Subtract(paramA, paramB);
            Func<U, U, U> subtract = Expression.Lambda<Func<U, U, U>>(body, paramA, paramB).Compile();
            return subtract(a, b);
        }

        public static U Multiply<U>(U a, U b)
        {
            ParameterExpression paramA = Expression.Parameter(typeof(U), "a");
            ParameterExpression paramB = Expression.Parameter(typeof(U), "b");
            BinaryExpression body = Expression.Multiply(paramA, paramB);
            Func<U, U, U> multiply = Expression.Lambda<Func<U, U, U>>(body, paramA, paramB).Compile();
            return multiply(a, b);
        }

        public static U Divide<U>(U a, U b)
        {
            ParameterExpression paramA = Expression.Parameter(typeof(U), "a");
            ParameterExpression paramB = Expression.Parameter(typeof(U), "b");
            BinaryExpression body = Expression.Divide(paramA, paramB);
            Func<U, U, U> divide = Expression.Lambda<Func<U, U, U>>(body, paramA, paramB).Compile();
            return divide(a, b);
        }

        public static bool Less<U>(U a, U b)
        {
            ParameterExpression paramA = Expression.Parameter(typeof(U), "a");
            ParameterExpression paramB = Expression.Parameter(typeof(U), "b");
            BinaryExpression body = Expression.LessThan(paramA, paramB);
            Func<U, U, bool> less = Expression.Lambda<Func<U, U, bool>>(body, paramA, paramB).Compile();
            return less(a, b);
        }

        public static bool Greater<U>(U a, U b)
        {
            ParameterExpression paramA = Expression.Parameter(typeof(U), "a");
            ParameterExpression paramB = Expression.Parameter(typeof(U), "b");
            BinaryExpression body = Expression.GreaterThan(paramA, paramB);
            Func<U, U, bool> less = Expression.Lambda<Func<U, U, bool>>(body, paramA, paramB).Compile();
            return less(a, b);
        }
    }
}
