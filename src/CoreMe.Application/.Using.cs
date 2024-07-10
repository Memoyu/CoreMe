global using System.Linq;

global using FluentValidation;
global using MediatR;
global using MapsterMapper;
global using Mapster;

global using CoreMe.Application.Common.Interfaces.Security;
global using CoreMe.Application.Common.Models;
global using CoreMe.Application.Common.Behaviours;
global using CoreMe.Domain.Constants;
global using CoreMe.Application.Common.Interfaces.Persistence.Repositories;
global using CoreMe.Application.Tokens.Common;
global using CoreMe.Domain.Entities;
global using CoreMe.Application.Common.Request;
global using CoreMe.Application.Common.Utils;
global using CoreMe.Domain.Enums;
global using CoreMe.Application.Common.Extensions;

global using ApiPermission = CoreMe.Domain.Constants.Security.Permissions.Permissions;
