using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public virtual DbSet<Department> Departments { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<EmployeeDepartment> EmployeeDepartments { get; set; }

    //EF Code First設定
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //設定Department
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.DepartmentID);

            entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
            entity.Property(d => d.Disable).IsRequired();
            entity.Property(d => d.CreatedTime).HasDefaultValueSql("GETDATE()");
            entity.Property(d => d.UpdatedTime).HasDefaultValueSql("GETDATE()");

            //設定自關聯
            entity.HasOne(d => d.ParentDepartment)
                .WithMany(d => d.SubDepartments)
                .HasForeignKey(d => d.AffiliatedDepartmentID)
                .OnDelete(DeleteBehavior.Restrict); //避免循環刪除

            //設定Index
            entity.HasIndex(d => d.AffiliatedDepartmentID)
                .HasDatabaseName("IX_Departments_AffiliatedDepartmentID");
        });

        //設定Employee
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeID);

            entity.Property(e => e.AccountID).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Disable).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(20);

            entity.Property(e => e.CreatedTime).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedTime).HasDefaultValueSql("GETDATE()");
        });

        //設定EmployeeDepartment (多對多關聯表)
        modelBuilder.Entity<EmployeeDepartment>(entity =>
        {
            entity.HasKey(ed => ed.EmployeeDepartmentID);

            entity.Property(ed => ed.Disable).IsRequired();
            entity.Property(ed => ed.CreatedTime).HasDefaultValueSql("GETDATE()");
            entity.Property(ed => ed.UpdatedTime).HasDefaultValueSql("GETDATE()");

            //設定 Employee 和 Department 的 FK
            entity.HasOne(ed => ed.Employee)
                .WithMany(e => e.EmployeeDepartments)
                .HasForeignKey(ed => ed.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(ed => ed.Department)
                .WithMany(d => d.EmployeeDepartments)
                .HasForeignKey(ed => ed.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            //設定複合唯一索引
            entity.HasIndex(ed => new { ed.EmployeeID, ed.DepartmentID }) //複合索引
                .IsUnique() //唯一
                .HasDatabaseName("IX_EmployeeDepartment_EmployeeID_DepartmentID");

            //設定Index
            entity.HasIndex(ed => ed.DepartmentID)
            .HasDatabaseName("IX_EmployeeDepartment_DepartmentID");
            entity.HasIndex(ed => ed.EmployeeID)
            .HasDatabaseName("IX_EmployeeDepartment_EmployeeID");
        });
    }
}
