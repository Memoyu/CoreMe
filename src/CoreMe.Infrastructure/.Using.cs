global using System.Linq;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Text;

global using Microsoft.Extensions.Configuration;

global using CoreMe.Application.Security;
global using CoreMe.Application.Common.Utils;
global using CoreMe.Application.Common.Interfaces.Security;
global using CoreMe.Application.Common.Models;
global using CoreMe.Application.Common.Request;
global using CoreMe.Application.Common.Interfaces.Persistence.Repositories;

global using CoreMe.Domain.Common;
global using CoreMe.Domain.Constants;
global using CoreMe.Domain.Enums;
global using CoreMe.Domain.Entities;

global using CoreMe.Infrastructure.Security.CurrentUserProvider;
global using CoreMe.Infrastructure.Security.GenerateToken;
global using CoreMe.Infrastructure.Persistence;
global using CoreMe.Infrastructure.Security.TokenValidation;
global using CoreMe.Infrastructure.Security;
global using CoreMe.Infrastructure.Persistence.Repositories;



