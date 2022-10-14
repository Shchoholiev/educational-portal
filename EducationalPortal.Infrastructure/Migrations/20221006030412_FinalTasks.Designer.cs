﻿// <auto-generated />
using System;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EducationalPortal.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20221006030412_FinalTasks")]
    partial class FinalTasks
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AuthorBook", b =>
                {
                    b.Property<int>("AuthorsId")
                        .HasColumnType("int");

                    b.Property<int>("BooksId")
                        .HasColumnType("int");

                    b.HasKey("AuthorsId", "BooksId");

                    b.HasIndex("BooksId");

                    b.ToTable("AuthorBook");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.CartItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("UserId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AuthorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FinalTaskId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Thumbnail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("FinalTaskId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.EducationalMaterials.MaterialsBase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Materials", (string)null);
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.EducationalMaterials.Properties.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.EducationalMaterials.Properties.Extension", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Extensions");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.EducationalMaterials.Properties.Quality", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Qualities");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.EducationalMaterials.Properties.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.FinalTasks.FinalTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ReviewDeadlineTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("FinalTasks");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.FinalTasks.ReviewQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("FinalTaskId")
                        .HasColumnType("int");

                    b.Property<int>("MaxMark")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FinalTaskId");

                    b.ToTable("ReviewQuestions");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.FinalTasks.SubmittedFinalTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FileLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FinalTaskId")
                        .HasColumnType("int");

                    b.Property<int>("Mark")
                        .HasColumnType("int");

                    b.Property<string>("RevievedById")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SubmitDateUTC")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("FinalTaskId");

                    b.HasIndex("UserId");

                    b.ToTable("SubmittedFinalTasks");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.FinalTasks.SubmittedReviewQuestion", b =>
                {
                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<int>("SubmittedFinalTaskId")
                        .HasColumnType("int");

                    b.Property<int>("Mark")
                        .HasColumnType("int");

                    b.HasKey("QuestionId", "SubmittedFinalTaskId");

                    b.HasIndex("SubmittedFinalTaskId");

                    b.ToTable("SubmittedReviewQuestions");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.JoinEntities.CoursesMaterials", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("MaterialId")
                        .HasColumnType("int");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.HasKey("CourseId", "MaterialId");

                    b.HasIndex("MaterialId");

                    b.ToTable("CoursesMaterials");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.JoinEntities.CoursesSkills", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.HasKey("CourseId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("CoursesSkills");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.JoinEntities.UsersCourses", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("LearnedMaterialsCount")
                        .HasColumnType("int");

                    b.Property<int>("MaterialsCount")
                        .HasColumnType("int");

                    b.HasKey("UserId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("UsersCourses");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.JoinEntities.UsersSkills", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.HasKey("UserId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("UsersSkills");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.ShoppingHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("UserId");

                    b.ToTable("ShoppingHistory");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Balance")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserTokenId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UserTokenId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.UserToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("UserToken");
                });

            modelBuilder.Entity("MaterialsBaseUser", b =>
                {
                    b.Property<int>("MaterialsId")
                        .HasColumnType("int");

                    b.Property<string>("UsersId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MaterialsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("MaterialsBaseUser");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<int>("RolesId")
                        .HasColumnType("int");

                    b.Property<string>("UsersId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RolesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("RoleUser");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.EducationalMaterials.Article", b =>
                {
                    b.HasBaseType("EducationalPortal.Core.Entities.EducationalMaterials.MaterialsBase");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ResourceId")
                        .HasColumnType("int");

                    b.HasIndex("ResourceId");

                    b.ToTable("Articles", (string)null);
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.EducationalMaterials.Book", b =>
                {
                    b.HasBaseType("EducationalPortal.Core.Entities.EducationalMaterials.MaterialsBase");

                    b.Property<int>("ExtensionId")
                        .HasColumnType("int");

                    b.Property<int>("PagesCount")
                        .HasColumnType("int");

                    b.Property<int>("PublicationYear")
                        .HasColumnType("int");

                    b.HasIndex("ExtensionId");

                    b.ToTable("Books", (string)null);
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.EducationalMaterials.Video", b =>
                {
                    b.HasBaseType("EducationalPortal.Core.Entities.EducationalMaterials.MaterialsBase");

                    b.Property<DateTime>("Duration")
                        .HasColumnType("datetime2");

                    b.Property<int>("QualityId")
                        .HasColumnType("int");

                    b.HasIndex("QualityId");

                    b.ToTable("Videos", (string)null);
                });

            modelBuilder.Entity("AuthorBook", b =>
                {
                    b.HasOne("EducationalPortal.Core.Entities.EducationalMaterials.Properties.Author", null)
                        .WithMany()
                        .HasForeignKey("AuthorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationalPortal.Core.Entities.EducationalMaterials.Book", null)
                        .WithMany()
                        .HasForeignKey("BooksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.CartItem", b =>
                {
                    b.HasOne("EducationalPortal.Core.Entities.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationalPortal.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.Course", b =>
                {
                    b.HasOne("EducationalPortal.Core.Entities.User", "Author")
                        .WithMany("CreatedCourses")
                        .HasForeignKey("AuthorId");

                    b.HasOne("EducationalPortal.Core.Entities.FinalTasks.FinalTask", "FinalTask")
                        .WithMany("Courses")
                        .HasForeignKey("FinalTaskId");

                    b.Navigation("Author");

                    b.Navigation("FinalTask");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.FinalTasks.ReviewQuestion", b =>
                {
                    b.HasOne("EducationalPortal.Core.Entities.FinalTasks.FinalTask", "FinalTask")
                        .WithMany("ReviewQuestions")
                        .HasForeignKey("FinalTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FinalTask");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.FinalTasks.SubmittedFinalTask", b =>
                {
                    b.HasOne("EducationalPortal.Core.Entities.FinalTasks.FinalTask", "FinalTask")
                        .WithMany("SubmittedFinalTasks")
                        .HasForeignKey("FinalTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationalPortal.Core.Entities.User", "User")
                        .WithMany("SubmittedFinalTasks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FinalTask");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.FinalTasks.SubmittedReviewQuestion", b =>
                {
                    b.HasOne("EducationalPortal.Core.Entities.FinalTasks.ReviewQuestion", "Question")
                        .WithMany("SubmittedReviewQuestions")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationalPortal.Core.Entities.FinalTasks.SubmittedFinalTask", "SubmittedFinalTask")
                        .WithMany("SubmittedReviewQuestions")
                        .HasForeignKey("SubmittedFinalTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("SubmittedFinalTask");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.JoinEntities.CoursesMaterials", b =>
                {
                    b.HasOne("EducationalPortal.Core.Entities.Course", "Course")
                        .WithMany("CoursesMaterials")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationalPortal.Core.Entities.EducationalMaterials.MaterialsBase", "Material")
                        .WithMany("CoursesMaterials")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Material");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.JoinEntities.CoursesSkills", b =>
                {
                    b.HasOne("EducationalPortal.Core.Entities.Course", "Course")
                        .WithMany("CoursesSkills")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationalPortal.Core.Entities.Skill", "Skill")
                        .WithMany("CoursesSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.JoinEntities.UsersCourses", b =>
                {
                    b.HasOne("EducationalPortal.Core.Entities.Course", "Course")
                        .WithMany("UsersCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationalPortal.Core.Entities.User", "User")
                        .WithMany("UsersCourses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.JoinEntities.UsersSkills", b =>
                {
                    b.HasOne("EducationalPortal.Core.Entities.Skill", "Skill")
                        .WithMany("UsersSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationalPortal.Core.Entities.User", "User")
                        .WithMany("UsersSkills")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Skill");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.ShoppingHistory", b =>
                {
                    b.HasOne("EducationalPortal.Core.Entities.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationalPortal.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.User", b =>
                {
                    b.HasOne("EducationalPortal.Core.Entities.UserToken", "UserToken")
                        .WithMany()
                        .HasForeignKey("UserTokenId");

                    b.Navigation("UserToken");
                });

            modelBuilder.Entity("MaterialsBaseUser", b =>
                {
                    b.HasOne("EducationalPortal.Core.Entities.EducationalMaterials.MaterialsBase", null)
                        .WithMany()
                        .HasForeignKey("MaterialsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationalPortal.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("EducationalPortal.Core.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationalPortal.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.EducationalMaterials.Article", b =>
                {
                    b.HasOne("EducationalPortal.Core.Entities.EducationalMaterials.MaterialsBase", null)
                        .WithOne()
                        .HasForeignKey("EducationalPortal.Core.Entities.EducationalMaterials.Article", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("EducationalPortal.Core.Entities.EducationalMaterials.Properties.Resource", "Resource")
                        .WithMany("Articles")
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Resource");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.EducationalMaterials.Book", b =>
                {
                    b.HasOne("EducationalPortal.Core.Entities.EducationalMaterials.Properties.Extension", "Extension")
                        .WithMany()
                        .HasForeignKey("ExtensionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationalPortal.Core.Entities.EducationalMaterials.MaterialsBase", null)
                        .WithOne()
                        .HasForeignKey("EducationalPortal.Core.Entities.EducationalMaterials.Book", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Extension");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.EducationalMaterials.Video", b =>
                {
                    b.HasOne("EducationalPortal.Core.Entities.EducationalMaterials.MaterialsBase", null)
                        .WithOne()
                        .HasForeignKey("EducationalPortal.Core.Entities.EducationalMaterials.Video", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("EducationalPortal.Core.Entities.EducationalMaterials.Properties.Quality", "Quality")
                        .WithMany()
                        .HasForeignKey("QualityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quality");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.Course", b =>
                {
                    b.Navigation("CoursesMaterials");

                    b.Navigation("CoursesSkills");

                    b.Navigation("UsersCourses");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.EducationalMaterials.MaterialsBase", b =>
                {
                    b.Navigation("CoursesMaterials");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.EducationalMaterials.Properties.Resource", b =>
                {
                    b.Navigation("Articles");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.FinalTasks.FinalTask", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("ReviewQuestions");

                    b.Navigation("SubmittedFinalTasks");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.FinalTasks.ReviewQuestion", b =>
                {
                    b.Navigation("SubmittedReviewQuestions");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.FinalTasks.SubmittedFinalTask", b =>
                {
                    b.Navigation("SubmittedReviewQuestions");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.Skill", b =>
                {
                    b.Navigation("CoursesSkills");

                    b.Navigation("UsersSkills");
                });

            modelBuilder.Entity("EducationalPortal.Core.Entities.User", b =>
                {
                    b.Navigation("CreatedCourses");

                    b.Navigation("SubmittedFinalTasks");

                    b.Navigation("UsersCourses");

                    b.Navigation("UsersSkills");
                });
#pragma warning restore 612, 618
        }
    }
}
