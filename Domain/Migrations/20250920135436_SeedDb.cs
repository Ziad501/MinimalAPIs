using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class SeedDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Credits", "Title", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 3, "Computer Science Fundamentals", new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 2, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 4, "Advanced Database Management", new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 3, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 3, "Modern Web Applications", new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 4, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 4, "Algorithms and Data Structures", new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 5, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 3, "Software Engineering Principles", new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 6, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 3, "Introduction to Artificial Intelligence", new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DateOfBirth", "FirstName", "IdNumber", "LastName", "Picture", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", new DateTime(2005, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ali", "2023001", "Hassan", "https://placehold.co/600x400/3498db/ffffff?text=Ali", new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 2, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", new DateTime(2004, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nour", "2023002", "Khaled", "https://placehold.co/600x400/e74c3c/ffffff?text=Nour", new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 3, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", new DateTime(2005, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Youssef", "2023003", "Mahmoud", "https://placehold.co/600x400/2ecc71/ffffff?text=Youssef", new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 4, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", new DateTime(2004, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fatima", "2023004", "Said", "https://placehold.co/600x400/f1c40f/ffffff?text=Fatima", new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 5, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", new DateTime(2005, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Omar", "2023005", "Adel", "https://placehold.co/600x400/9b59b6/ffffff?text=Omar", new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 6, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", new DateTime(2004, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mariam", "2023006", "Gamal", "https://placehold.co/600x400/1abc9c/ffffff?text=Mariam", new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 7, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", new DateTime(2005, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ahmed", "2023007", "Tarek", "https://placehold.co/600x400/e67e22/ffffff?text=Ahmed", new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 8, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", new DateTime(2004, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hana", "2023008", "Ibrahim", "https://placehold.co/600x400/34495e/ffffff?text=Hana", new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 9, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", new DateTime(2005, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khaled", "2023009", "Ezzat", "https://placehold.co/600x400/d35400/ffffff?text=Khaled", new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 10, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", new DateTime(2004, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Salma", "2023010", "Mostafa", "https://placehold.co/600x400/c0392b/ffffff?text=Salma", new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" }
                });

            migrationBuilder.InsertData(
                table: "Enrollments",
                columns: new[] { "Id", "CourseId", "CreatedAt", "CreatedBy", "StudentId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 1, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 2, 3, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 1, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 3, 1, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 2, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 4, 2, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 2, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 5, 4, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 3, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 6, 5, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 4, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 7, 6, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 4, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 8, 2, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 5, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 9, 3, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 6, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 10, 4, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 7, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 11, 1, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 8, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 12, 5, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 9, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" },
                    { 13, 6, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System", 10, new DateTime(2025, 9, 20, 16, 53, 0, 0, DateTimeKind.Utc), "System" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
