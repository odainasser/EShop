// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.


namespace Eshop.Domain.Entities;

/// <summary>
/// This holds what modules a user or tenant has
/// </summary>
/// <remarks>
/// This links modules to a user
/// </remarks>
/// <param name="userId"></param>
/// <param name="allowedPaidForModules"></param>
public class ModulesForUser(string userId, PaidForModules allowedPaidForModules) : BaseEntity
{
    [Key, MaxLength(ExtraAuthConstants.UserIdSize)]
    public string UserId { get; private set; } = userId ?? throw new ArgumentNullException(nameof(userId));

    public PaidForModules AllowedPaidForModules { get; private set; } = allowedPaidForModules;
}