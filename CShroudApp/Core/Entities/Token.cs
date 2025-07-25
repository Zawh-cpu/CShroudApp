﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace CShroudApp.Core.Entities;

public class Token
{
    public required string Data;
    public required DateTime Expiration;

    public static Token Parse(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);

        var expClaim = token.Claims.FirstOrDefault(c => c.Type == "exp");
        if (expClaim == null)
            return new Token() { Data = jwt, Expiration = DateTime.MaxValue };

        var exp = long.Parse(expClaim.Value);
        var expirationDate = DateTimeOffset.FromUnixTimeSeconds(exp).DateTime;

        return new Token() { Data = jwt, Expiration = expirationDate };
    }
}