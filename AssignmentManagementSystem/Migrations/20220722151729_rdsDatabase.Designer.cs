// <auto-generated />
using System;
using AssignmentManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AssignmentManagementSystem.Migrations
{
    [DbContext(typeof(AssignmentManagementSystemContext))]
    [Migration("20220722151729_rdsDatabase")]
    partial class rdsDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AssignmentManagementSystem.Areas.Identity.Data.AssignmentManagementSystemUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("firstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userrole")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("AssignmentManagementSystem.Models.Assignment", b =>
                {
                    b.Property<string>("AssignmentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AssignmentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModuleRefId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("handoutDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("submissionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("AssignmentId");

                    b.HasIndex("ModuleRefId");

                    b.ToTable("Assignment");
                });

            modelBuilder.Entity("AssignmentManagementSystem.Models.LecturerModule", b =>
                {
                    b.Property<string>("LecturerModuleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ModuleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LecturerModuleId");

                    b.HasIndex("ModuleId");

                    b.HasIndex("UserId");

                    b.ToTable("LecturerModule");
                });

            modelBuilder.Entity("AssignmentManagementSystem.Models.Module", b =>
                {
                    b.Property<string>("ModuleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ModuleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ModuleId");

                    b.ToTable("Module");
                });

            modelBuilder.Entity("AssignmentManagementSystem.Models.Task", b =>
                {
                    b.Property<string>("TaskId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AssignmentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("SubmissionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SubmitStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TaskId");

                    b.HasIndex("AssignmentId");

                    b.HasIndex("UserId");

                    b.ToTable("Task");
                });

            modelBuilder.Entity("AssignmentManagementSystem.Models.Team", b =>
                {
                    b.Property<string>("TeamId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TeamId");

                    b.ToTable("Team");
                });

            modelBuilder.Entity("AssignmentManagementSystem.Models.TeamAssignment", b =>
                {
                    b.Property<string>("TeamAssignmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AssignmentId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeamId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("mark")
                        .HasColumnType("int");

                    b.Property<string>("s3Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("submitStatus")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TeamAssignmentId");

                    b.ToTable("TeamAssignment");
                });

            modelBuilder.Entity("AssignmentManagementSystem.Models.TeamUser", b =>
                {
                    b.Property<string>("TeamUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TeamId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TeamUserId");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserId");

                    b.ToTable("TeamUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("AssignmentManagementSystem.Models.Assignment", b =>
                {
                    b.HasOne("AssignmentManagementSystem.Models.Module", "Module")
                        .WithMany("Assignments")
                        .HasForeignKey("ModuleRefId");
                });

            modelBuilder.Entity("AssignmentManagementSystem.Models.LecturerModule", b =>
                {
                    b.HasOne("AssignmentManagementSystem.Models.Module", "Module")
                        .WithMany()
                        .HasForeignKey("ModuleId");

                    b.HasOne("AssignmentManagementSystem.Areas.Identity.Data.AssignmentManagementSystemUser", "Users")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("AssignmentManagementSystem.Models.Task", b =>
                {
                    b.HasOne("AssignmentManagementSystem.Models.Assignment", "Assignment")
                        .WithMany()
                        .HasForeignKey("AssignmentId");

                    b.HasOne("AssignmentManagementSystem.Areas.Identity.Data.AssignmentManagementSystemUser", "Users")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("AssignmentManagementSystem.Models.TeamUser", b =>
                {
                    b.HasOne("AssignmentManagementSystem.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");

                    b.HasOne("AssignmentManagementSystem.Areas.Identity.Data.AssignmentManagementSystemUser", "Users")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AssignmentManagementSystem.Areas.Identity.Data.AssignmentManagementSystemUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AssignmentManagementSystem.Areas.Identity.Data.AssignmentManagementSystemUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AssignmentManagementSystem.Areas.Identity.Data.AssignmentManagementSystemUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("AssignmentManagementSystem.Areas.Identity.Data.AssignmentManagementSystemUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
