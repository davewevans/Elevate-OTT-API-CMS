﻿global using OttApiPlatform.AppResources;
global using OttApiPlatform.CMS;
global using OttApiPlatform.CMS.Consumers;
global using OttApiPlatform.CMS.Enums;
global using OttApiPlatform.CMS.Extensions;
global using OttApiPlatform.CMS.Features.AppSettings.Commands.UpdateSettings.UpdateFileStorageSettings;
global using OttApiPlatform.CMS.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings;
global using OttApiPlatform.CMS.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings.IdentitySettingsCommand;
global using OttApiPlatform.CMS.Features.AppSettings.Commands.UpdateSettings.UpdateTokenSettings;
global using OttApiPlatform.CMS.Features.AppSettings.Queries.GetSettings.GetFileStorageSettings;
global using OttApiPlatform.CMS.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;
global using OttApiPlatform.CMS.Features.AppSettings.Queries.GetSettings.GetTokenSettings;
global using OttApiPlatform.CMS.Features.Dashboard.Queries.GetHeadlines;
global using OttApiPlatform.CMS.Features.FileUpload;
global using OttApiPlatform.CMS.Features.Identity.Account.Commands.ConfirmEmail;
global using OttApiPlatform.CMS.Features.Identity.Account.Commands.ForgotPassword;
global using OttApiPlatform.CMS.Features.Identity.Account.Commands.Login;
global using OttApiPlatform.CMS.Features.Identity.Account.Commands.LoginWith2fa;
global using OttApiPlatform.CMS.Features.Identity.Account.Commands.LoginWithRecoveryCode;
global using OttApiPlatform.CMS.Features.Identity.Account.Commands.RefreshToken;
global using OttApiPlatform.CMS.Features.Identity.Account.Commands.Register;
global using OttApiPlatform.CMS.Features.Identity.Account.Commands.ResendEmailConfirmation;
global using OttApiPlatform.CMS.Features.Identity.Account.Commands.ResetPassword;
global using OttApiPlatform.CMS.Features.Identity.Manage.Commands.ChangeEmail;
global using OttApiPlatform.CMS.Features.Identity.Manage.Commands.ChangePassword;
global using OttApiPlatform.CMS.Features.Identity.Manage.Commands.ConfirmEmailChange;
global using OttApiPlatform.CMS.Features.Identity.Manage.Commands.DeletePersonalData;
global using OttApiPlatform.CMS.Features.Identity.Manage.Commands.EnableAuthenticator;
global using OttApiPlatform.CMS.Features.Identity.Manage.Commands.ResetAuthenticator;
global using OttApiPlatform.CMS.Features.Identity.Manage.Commands.UpdateUserAvatar;
global using OttApiPlatform.CMS.Features.Identity.Manage.Commands.UpdateUserProfile;
global using OttApiPlatform.CMS.Features.Identity.Manage.Queries.CheckUser2faState;
global using OttApiPlatform.CMS.Features.Identity.Manage.Queries.DownloadPersonalData;
global using OttApiPlatform.CMS.Features.Identity.Manage.Queries.GenerateRecoveryCodes;
global using OttApiPlatform.CMS.Features.Identity.Manage.Queries.Get2faState;
global using OttApiPlatform.CMS.Features.Identity.Manage.Queries.GetUser;
global using OttApiPlatform.CMS.Features.Identity.Manage.Queries.GetUserAvatar;
global using OttApiPlatform.CMS.Features.Identity.Manage.Queries.LoadSharedKeyAndQrCodeUri;
global using OttApiPlatform.CMS.Features.Identity.Permissions.Queries.GetPermissions;
global using OttApiPlatform.CMS.Features.Identity.Roles.Commands.CreateRole;
global using OttApiPlatform.CMS.Features.Identity.Roles.Commands.UpdateRole;
global using OttApiPlatform.CMS.Features.Identity.Roles.Queries.GetRoleForEdit;
global using OttApiPlatform.CMS.Features.Identity.Roles.Queries.GetRoles;
global using OttApiPlatform.CMS.Features.Identity.Users.Commands.CreateUser;
global using OttApiPlatform.CMS.Features.Identity.Users.Commands.GrantOrRevokeUserPermissions;
global using OttApiPlatform.CMS.Features.Identity.Users.Commands.UpdateUser;
global using OttApiPlatform.CMS.Features.Identity.Users.Queries.GetUserForEdit;
global using OttApiPlatform.CMS.Features.Identity.Users.Queries.GetUserPermissions;
global using OttApiPlatform.CMS.Features.Identity.Users.Queries.GetUsers;
global using OttApiPlatform.CMS.Features.MyTenant.Commands.UpdateTenant;
global using OttApiPlatform.CMS.Features.MyTenant.Queries.GetTenantForEdit;
global using OttApiPlatform.CMS.Features.POC.Applicants.Commands.CreateApplicant;
global using OttApiPlatform.CMS.Features.POC.Applicants.Commands.UpdateApplicant;
global using OttApiPlatform.CMS.Features.POC.Applicants.Queries.ExportApplicants;
global using OttApiPlatform.CMS.Features.POC.Applicants.Queries.GetApplicantForEdit;
global using OttApiPlatform.CMS.Features.POC.Applicants.Queries.GetApplicants;
global using OttApiPlatform.CMS.Features.POC.Applicants.Queries.GetApplicantsReferences;
global using OttApiPlatform.CMS.Features.Reports.GetReportForEdit;
global using OttApiPlatform.CMS.Features.Reports.GetReports;
global using OttApiPlatform.CMS.Features.Tenants.Commands.CreateTenant;
global using OttApiPlatform.CMS.Features.Tenants.Commands.UpdateTenant;
global using OttApiPlatform.CMS.Features.Tenants.Queries.GetTenantForEdit;
global using OttApiPlatform.CMS.Features.Tenants.Queries.GetTenants;
global using OttApiPlatform.CMS.Helpers;
global using OttApiPlatform.CMS.Models;
global using OttApiPlatform.CMS.Models.Dashboard;
global using OttApiPlatform.CMS.Models.Reporting;
global using OttApiPlatform.CMS.Pages.Identity.Roles;
global using OttApiPlatform.CMS.Providers;
global using OttApiPlatform.CMS.Services;
global using OttApiPlatform.CMS.Shared.Dialog;
global using OttApiPlatform.CMS.Shared.FileUpload;
global using OttApiPlatform.CMS.Theme;
global using Blazored.LocalStorage;
global using FluentValidation;
global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Components.Authorization;
global using Microsoft.AspNetCore.Components.Forms;
global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.SignalR.Client;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Localization;
global using Microsoft.JSInterop;
global using MudBlazor;
global using MudBlazor.Services;
global using System;
global using System.Collections.Generic;
global using System.Globalization;
global using System.IO;
global using System.Linq;
global using System.Net;
global using System.Net.Http;
global using System.Net.Http.Headers;
global using System.Net.Http.Json;
global using System.Reflection;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Json;
global using System.Threading;
global using System.Threading.Tasks;
global using OttApiPlatform.CMS.Contracts.Consumers;
global using OttApiPlatform.CMS.Contracts.Helpers;
global using OttApiPlatform.CMS.Contracts.Providers;
global using OttApiPlatform.CMS.Contracts.Services;
global using Microsoft.AspNetCore.WebUtilities;
global using Severity = MudBlazor.Severity;