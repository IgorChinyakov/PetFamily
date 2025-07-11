﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Accounts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removenamingconventions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_admin_accounts_users_user_id",
                schema: "accounts",
                table: "admin_accounts");

            migrationBuilder.DropForeignKey(
                name: "fk_participant_accounts_users_user_id",
                schema: "accounts",
                table: "participant_accounts");

            migrationBuilder.DropForeignKey(
                name: "fk_refresh_sessions_users_user_id",
                schema: "accounts",
                table: "refresh_sessions");

            migrationBuilder.DropForeignKey(
                name: "fk_role_claims_roles_role_id",
                schema: "accounts",
                table: "role_claims");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permissions_permissions_permission_id",
                schema: "accounts",
                table: "role_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permissions_roles_role_id",
                schema: "accounts",
                table: "role_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_user_claims_users_user_id",
                schema: "accounts",
                table: "user_claims");

            migrationBuilder.DropForeignKey(
                name: "fk_user_logins_users_user_id",
                schema: "accounts",
                table: "user_logins");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_roles_role_id",
                schema: "accounts",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_users_user_id",
                schema: "accounts",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_user_tokens_users_user_id",
                schema: "accounts",
                table: "user_tokens");

            migrationBuilder.DropForeignKey(
                name: "fk_volunteer_accounts_users_user_id",
                schema: "accounts",
                table: "volunteer_accounts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_volunteer_accounts",
                schema: "accounts",
                table: "volunteer_accounts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                schema: "accounts",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_tokens",
                schema: "accounts",
                table: "user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_roles",
                schema: "accounts",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_logins",
                schema: "accounts",
                table: "user_logins");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_claims",
                schema: "accounts",
                table: "user_claims");

            migrationBuilder.DropPrimaryKey(
                name: "pk_roles",
                schema: "accounts",
                table: "roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_role_permissions",
                schema: "accounts",
                table: "role_permissions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_role_claims",
                schema: "accounts",
                table: "role_claims");

            migrationBuilder.DropPrimaryKey(
                name: "pk_refresh_sessions",
                schema: "accounts",
                table: "refresh_sessions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_permissions",
                schema: "accounts",
                table: "permissions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_participant_accounts",
                schema: "accounts",
                table: "participant_accounts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_admin_accounts",
                schema: "accounts",
                table: "admin_accounts");

            migrationBuilder.RenameColumn(
                name: "experience",
                schema: "accounts",
                table: "volunteer_accounts",
                newName: "Experience");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "accounts",
                table: "volunteer_accounts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "accounts",
                table: "volunteer_accounts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "ix_volunteer_accounts_user_id",
                schema: "accounts",
                table: "volunteer_accounts",
                newName: "IX_volunteer_accounts_UserId");

            migrationBuilder.RenameColumn(
                name: "email",
                schema: "accounts",
                table: "users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "accounts",
                table: "users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_name",
                schema: "accounts",
                table: "users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "two_factor_enabled",
                schema: "accounts",
                table: "users",
                newName: "TwoFactorEnabled");

            migrationBuilder.RenameColumn(
                name: "security_stamp",
                schema: "accounts",
                table: "users",
                newName: "SecurityStamp");

            migrationBuilder.RenameColumn(
                name: "phone_number_confirmed",
                schema: "accounts",
                table: "users",
                newName: "PhoneNumberConfirmed");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                schema: "accounts",
                table: "users",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "path_to_photo",
                schema: "accounts",
                table: "users",
                newName: "PathToPhoto");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                schema: "accounts",
                table: "users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "normalized_user_name",
                schema: "accounts",
                table: "users",
                newName: "NormalizedUserName");

            migrationBuilder.RenameColumn(
                name: "normalized_email",
                schema: "accounts",
                table: "users",
                newName: "NormalizedEmail");

            migrationBuilder.RenameColumn(
                name: "lockout_end",
                schema: "accounts",
                table: "users",
                newName: "LockoutEnd");

            migrationBuilder.RenameColumn(
                name: "lockout_enabled",
                schema: "accounts",
                table: "users",
                newName: "LockoutEnabled");

            migrationBuilder.RenameColumn(
                name: "email_confirmed",
                schema: "accounts",
                table: "users",
                newName: "EmailConfirmed");

            migrationBuilder.RenameColumn(
                name: "concurrency_stamp",
                schema: "accounts",
                table: "users",
                newName: "ConcurrencyStamp");

            migrationBuilder.RenameColumn(
                name: "access_failed_count",
                schema: "accounts",
                table: "users",
                newName: "AccessFailedCount");

            migrationBuilder.RenameColumn(
                name: "value",
                schema: "accounts",
                table: "user_tokens",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "accounts",
                table: "user_tokens",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "login_provider",
                schema: "accounts",
                table: "user_tokens",
                newName: "LoginProvider");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "accounts",
                table: "user_tokens",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "role_id",
                schema: "accounts",
                table: "user_roles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "accounts",
                table: "user_roles",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "ix_user_roles_role_id",
                schema: "accounts",
                table: "user_roles",
                newName: "IX_user_roles_RoleId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "accounts",
                table: "user_logins",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "provider_display_name",
                schema: "accounts",
                table: "user_logins",
                newName: "ProviderDisplayName");

            migrationBuilder.RenameColumn(
                name: "provider_key",
                schema: "accounts",
                table: "user_logins",
                newName: "ProviderKey");

            migrationBuilder.RenameColumn(
                name: "login_provider",
                schema: "accounts",
                table: "user_logins",
                newName: "LoginProvider");

            migrationBuilder.RenameIndex(
                name: "ix_user_logins_user_id",
                schema: "accounts",
                table: "user_logins",
                newName: "IX_user_logins_UserId");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "accounts",
                table: "user_claims",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "accounts",
                table: "user_claims",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "claim_value",
                schema: "accounts",
                table: "user_claims",
                newName: "ClaimValue");

            migrationBuilder.RenameColumn(
                name: "claim_type",
                schema: "accounts",
                table: "user_claims",
                newName: "ClaimType");

            migrationBuilder.RenameIndex(
                name: "ix_user_claims_user_id",
                schema: "accounts",
                table: "user_claims",
                newName: "IX_user_claims_UserId");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "accounts",
                table: "roles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "accounts",
                table: "roles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "normalized_name",
                schema: "accounts",
                table: "roles",
                newName: "NormalizedName");

            migrationBuilder.RenameColumn(
                name: "concurrency_stamp",
                schema: "accounts",
                table: "roles",
                newName: "ConcurrencyStamp");

            migrationBuilder.RenameColumn(
                name: "role_id",
                schema: "accounts",
                table: "role_permissions",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "permission_id",
                schema: "accounts",
                table: "role_permissions",
                newName: "PermissionId");

            migrationBuilder.RenameIndex(
                name: "ix_role_permissions_role_id",
                schema: "accounts",
                table: "role_permissions",
                newName: "IX_role_permissions_RoleId");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "accounts",
                table: "role_claims",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "role_id",
                schema: "accounts",
                table: "role_claims",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "claim_value",
                schema: "accounts",
                table: "role_claims",
                newName: "ClaimValue");

            migrationBuilder.RenameColumn(
                name: "claim_type",
                schema: "accounts",
                table: "role_claims",
                newName: "ClaimType");

            migrationBuilder.RenameIndex(
                name: "ix_role_claims_role_id",
                schema: "accounts",
                table: "role_claims",
                newName: "IX_role_claims_RoleId");

            migrationBuilder.RenameColumn(
                name: "jti",
                schema: "accounts",
                table: "refresh_sessions",
                newName: "Jti");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "accounts",
                table: "refresh_sessions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "accounts",
                table: "refresh_sessions",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "refresh_token",
                schema: "accounts",
                table: "refresh_sessions",
                newName: "RefreshToken");

            migrationBuilder.RenameColumn(
                name: "expires_in",
                schema: "accounts",
                table: "refresh_sessions",
                newName: "ExpiresIn");

            migrationBuilder.RenameColumn(
                name: "created_at",
                schema: "accounts",
                table: "refresh_sessions",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_refresh_sessions_user_id",
                schema: "accounts",
                table: "refresh_sessions",
                newName: "IX_refresh_sessions_UserId");

            migrationBuilder.RenameColumn(
                name: "code",
                schema: "accounts",
                table: "permissions",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "accounts",
                table: "permissions",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "ix_permissions_code",
                schema: "accounts",
                table: "permissions",
                newName: "IX_permissions_Code");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "accounts",
                table: "participant_accounts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "accounts",
                table: "participant_accounts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "ix_participant_accounts_user_id",
                schema: "accounts",
                table: "participant_accounts",
                newName: "IX_participant_accounts_UserId");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "accounts",
                table: "admin_accounts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "accounts",
                table: "admin_accounts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "ix_admin_accounts_user_id",
                schema: "accounts",
                table: "admin_accounts",
                newName: "IX_admin_accounts_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_volunteer_accounts",
                schema: "accounts",
                table: "volunteer_accounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                schema: "accounts",
                table: "users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_tokens",
                schema: "accounts",
                table: "user_tokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_roles",
                schema: "accounts",
                table: "user_roles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_logins",
                schema: "accounts",
                table: "user_logins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_claims",
                schema: "accounts",
                table: "user_claims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_roles",
                schema: "accounts",
                table: "roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_permissions",
                schema: "accounts",
                table: "role_permissions",
                columns: new[] { "PermissionId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_claims",
                schema: "accounts",
                table: "role_claims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_refresh_sessions",
                schema: "accounts",
                table: "refresh_sessions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_permissions",
                schema: "accounts",
                table: "permissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_participant_accounts",
                schema: "accounts",
                table: "participant_accounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_admin_accounts",
                schema: "accounts",
                table: "admin_accounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_admin_accounts_users_UserId",
                schema: "accounts",
                table: "admin_accounts",
                column: "UserId",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_participant_accounts_users_UserId",
                schema: "accounts",
                table: "participant_accounts",
                column: "UserId",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_sessions_users_UserId",
                schema: "accounts",
                table: "refresh_sessions",
                column: "UserId",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_role_claims_roles_RoleId",
                schema: "accounts",
                table: "role_claims",
                column: "RoleId",
                principalSchema: "accounts",
                principalTable: "roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permissions_permissions_PermissionId",
                schema: "accounts",
                table: "role_permissions",
                column: "PermissionId",
                principalSchema: "accounts",
                principalTable: "permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permissions_roles_RoleId",
                schema: "accounts",
                table: "role_permissions",
                column: "RoleId",
                principalSchema: "accounts",
                principalTable: "roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_claims_users_UserId",
                schema: "accounts",
                table: "user_claims",
                column: "UserId",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_logins_users_UserId",
                schema: "accounts",
                table: "user_logins",
                column: "UserId",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_roles_RoleId",
                schema: "accounts",
                table: "user_roles",
                column: "RoleId",
                principalSchema: "accounts",
                principalTable: "roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_users_UserId",
                schema: "accounts",
                table: "user_roles",
                column: "UserId",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_tokens_users_UserId",
                schema: "accounts",
                table: "user_tokens",
                column: "UserId",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_volunteer_accounts_users_UserId",
                schema: "accounts",
                table: "volunteer_accounts",
                column: "UserId",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_admin_accounts_users_UserId",
                schema: "accounts",
                table: "admin_accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_participant_accounts_users_UserId",
                schema: "accounts",
                table: "participant_accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_refresh_sessions_users_UserId",
                schema: "accounts",
                table: "refresh_sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_role_claims_roles_RoleId",
                schema: "accounts",
                table: "role_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permissions_permissions_PermissionId",
                schema: "accounts",
                table: "role_permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permissions_roles_RoleId",
                schema: "accounts",
                table: "role_permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_user_claims_users_UserId",
                schema: "accounts",
                table: "user_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_user_logins_users_UserId",
                schema: "accounts",
                table: "user_logins");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_roles_RoleId",
                schema: "accounts",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_users_UserId",
                schema: "accounts",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_tokens_users_UserId",
                schema: "accounts",
                table: "user_tokens");

            migrationBuilder.DropForeignKey(
                name: "FK_volunteer_accounts_users_UserId",
                schema: "accounts",
                table: "volunteer_accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_volunteer_accounts",
                schema: "accounts",
                table: "volunteer_accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                schema: "accounts",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_tokens",
                schema: "accounts",
                table: "user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_roles",
                schema: "accounts",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_logins",
                schema: "accounts",
                table: "user_logins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_claims",
                schema: "accounts",
                table: "user_claims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_roles",
                schema: "accounts",
                table: "roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role_permissions",
                schema: "accounts",
                table: "role_permissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role_claims",
                schema: "accounts",
                table: "role_claims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_refresh_sessions",
                schema: "accounts",
                table: "refresh_sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_permissions",
                schema: "accounts",
                table: "permissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_participant_accounts",
                schema: "accounts",
                table: "participant_accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_admin_accounts",
                schema: "accounts",
                table: "admin_accounts");

            migrationBuilder.RenameColumn(
                name: "Experience",
                schema: "accounts",
                table: "volunteer_accounts",
                newName: "experience");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "accounts",
                table: "volunteer_accounts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "accounts",
                table: "volunteer_accounts",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_volunteer_accounts_UserId",
                schema: "accounts",
                table: "volunteer_accounts",
                newName: "ix_volunteer_accounts_user_id");

            migrationBuilder.RenameColumn(
                name: "Email",
                schema: "accounts",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "accounts",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserName",
                schema: "accounts",
                table: "users",
                newName: "user_name");

            migrationBuilder.RenameColumn(
                name: "TwoFactorEnabled",
                schema: "accounts",
                table: "users",
                newName: "two_factor_enabled");

            migrationBuilder.RenameColumn(
                name: "SecurityStamp",
                schema: "accounts",
                table: "users",
                newName: "security_stamp");

            migrationBuilder.RenameColumn(
                name: "PhoneNumberConfirmed",
                schema: "accounts",
                table: "users",
                newName: "phone_number_confirmed");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                schema: "accounts",
                table: "users",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "PathToPhoto",
                schema: "accounts",
                table: "users",
                newName: "path_to_photo");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                schema: "accounts",
                table: "users",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "NormalizedUserName",
                schema: "accounts",
                table: "users",
                newName: "normalized_user_name");

            migrationBuilder.RenameColumn(
                name: "NormalizedEmail",
                schema: "accounts",
                table: "users",
                newName: "normalized_email");

            migrationBuilder.RenameColumn(
                name: "LockoutEnd",
                schema: "accounts",
                table: "users",
                newName: "lockout_end");

            migrationBuilder.RenameColumn(
                name: "LockoutEnabled",
                schema: "accounts",
                table: "users",
                newName: "lockout_enabled");

            migrationBuilder.RenameColumn(
                name: "EmailConfirmed",
                schema: "accounts",
                table: "users",
                newName: "email_confirmed");

            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                schema: "accounts",
                table: "users",
                newName: "concurrency_stamp");

            migrationBuilder.RenameColumn(
                name: "AccessFailedCount",
                schema: "accounts",
                table: "users",
                newName: "access_failed_count");

            migrationBuilder.RenameColumn(
                name: "Value",
                schema: "accounts",
                table: "user_tokens",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "accounts",
                table: "user_tokens",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "LoginProvider",
                schema: "accounts",
                table: "user_tokens",
                newName: "login_provider");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "accounts",
                table: "user_tokens",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                schema: "accounts",
                table: "user_roles",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "accounts",
                table: "user_roles",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_roles_RoleId",
                schema: "accounts",
                table: "user_roles",
                newName: "ix_user_roles_role_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "accounts",
                table: "user_logins",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ProviderDisplayName",
                schema: "accounts",
                table: "user_logins",
                newName: "provider_display_name");

            migrationBuilder.RenameColumn(
                name: "ProviderKey",
                schema: "accounts",
                table: "user_logins",
                newName: "provider_key");

            migrationBuilder.RenameColumn(
                name: "LoginProvider",
                schema: "accounts",
                table: "user_logins",
                newName: "login_provider");

            migrationBuilder.RenameIndex(
                name: "IX_user_logins_UserId",
                schema: "accounts",
                table: "user_logins",
                newName: "ix_user_logins_user_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "accounts",
                table: "user_claims",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "accounts",
                table: "user_claims",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ClaimValue",
                schema: "accounts",
                table: "user_claims",
                newName: "claim_value");

            migrationBuilder.RenameColumn(
                name: "ClaimType",
                schema: "accounts",
                table: "user_claims",
                newName: "claim_type");

            migrationBuilder.RenameIndex(
                name: "IX_user_claims_UserId",
                schema: "accounts",
                table: "user_claims",
                newName: "ix_user_claims_user_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "accounts",
                table: "roles",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "accounts",
                table: "roles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "NormalizedName",
                schema: "accounts",
                table: "roles",
                newName: "normalized_name");

            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                schema: "accounts",
                table: "roles",
                newName: "concurrency_stamp");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                schema: "accounts",
                table: "role_permissions",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "PermissionId",
                schema: "accounts",
                table: "role_permissions",
                newName: "permission_id");

            migrationBuilder.RenameIndex(
                name: "IX_role_permissions_RoleId",
                schema: "accounts",
                table: "role_permissions",
                newName: "ix_role_permissions_role_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "accounts",
                table: "role_claims",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                schema: "accounts",
                table: "role_claims",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "ClaimValue",
                schema: "accounts",
                table: "role_claims",
                newName: "claim_value");

            migrationBuilder.RenameColumn(
                name: "ClaimType",
                schema: "accounts",
                table: "role_claims",
                newName: "claim_type");

            migrationBuilder.RenameIndex(
                name: "IX_role_claims_RoleId",
                schema: "accounts",
                table: "role_claims",
                newName: "ix_role_claims_role_id");

            migrationBuilder.RenameColumn(
                name: "Jti",
                schema: "accounts",
                table: "refresh_sessions",
                newName: "jti");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "accounts",
                table: "refresh_sessions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "accounts",
                table: "refresh_sessions",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                schema: "accounts",
                table: "refresh_sessions",
                newName: "refresh_token");

            migrationBuilder.RenameColumn(
                name: "ExpiresIn",
                schema: "accounts",
                table: "refresh_sessions",
                newName: "expires_in");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "accounts",
                table: "refresh_sessions",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_refresh_sessions_UserId",
                schema: "accounts",
                table: "refresh_sessions",
                newName: "ix_refresh_sessions_user_id");

            migrationBuilder.RenameColumn(
                name: "Code",
                schema: "accounts",
                table: "permissions",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "accounts",
                table: "permissions",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_permissions_Code",
                schema: "accounts",
                table: "permissions",
                newName: "ix_permissions_code");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "accounts",
                table: "participant_accounts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "accounts",
                table: "participant_accounts",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_participant_accounts_UserId",
                schema: "accounts",
                table: "participant_accounts",
                newName: "ix_participant_accounts_user_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "accounts",
                table: "admin_accounts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "accounts",
                table: "admin_accounts",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_admin_accounts_UserId",
                schema: "accounts",
                table: "admin_accounts",
                newName: "ix_admin_accounts_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_volunteer_accounts",
                schema: "accounts",
                table: "volunteer_accounts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                schema: "accounts",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_tokens",
                schema: "accounts",
                table: "user_tokens",
                columns: new[] { "user_id", "login_provider", "name" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_roles",
                schema: "accounts",
                table: "user_roles",
                columns: new[] { "user_id", "role_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_logins",
                schema: "accounts",
                table: "user_logins",
                columns: new[] { "login_provider", "provider_key" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_claims",
                schema: "accounts",
                table: "user_claims",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_roles",
                schema: "accounts",
                table: "roles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_role_permissions",
                schema: "accounts",
                table: "role_permissions",
                columns: new[] { "permission_id", "role_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_role_claims",
                schema: "accounts",
                table: "role_claims",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_refresh_sessions",
                schema: "accounts",
                table: "refresh_sessions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_permissions",
                schema: "accounts",
                table: "permissions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_participant_accounts",
                schema: "accounts",
                table: "participant_accounts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_admin_accounts",
                schema: "accounts",
                table: "admin_accounts",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_admin_accounts_users_user_id",
                schema: "accounts",
                table: "admin_accounts",
                column: "user_id",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_participant_accounts_users_user_id",
                schema: "accounts",
                table: "participant_accounts",
                column: "user_id",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_refresh_sessions_users_user_id",
                schema: "accounts",
                table: "refresh_sessions",
                column: "user_id",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_role_claims_roles_role_id",
                schema: "accounts",
                table: "role_claims",
                column: "role_id",
                principalSchema: "accounts",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permissions_permissions_permission_id",
                schema: "accounts",
                table: "role_permissions",
                column: "permission_id",
                principalSchema: "accounts",
                principalTable: "permissions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permissions_roles_role_id",
                schema: "accounts",
                table: "role_permissions",
                column: "role_id",
                principalSchema: "accounts",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_claims_users_user_id",
                schema: "accounts",
                table: "user_claims",
                column: "user_id",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_logins_users_user_id",
                schema: "accounts",
                table: "user_logins",
                column: "user_id",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_roles_role_id",
                schema: "accounts",
                table: "user_roles",
                column: "role_id",
                principalSchema: "accounts",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_users_user_id",
                schema: "accounts",
                table: "user_roles",
                column: "user_id",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_tokens_users_user_id",
                schema: "accounts",
                table: "user_tokens",
                column: "user_id",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_volunteer_accounts_users_user_id",
                schema: "accounts",
                table: "volunteer_accounts",
                column: "user_id",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
