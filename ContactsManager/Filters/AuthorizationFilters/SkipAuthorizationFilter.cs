using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.Filters.AuthorizationFilters;

public class SkipAuthorizationFilter : Attribute, IFilterMetadata
{

}
