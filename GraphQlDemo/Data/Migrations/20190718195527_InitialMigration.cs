using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GraphQlDemo.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Isbn = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    AuthorId = table.Column<int>(nullable: false),
                    PublisherId = table.Column<int>(nullable: false),
                    SubTitle = table.Column<string>(nullable: true),
                    Published = table.Column<DateTime>(nullable: false),
                    Pages = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Isbn);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_Publishers_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Publishers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Marijn Haverbeke" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Addy Osmani" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Axel Rauschmayer" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "Eric Elliott" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Name" },
                values: new object[] { 5, "Nicholas C. Zakas" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Name" },
                values: new object[] { 6, "Kyle Simpson" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Name" },
                values: new object[] { 7, "Richard E. Silverman" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Name" },
                values: new object[] { 8, "Glenn Block, et al." });

            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "No Starch Press" });

            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "O'Reilly Media" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Isbn", "AuthorId", "Description", "Pages", "Published", "PublisherId", "SubTitle", "Title", "Website" },
                values: new object[] { "9781593275846", 1, "JavaScript lies at the heart of almost every modern web application, from social apps to the newest browser-based games. Though simple for beginners to pick up and play with, JavaScript is a flexible, complex language that you can use to build full-scale applications.", 472, new DateTime(2014, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), 1, "A Modern Introduction to Programming", "Eloquent JavaScript, Second Edition", "http://eloquentjavascript.net/" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Isbn", "AuthorId", "Description", "Pages", "Published", "PublisherId", "SubTitle", "Title", "Website" },
                values: new object[] { "9781593277574", 5, "ECMAScript 6 represents the biggest update to the core of JavaScript in the history of the language. In Understanding ECMAScript 6, expert developer Nicholas C. Zakas provides a complete guide to the object types, syntax, and other exciting changes that ECMAScript 6 brings to JavaScript.", 352, new DateTime(2016, 9, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, "The Definitive Guide for JavaScript Developers", "Understanding ECMAScript 6", "https://leanpub.com/understandinges6/read" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Isbn", "AuthorId", "Description", "Pages", "Published", "PublisherId", "SubTitle", "Title", "Website" },
                values: new object[] { "9781449331818", 2, "With Learning JavaScript Design Patterns, you'll learn how to write beautiful, structured, and maintainable JavaScript by applying classical and modern design patterns to the language. If you want to keep your code efficient, more manageable, and up-to-date with the latest best practices, this book is for you.", 254, new DateTime(2012, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "A JavaScript and jQuery Developer's Guide", "Learning JavaScript Design Patterns", "http://www.addyosmani.com/resources/essentialjsdesignpatterns/book/" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Isbn", "AuthorId", "Description", "Pages", "Published", "PublisherId", "SubTitle", "Title", "Website" },
                values: new object[] { "9781449365035", 3, "Like it or not, JavaScript is everywhere these days-from browser to server to mobile-and now you, too, need to learn the language or dive deeper than you have. This concise book guides you into and through JavaScript, written by a veteran programmer who once found himself in the same position.", 460, new DateTime(2014, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "An In-Depth Guide for Programmers", "Speaking JavaScript", "http://speakingjs.com/" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Isbn", "AuthorId", "Description", "Pages", "Published", "PublisherId", "SubTitle", "Title", "Website" },
                values: new object[] { "9781491950296", 4, "Take advantage of JavaScript's power to build robust web-scale or enterprise applications that are easy to extend and maintain. By applying the design patterns outlined in this practical book, experienced JavaScript developers will learn how to write flexible and resilient code that's easier-yes, easier-to work with as your code base grows.", 254, new DateTime(2014, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Robust Web Architecture with Node, HTML5, and Modern JS Libraries", "Programming JavaScript Applications", "http://chimera.labs.oreilly.com/books/1234000000262/index.html" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Isbn", "AuthorId", "Description", "Pages", "Published", "PublisherId", "SubTitle", "Title", "Website" },
                values: new object[] { "9781491904244", 6, "No matter how much experience you have with JavaScript, odds are you don�t fully understand the language. As part of the \"You Don�t Know JS\" series, this compact guide focuses on new features available in ECMAScript 6 (ES6), the latest version of the standard upon which JavaScript is built.", 278, new DateTime(2015, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 2, "ES6 & Beyond", "You Don't Know JS", "https://github.com/getify/You-Dont-Know-JS/tree/master/es6 & beyond" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Isbn", "AuthorId", "Description", "Pages", "Published", "PublisherId", "SubTitle", "Title", "Website" },
                values: new object[] { "9781449325862", 7, "This pocket guide is the perfect on-the-job companion to Git, the distributed version control system. It provides a compact, readable introduction to Git for new users, as well as a reference to common commands and procedures for those of you with Git experience.", 234, new DateTime(2013, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2, "A Working Introduction", "Git Pocket Guide", "http://chimera.labs.oreilly.com/books/1230000000561/index.html" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Isbn", "AuthorId", "Description", "Pages", "Published", "PublisherId", "SubTitle", "Title", "Website" },
                values: new object[] { "9781449337711", 8, "Design and build Web APIs for a broad range of clients�including browsers and mobile devices�that can adapt to change over time. This practical, hands-on guide takes you through the theory and tools you need to build evolvable HTTP services with Microsoft�s ASP.NET Web API framework. In the process, you�ll learn how design and implement a real-world Web API.", 538, new DateTime(2014, 4, 7, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Harnessing the Power of the Web", "Designing Evolvable Web APIs with ASP.NET", "http://chimera.labs.oreilly.com/books/1234000001708/index.html" });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublisherId",
                table: "Books",
                column: "PublisherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Publishers");
        }
    }
}
