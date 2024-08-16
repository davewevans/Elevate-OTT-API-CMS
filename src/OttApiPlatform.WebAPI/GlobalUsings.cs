﻿global using OttApiPlatform.Application;
global using OttApiPlatform.Application.Common.Exceptions;
global using OttApiPlatform.Application.Common.Models;
global using OttApiPlatform.Application.Common.Models.ApplicationOptions.ApplicationIdentityOptions;
global using OttApiPlatform.Application.Features.Account.Commands.ConfirmEmail;
global using OttApiPlatform.Application.Features.Account.Commands.ForgotPassword;
global using OttApiPlatform.Application.Features.Account.Commands.Login;
global using OttApiPlatform.Application.Features.Account.Commands.LoginWith2fa;
global using OttApiPlatform.Application.Features.Account.Commands.LoginWithRecoveryCode;
global using OttApiPlatform.Application.Features.Account.Commands.RefreshToken;
global using OttApiPlatform.Application.Features.Account.Commands.Register;
global using OttApiPlatform.Application.Features.Account.Commands.ResendEmailConfirmation;
global using OttApiPlatform.Application.Features.Account.Commands.ResetPassword;
global using OttApiPlatform.Application.Features.Account.Manage.Commands.ChangeEmail;
global using OttApiPlatform.Application.Features.Account.Manage.Commands.ChangePassword;
global using OttApiPlatform.Application.Features.Account.Manage.Commands.ConfirmEmailChange;
global using OttApiPlatform.Application.Features.Account.Manage.Commands.DeletePersonalData;
global using OttApiPlatform.Application.Features.Account.Manage.Commands.Disable2fa;
global using OttApiPlatform.Application.Features.Account.Manage.Commands.EnableAuthenticator;
global using OttApiPlatform.Application.Features.Account.Manage.Commands.ResetAuthenticator;
global using OttApiPlatform.Application.Features.Account.Manage.Commands.SetPassword;
global using OttApiPlatform.Application.Features.Account.Manage.Commands.UpdateUserAvatar;
global using OttApiPlatform.Application.Features.Account.Manage.Commands.UpdateUserProfile;
global using OttApiPlatform.Application.Features.Account.Manage.Queries.CheckUser2faState;
global using OttApiPlatform.Application.Features.Account.Manage.Queries.DownloadPersonalData;
global using OttApiPlatform.Application.Features.Account.Manage.Queries.GenerateRecoveryCodes;
global using OttApiPlatform.Application.Features.Account.Manage.Queries.Get2faState;
global using OttApiPlatform.Application.Features.Account.Manage.Queries.GetUser;
global using OttApiPlatform.Application.Features.Account.Manage.Queries.GetUserAvatar;
global using OttApiPlatform.Application.Features.Account.Manage.Queries.HasPassword;
global using OttApiPlatform.Application.Features.Account.Manage.Queries.LoadSharedKeyAndQrCodeUri;
global using OttApiPlatform.Application.Features.AppSettings.Commands.UpdateSettings.UpdateFileStorageSettings;
global using OttApiPlatform.Application.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings;
global using OttApiPlatform.Application.Features.AppSettings.Commands.UpdateSettings.UpdateTokenSettings;
global using OttApiPlatform.Application.Features.AppSettings.Queries.GetSettings.GetFileStorageSettings;
global using OttApiPlatform.Application.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;
global using OttApiPlatform.Application.Features.AppSettings.Queries.GetSettings.GetTokenSettings;
global using OttApiPlatform.Application.Features.Dashboard.Queries.GetHeadlines;
global using OttApiPlatform.Application.Features.FileUpload;
global using OttApiPlatform.Application.Features.Identity.Permissions.Queries.GetPermissions;
global using OttApiPlatform.Application.Features.Identity.Roles.Commands.CreateRole;
global using OttApiPlatform.Application.Features.Identity.Roles.Commands.DeleteRole;
global using OttApiPlatform.Application.Features.Identity.Roles.Commands.UpdateRole;
global using OttApiPlatform.Application.Features.Identity.Roles.Queries.GetRoleForEdit;
global using OttApiPlatform.Application.Features.Identity.Roles.Queries.GetRoles;
global using OttApiPlatform.Application.Features.Identity.Users.Commands.CreateUser;
global using OttApiPlatform.Application.Features.Identity.Users.Commands.DeleteUser;
global using OttApiPlatform.Application.Features.Identity.Users.Commands.GrantOrRevokeUserPermissions;
global using OttApiPlatform.Application.Features.Identity.Users.Commands.UpdateUser;
global using OttApiPlatform.Application.Features.Identity.Users.Queries.GetUserForEdit;
global using OttApiPlatform.Application.Features.Identity.Users.Queries.GetUserPermissions;
global using OttApiPlatform.Application.Features.Identity.Users.Queries.GetUsers;
global using OttApiPlatform.Application.Features.MyTenant.Commands.DeleteMyTenant;
global using OttApiPlatform.Application.Features.MyTenant.Commands.UpdateMyTenant;
global using OttApiPlatform.Application.Features.MyTenant.Queries.GetMyTenant;
global using OttApiPlatform.Application.Features.POC.Applicants.Commands.CreateApplicant;
global using OttApiPlatform.Application.Features.POC.Applicants.Commands.DeleteApplicant;
global using OttApiPlatform.Application.Features.POC.Applicants.Commands.UpdateApplicant;
global using OttApiPlatform.Application.Features.POC.Applicants.Queries.ExportApplicants;
global using OttApiPlatform.Application.Features.POC.Applicants.Queries.GetApplicantForEdit;
global using OttApiPlatform.Application.Features.POC.Applicants.Queries.GetApplicants;
global using OttApiPlatform.Application.Features.POC.Applicants.Queries.GetApplicantsReferences;
global using OttApiPlatform.Application.Features.Reports.GetReportForEdit;
global using OttApiPlatform.Application.Features.Reports.GetReports;
global using OttApiPlatform.Application.Features.Tenants.Commands.CreateTenant;
global using OttApiPlatform.Application.Features.Tenants.Commands.DeleteTenant;
global using OttApiPlatform.Application.Features.Tenants.Commands.UpdateTenant;
global using OttApiPlatform.Application.Features.Tenants.Queries.GetTenantForEdit;
global using OttApiPlatform.Application.Features.Tenants.Queries.GetTenants;
global using OttApiPlatform.AppResources;
global using OttApiPlatform.Domain.Enums;
global using OttApiPlatform.Infrastructure;
global using OttApiPlatform.Infrastructure.Extensions;
global using OttApiPlatform.Infrastructure.Persistence;
global using OttApiPlatform.WebAPI.Exceptions;
global using OttApiPlatform.WebAPI.Extensions;
global using OttApiPlatform.WebAPI.Filters;
global using OttApiPlatform.WebAPI.Hubs;
global using OttApiPlatform.WebAPI.Managers;
global using OttApiPlatform.WebAPI.Middleware;
global using OttApiPlatform.WebAPI.Models;
global using OttApiPlatform.WebAPI.Services.HubServices;
global using FluentValidation;
global using FluentValidation.AspNetCore;
global using Hangfire;
global using MediatR;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Http.Extensions;
global using Microsoft.AspNetCore.Http.Features;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.OpenApi.Models;
global using Nancy.Security;
global using Swashbuckle.AspNetCore.SwaggerGen;
global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Net;
global using System.Reflection;
global using System.Security.Claims;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Threading;
global using System.Threading.Tasks;
global using OttApiPlatform.Application.Common.Contracts.Infrastructure;
global using OttApiPlatform.Application.Common.Contracts.Infrastructure.Persistence;
global using OttApiPlatform.Application.Common.Contracts.Infrastructure.Reporting;
global using OttApiPlatform.Application.Common.Contracts.WebAPI.Hub;
global using OttApiPlatform.Application.Features.Identity.Roles.Queries.GetRolePermissions;