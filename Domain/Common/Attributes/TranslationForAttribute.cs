namespace Eshop.Domain.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TranslationForAttribute(Type entityType) : Attribute
{
    public Type EntityType => entityType;
}
