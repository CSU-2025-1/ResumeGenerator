using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeGenerator.ApiService.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_first_name = table.Column<string>(type: "text", nullable: false),
                    user_last_name = table.Column<string>(type: "text", nullable: false),
                    user_patronymic = table.Column<string>(type: "text", nullable: false),
                    desired_position = table.Column<string>(type: "text", nullable: false),
                    github_link = table.Column<string>(type: "text", nullable: false),
                    telegram_link = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    education = table.Column<string>(type: "text", nullable: false),
                    experience_years = table.Column<int>(type: "integer", nullable: false),
                    hard_skills = table.Column<string>(type: "text", nullable: false),
                    soft_skills = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_phone_number",
                table: "users",
                column: "phone_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
