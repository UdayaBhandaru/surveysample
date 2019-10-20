using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Identity.Service.Data.Migrations
{
    public partial class CreateIdentitySchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "IdentityUser",
               columns: table => new
               {
                   Id = table.Column<string>(maxLength: 256, nullable: false),
                   UserName = table.Column<string>(maxLength: 256, nullable: true),
                   NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                   Email = table.Column<string>(maxLength: 256, nullable: true),
                   NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                   EmailConfirmed = table.Column<bool>(nullable: false),
                   SecurityStamp = table.Column<string>(nullable: true),
                   ConcurrencyStamp = table.Column<string>(nullable: true),
                   PhoneNumber = table.Column<string>(nullable: true),
                   PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                   TwoFactorEnabled = table.Column<bool>(nullable: false),
                   LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                   LockoutEnabled = table.Column<bool>(nullable: false),
                   AccessFailedCount = table.Column<int>(nullable: false),
                   CreatedDate = table.Column<DateTimeOffset>(nullable: true),
                   OrganizationId = table.Column<int>(nullable: true),
                   TenantId = table.Column<int>(nullable: true),
                   DeletedInd = table.Column<bool>(nullable: false),
                   CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                   ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                   ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                   UserProfileAsJson = table.Column<string>(nullable: true),
                   IsActive = table.Column<bool>(nullable: true),
                   PasswordHash = table.Column<string>(nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_IdentityUser", x => x.Id);
               });
            MigrateData(migrationBuilder, "IdentityUser");
            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(maxLength: 256, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "Identityuser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(maxLength: 256,nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

           
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "IdentityUser",
                column: "NormalizedUserName",
                unique: true);
        }

        private void MigrateData(MigrationBuilder migrationBuilder, string tableName)
        {
            List<string> columns = new List<string>();
            List<string> values = new List<string>();
            using (StreamReader r = new StreamReader("Data.json"))
            {
                string json = r.ReadToEnd();
                JToken jsonData = JToken.Parse(json);
                JToken table = jsonData.SelectToken(tableName);
                if (table != null && table is JArray)
                {
                    foreach (var record in table)
                    {
                        columns.Clear();
                        values.Clear();

                        foreach (JProperty column in record)
                        {
                            if (!string.IsNullOrEmpty(column.Value.ToString()))
                            {
                                columns.Add(column.Name);
                                values.Add(column.Value.ToString());
                            }
                        }

                        migrationBuilder.InsertData(tableName, columns.ToArray(), values.ToArray());

                    }
                }

            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "IdentityUser");
        }
    }
}
