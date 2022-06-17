using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

public static class JWTHelper
{
    public static TokenValidationParameters TokenValidationParamegersGenerate(string key)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var encryptionkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key[..16]));

        var validations = new TokenValidationParameters
        {
            RequireSignedTokens = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,

            RequireExpirationTime = true,
            ValidateLifetime = true,

            ValidateAudience = false, //default : false
           //ValidAudience = jwtSettings.Audience,

            ValidateIssuer = false, //default : false
            //ValidIssuer = jwtSettings.Issuer,

            TokenDecryptionKey = encryptionkey
        };

        return validations;
    }

    public static List<Claim> TokenClaimsGet(string token, string key)
    { 
        var handler = new JwtSecurityTokenHandler();
        var data = handler.ValidateToken(token, TokenValidationParamegersGenerate(key), out var tokenSecure);
        var toReturn = data.Claims.Select(item => new Claim(item.Type, item.Value)).ToList();

        return toReturn;
    }
}