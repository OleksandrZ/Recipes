using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Domain;

namespace Recipes.Infrastructure.Interfaces
{
    public interface IRefreshTokenGenerator
    {
        public RefreshToken GenerateRefreshToken(string ipAddress);
    }
}
