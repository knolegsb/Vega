﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vega.Migrations
{
    public partial class SeedFeatures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Features (Name) VALUES ('Features1')");
            migrationBuilder.Sql("INSERT INTO Features (Name) VALUES ('Features2')");
            migrationBuilder.Sql("INSERT INTO Features (Name) VALUES ('Features3')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Features WHERE Name IN ('Feature1', 'Feature2', 'Feature3')");
        }
    }
}
