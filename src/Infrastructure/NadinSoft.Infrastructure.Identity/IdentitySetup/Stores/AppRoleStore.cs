using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NadinSoft.Domain.Entities.User;
using NadinSoft.Infrastructure.Persistence;

namespace NadinSoft.Infrastructure.Identity.IdentitySetup.Stores;

internal class AppRoleStore(NadinSoftDbContext context, IdentityErrorDescriber? describer = null)
    : RoleStore<RoleEntity, NadinSoftDbContext, Guid>(context, describer);