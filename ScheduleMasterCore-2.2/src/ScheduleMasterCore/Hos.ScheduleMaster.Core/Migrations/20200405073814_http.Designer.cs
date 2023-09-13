﻿// <auto-generated />
using System;
using Hos.ScheduleMaster.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hos.ScheduleMaster.Core.Migrations
{
    [DbContext(typeof(SmDbContext))]
    [Migration("20200405073814_http")]
    partial class http
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Hos.ScheduleMaster.Core.Models.ScheduleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("char(36)");

                    b.Property<string>("AssemblyName")
                        .HasColumnName("assemblyname")
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200);

                    b.Property<string>("ClassName")
                        .HasColumnName("classname")
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200);

                    b.Property<DateTime>("CreateTime")
                        .HasColumnName("createtime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CreateUserId")
                        .HasColumnName("createuserid")
                        .HasColumnType("int");

                    b.Property<string>("CreateUserName")
                        .HasColumnName("createusername")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("CronExpression")
                        .HasColumnName("cronexpression")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("CustomParamsJson")
                        .HasColumnName("customparamsjson")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnName("enddate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("LastRunTime")
                        .HasColumnName("lastruntime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("MetaType")
                        .HasColumnName("metatype")
                        .HasColumnType("int");

                    b.Property<DateTime?>("NextRunTime")
                        .HasColumnName("nextruntime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Remark")
                        .HasColumnName("remark")
                        .HasColumnType("varchar(500) CHARACTER SET utf8mb4")
                        .HasMaxLength(500);

                    b.Property<bool>("RunLoop")
                        .HasColumnName("runloop")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnName("startdate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnName("status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<int>("TotalRunCount")
                        .HasColumnName("totalruncount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("schedules");
                });

            modelBuilder.Entity("Hos.ScheduleMaster.Core.Models.ScheduleExecutorEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<Guid>("ScheduleId")
                        .HasColumnName("scheduleid")
                        .HasColumnType("char(36)");

                    b.Property<string>("WorkerName")
                        .HasColumnName("workername")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("scheduleexecutors");
                });

            modelBuilder.Entity("Hos.ScheduleMaster.Core.Models.ScheduleHttpOptionEntity", b =>
                {
                    b.Property<Guid>("ScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("scheduleid")
                        .HasColumnType("char(36)");

                    b.Property<string>("Body")
                        .HasColumnName("body")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnName("contenttype")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("Headers")
                        .HasColumnName("headers")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnName("method")
                        .HasColumnType("varchar(10) CHARACTER SET utf8mb4")
                        .HasMaxLength(10);

                    b.Property<string>("RequestUrl")
                        .IsRequired()
                        .HasColumnName("requesturl")
                        .HasColumnType("varchar(500) CHARACTER SET utf8mb4")
                        .HasMaxLength(500);

                    b.HasKey("ScheduleId");

                    b.ToTable("schedulehttpoptions");
                });

            modelBuilder.Entity("Hos.ScheduleMaster.Core.Models.ScheduleKeeperEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<Guid>("ScheduleId")
                        .HasColumnName("scheduleid")
                        .HasColumnType("char(36)");

                    b.Property<int>("UserId")
                        .HasColumnName("userid")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("schedulekeepers");
                });

            modelBuilder.Entity("Hos.ScheduleMaster.Core.Models.ScheduleLockEntity", b =>
                {
                    b.Property<Guid>("ScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("scheduleid")
                        .HasColumnType("char(36)");

                    b.Property<string>("LockedNode")
                        .HasColumnName("lockednode")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("LockedTime")
                        .HasColumnName("lockedtime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnName("status")
                        .HasColumnType("int");

                    b.HasKey("ScheduleId");

                    b.ToTable("schedulelocks");
                });

            modelBuilder.Entity("Hos.ScheduleMaster.Core.Models.ScheduleReferenceEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<Guid>("ChildId")
                        .HasColumnName("childid")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ScheduleId")
                        .HasColumnName("scheduleid")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("schedulereferences");
                });

            modelBuilder.Entity("Hos.ScheduleMaster.Core.Models.ScheduleTraceEntity", b =>
                {
                    b.Property<Guid>("TraceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("traceid")
                        .HasColumnType("char(36)");

                    b.Property<double>("ElapsedTime")
                        .HasColumnName("elapsedtime")
                        .HasColumnType("double");

                    b.Property<DateTime>("EndTime")
                        .HasColumnName("endtime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Node")
                        .HasColumnName("node")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Result")
                        .HasColumnName("result")
                        .HasColumnType("int");

                    b.Property<Guid>("ScheduleId")
                        .HasColumnName("scheduleid")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnName("starttime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("TraceId");

                    b.ToTable("scheduletraces");
                });

            modelBuilder.Entity("Hos.ScheduleMaster.Core.Models.ServerNodeEntity", b =>
                {
                    b.Property<string>("NodeName")
                        .HasColumnName("nodename")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("AccessProtocol")
                        .IsRequired()
                        .HasColumnName("accessprotocol")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("AccessSecret")
                        .HasColumnName("accesssecret")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Host")
                        .IsRequired()
                        .HasColumnName("host")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("LastUpdateTime")
                        .HasColumnName("lastupdatetime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("MachineName")
                        .HasColumnName("machinename")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("NodeType")
                        .IsRequired()
                        .HasColumnName("nodetype")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Priority")
                        .HasColumnName("priority")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnName("status")
                        .HasColumnType("int");

                    b.HasKey("NodeName");

                    b.ToTable("servernodes");
                });

            modelBuilder.Entity("Hos.ScheduleMaster.Core.Models.SystemConfigEntity", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnName("key")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime")
                        .HasColumnName("createtime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Group")
                        .IsRequired()
                        .HasColumnName("group")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<bool>("IsReuired")
                        .HasColumnName("isreuired")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100);

                    b.Property<string>("Remark")
                        .HasColumnName("remark")
                        .HasColumnType("varchar(500) CHARACTER SET utf8mb4")
                        .HasMaxLength(500);

                    b.Property<int>("Sort")
                        .HasColumnName("sort")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnName("updatetime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdateUserName")
                        .HasColumnName("updateusername")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Value")
                        .HasColumnName("value")
                        .HasColumnType("varchar(1000) CHARACTER SET utf8mb4")
                        .HasMaxLength(1000);

                    b.HasKey("Key");

                    b.ToTable("systemconfigs");

                    b.HasData(
                        new
                        {
                            Key = "Email_SmtpServer",
                            CreateTime = new DateTime(2020, 4, 5, 15, 38, 14, 582, DateTimeKind.Local).AddTicks(8634),
                            Group = "邮件配置",
                            IsReuired = true,
                            Name = "邮件服务器",
                            Remark = "seed by efcore auto migration",
                            Sort = 1,
                            Value = ""
                        },
                        new
                        {
                            Key = "Email_SmtpPort",
                            CreateTime = new DateTime(2020, 4, 5, 15, 38, 14, 583, DateTimeKind.Local).AddTicks(535),
                            Group = "邮件配置",
                            IsReuired = true,
                            Name = "邮件服务器端口",
                            Remark = "seed by efcore auto migration",
                            Sort = 2,
                            Value = ""
                        },
                        new
                        {
                            Key = "Email_FromAccount",
                            CreateTime = new DateTime(2020, 4, 5, 15, 38, 14, 583, DateTimeKind.Local).AddTicks(606),
                            Group = "邮件配置",
                            IsReuired = true,
                            Name = "发件人账号",
                            Remark = "seed by efcore auto migration",
                            Sort = 3,
                            Value = ""
                        },
                        new
                        {
                            Key = "Email_FromAccountPwd",
                            CreateTime = new DateTime(2020, 4, 5, 15, 38, 14, 583, DateTimeKind.Local).AddTicks(608),
                            Group = "邮件配置",
                            IsReuired = true,
                            Name = "发件人账号密码",
                            Remark = "seed by efcore auto migration",
                            Sort = 4,
                            Value = ""
                        });
                });

            modelBuilder.Entity("Hos.ScheduleMaster.Core.Models.SystemLogEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<int>("Category")
                        .HasColumnName("category")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnName("createtime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnName("message")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Node")
                        .HasColumnName("node")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<Guid?>("ScheduleId")
                        .HasColumnName("scheduleid")
                        .HasColumnType("char(36)");

                    b.Property<string>("StackTrace")
                        .HasColumnName("stacktrace")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<Guid?>("TraceId")
                        .HasColumnName("traceid")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("systemlogs");
                });

            modelBuilder.Entity("Hos.ScheduleMaster.Core.Models.SystemUserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnName("createtime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnName("email")
                        .HasColumnType("varchar(500) CHARACTER SET utf8mb4")
                        .HasMaxLength(500);

                    b.Property<DateTime?>("LastLoginTime")
                        .HasColumnName("lastlogintime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("password")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Phone")
                        .HasColumnName("phone")
                        .HasColumnType("varchar(15) CHARACTER SET utf8mb4")
                        .HasMaxLength(15);

                    b.Property<string>("RealName")
                        .IsRequired()
                        .HasColumnName("realname")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Status")
                        .HasColumnName("status")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnName("username")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("systemusers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreateTime = new DateTime(2020, 4, 5, 15, 38, 14, 577, DateTimeKind.Local).AddTicks(8843),
                            Password = "96e79218965eb72c92a549dd5a330112",
                            RealName = "admin",
                            Status = 1,
                            UserName = "admin"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
