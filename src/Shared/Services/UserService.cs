using LanguageExt.Common;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services;

public interface IUserService
{
    public bool TryGetUserId(out Guid? id);
}

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _accessor;
    private Claim[]? _claims;

    public UserService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public bool TryGetUserId(out Guid? id)
    {
        id = null;
        var userId = GetUserClaims().FirstOrDefault(claim => claim.Type == "oid")?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return false;
        }

        if (!Guid.TryParse(userId, out Guid parsedGuid))
        {
            return false;
        }

        id = parsedGuid;

        return true;
    }


    private IEnumerable<Claim> GetUserClaims()
        => _claims ??= _accessor.HttpContext.User.Claims.ToArray();
}

public class DevUserService : IUserService
{
    public bool TryGetUserId(out Guid? id)
    {
        id = Guid.NewGuid();

        return true;
    }
}
