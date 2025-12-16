namespace AspRouting.CustomConstraints;

public class AdultConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (!values.ContainsKey("date")) return false;
        DateTime? date = Convert.ToDateTime(values["date"]);

        if (date is null) return false;

        DateTime today = DateTime.UtcNow;
        var minDate = today.AddYears(-18);
        return date <= minDate;
    }
}
