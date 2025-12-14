using System.Globalization;

namespace MyFirstApp;

public static class MathExtensions
{
    public static void UseMathUtility(this IApplicationBuilder app)
    {
        app.Use(async (context, next)=>
        {
            List<string> errors = [];
            double firstNumber = 0;
            double secondNumber = 0;
            Operation operation;

            IQueryCollection queries = context.Request.Query;
            if (queries.ContainsKey("firstNumber"))
            {
                firstNumber = TryParseQueryString(queries, nameof(firstNumber));
            }
            else
            {
                errors.Add($"Invalid input for '{nameof(firstNumber)}'");
            }

            if (queries.ContainsKey(nameof(secondNumber)))
            {
                secondNumber = TryParseQueryString(queries, nameof(secondNumber));
            }
            else
            {
                errors.Add($"Invalid input for '{nameof(secondNumber)}'");
            }

            if (queries.ContainsKey("operation"))
            {
                double result;
                operation = queries["operation"][0]!.ToOperation();
                if (operation == Operation.Invalid)
                {
                    errors.Add($"Invalid input for '{nameof(operation)}'");
                }
                else
                {
                    result = Calculate(operation, firstNumber, secondNumber);
                    await context.Response.WriteAsync(result.ToString());
                }
            }
            else
            {
                errors.Add($"Invalid input for '{nameof(operation)}'");
            }

            if (errors.Count > 0)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                errors.ForEach(async error => await context.Response.WriteAsync(error + "\n"));
            }
            await next(context);
        });
    }

    private enum Operation
    {
        Add,
        Div,
        Mul,
        Sub,
        Invalid
    }
    private static Operation ToOperation(this string input)
    {
        return input switch
        {
            "add" => Operation.Add,
            "multiply" => Operation.Mul,
            "div" => Operation.Div,
            "sub" => Operation.Sub,
            _ => Operation.Invalid
        };
    }
    private static double Calculate(Operation operation, double a, double b)
    {
        return operation switch
        {
            Operation.Add => a + b,
            Operation.Mul => a * b,
            Operation.Div => b.Equals(0) ? 0 : a / b,
            Operation.Sub => a - b,
            _ => 0
        };
    }
    private static double TryParseQueryString(IQueryCollection queries, string input) 
    {
        double.TryParse(queries[input][0], NumberFormatInfo.CurrentInfo, out double value);
        return value;
    }
}
