using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Portfolio.Dal.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsSuperAdmin = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PublicId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, defaultValue: "SYSTEM"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)),
                    ModifiedBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LockId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    PublicId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, defaultValue: "SYSTEM"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)),
                    ModifiedBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LockId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PublicId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, defaultValue: "SYSTEM"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)),
                    ModifiedBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LockId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResetToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PublicId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, defaultValue: "SYSTEM"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)),
                    ModifiedBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LockId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PublicId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, defaultValue: "SYSTEM"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)),
                    ModifiedBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LockId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountUser_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_CreatedAt",
                table: "Account",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Account_OwnerId",
                table: "Account",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_PublicId",
                table: "Account",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_Slug",
                table: "Account",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountUser_AccountId",
                table: "AccountUser",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountUser_AccountId_UserId",
                table: "AccountUser",
                columns: new[] { "AccountId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountUser_PublicId",
                table: "AccountUser",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountUser_Role",
                table: "AccountUser",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_AccountUser_UserId",
                table: "AccountUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetToken_ExpiresAt",
                table: "PasswordResetToken",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetToken_PublicId",
                table: "PasswordResetToken",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetToken_Token",
                table: "PasswordResetToken",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetToken_UserId",
                table: "PasswordResetToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetToken_UserId_ExpiresAt",
                table: "PasswordResetToken",
                columns: new[] { "UserId", "ExpiresAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_ExpiresAt",
                table: "RefreshToken",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_PublicId",
                table: "RefreshToken",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_Token",
                table: "RefreshToken",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedAt",
                table: "User",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_PublicId",
                table: "User",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                table: "User",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountUser");

            migrationBuilder.DropTable(
                name: "PasswordResetToken");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
