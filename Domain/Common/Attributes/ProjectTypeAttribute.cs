namespace Eshop.Domain.Common.Attributes;

/// <summary>
/// no benefits. it only to show this class used for which projects
/// </summary>
/// <param name="projectType"></param>
[AttributeUsage(AttributeTargets.All)]
public class ProjectTypeAttribute(ProjectTypeEnum projectType) : Attribute
{
    public ProjectTypeEnum ProjectType => projectType;
}