namespace SheetDB.Linq
{
    using Implementation;
    using System.Linq.Expressions;

    public class CustomQueryProvider<T> : QueryProvider
    {
        private readonly ITable<T> table;

        public CustomQueryProvider(ITable<T> table)
        {
            this.table = table;
        }

        public override Query GetQuery(Expression expression)
        {
            expression = Evaluator.PartialEval(expression);
            return new QueryTranslator().Translate(expression);
        }

        public override object Execute(Expression expression)
        {
            return table.Find(GetQuery(expression));
        }
    }
}
