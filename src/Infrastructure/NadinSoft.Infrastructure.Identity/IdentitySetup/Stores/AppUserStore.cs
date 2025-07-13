using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NadinSoft.Domain.Entities.User;
using NadinSoft.Infrastructure.Persistence;

namespace NadinSoft.Infrastructure.Identity.IdentitySetup.Stores;

internal class AppUserStore(NadinSoftDbContext context, IdentityErrorDescriber? describer = null)
    : UserStore<UserEntity, RoleEntity, NadinSoftDbContext, Guid, UserClaimEntity, UserRoleEntity, UserLoginEntity,
        UserTokenEntity, RoleClaimEntity>(context, describer);