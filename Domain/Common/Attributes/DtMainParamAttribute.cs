namespace Eshop.Domain.Common.Attributes;

/// <summary>
/// doesn't contain any property
/// </summary>
public class DtMainParamAttribute : Attribute
{
    // main params are query params that not included in the dynamic sql query
    // or in 'query' param.
}