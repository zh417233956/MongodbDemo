using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionDemo
{
    class Program
    {
        static void Main1(string[] args)
        {
            Func<Users, bool> funcUser = Users => Users.Name == "zhh";
            Expression<Func<Users, bool>> expressionUser = users => users.Name == "zhh";
            var ex = new ExpressionTypeHelper_V1();
            var where = ex.ResolveExpression(expressionUser);
            Console.WriteLine(where);
            Console.ReadKey();
        }
        static void Main2(string[] args)
        {
            Expression<Func<Users, bool>> expressionUser = users => users.Name == "zhh" && users.Age >= 20;
            var ex = new ExpressionTypeHelper();
            ex.ResolveExpression(expressionUser);
            Console.WriteLine(ex.Where);
            Console.ReadKey();
        }
        static void Main(string[] args)
        {
            Expression<Func<Users, bool>> expressionUser = users => users.Name == "zhh" && users.Age >= 20;
            var ex = new ExpressionTrasfer();
            ex.ResolveExpression(expressionUser);
            Console.WriteLine(ex.Where);
            Console.ReadKey();
        }

    }
    #region 1.0 版本    
    public class ExpressionTypeHelper_V1
    {
        public string ResolveExpression(Expression<Func<Users, bool>> expression)
        {
            var bodyNode = (BinaryExpression)expression.Body;

            var leftNode = (MemberExpression)bodyNode.Left;

            var rightNode = (ConstantExpression)bodyNode.Right;

            return string.Format(" {0} {2} {1} ", leftNode.Member.Name, rightNode.Value, bodyNode.NodeType.TransferExpressionType());
        }
    }
    public static class ExpressionTypeExt
    {
        public static string TransferExpressionType(this ExpressionType expressionType)
        {
            string type = "";
            switch (expressionType)
            {
                case ExpressionType.Equal:
                    type = "="; break;
                case ExpressionType.GreaterThanOrEqual:
                    type = ">="; break;
                case ExpressionType.LessThanOrEqual:
                    type = "<="; break;
                case ExpressionType.NotEqual:
                    type = "!="; break;
                case ExpressionType.AndAlso:
                    type = "And"; break;
                case ExpressionType.OrElse:
                    type = "Or"; break;
            }
            return type;
        }
    }

    #endregion
    #region 2.0 升级版
    public class ExpressionTypeHelper
    {
        public StringBuilder GeWhere = new StringBuilder(100);

        public string Where
        {
            get { return GeWhere.ToString(); }
        }

        public void ResolveExpression(Expression<Func<Users, bool>> expression)
        {
            Visit(expression.Body);
        }

        public void Visit(Expression expression)
        {
            //NodeType 分别为 MemberAccess(从字段或属性进行读取的运算)、Constant(常量)。
            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                    VisitConstantExpression(expression);
                    break;
                case ExpressionType.MemberAccess:
                    VisitMemberExpression(expression);
                    break;
                case ExpressionType.Convert:
                    VisitUnaryExpression(expression);
                    break;
                default:
                    VisitBinaryExpression(expression);
                    break;
            }
        }
        public void VisitUnaryExpression(Expression expression)
        {
            var e = (UnaryExpression)expression;
            Visit(e.Operand);
        }
        public void VisitBinaryExpression(Expression expression)
        {
            var e = (BinaryExpression)expression;
            GeWhere.Append("(");
            Visit(e.Left);

            GeWhere.Append(e.NodeType.TransferExpressionType());

            Visit(e.Right);
            GeWhere.Append(")");
        }
        public void VisitConstantExpression(Expression expression)
        {
            var e = (ConstantExpression)expression;

            if (e.Type == typeof(string))
            {
                GeWhere.Append("'" + e.Value + "'");
            }
            else
            {
                GeWhere.Append(e.Value);
            }
        }
        public void VisitMemberExpression(Expression expression)
        {
            var e = (MemberExpression)expression;
            GeWhere.Append(e.Member.Name);
        }
    }
    #endregion

    #region 3.0版本
    public class ExpressionTrasfer : ExpressionVisitor
    {
        public StringBuilder GeWhere = new StringBuilder(100);

        public string Where
        {
            get { return GeWhere.ToString(); }
        }

        public void ResolveExpression(Expression<Func<Users, bool>> expression)
        {
            Visit(expression.Body);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            GeWhere.Append("(");
            Visit(node.Left);

            GeWhere.Append(node.NodeType.TransferExpressionType());

            Visit(node.Right);
            GeWhere.Append(")");

            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Type == typeof(string))
            {
                GeWhere.Append("'" + node.Value + "'");
            }
            else if (node.Type == typeof(int))
            {
                GeWhere.Append(node.Value);
            }
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            GeWhere.Append(node.Member.Name);
            return node;
        }
    }
    #endregion
}
