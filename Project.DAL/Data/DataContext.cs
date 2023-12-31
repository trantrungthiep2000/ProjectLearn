﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Domain.Aggregates;

namespace Project.DAL.Data;

/// <summary>
/// Information of data context
/// CreatedBy: ThiepTT(30/10/2023)
/// </summary>
public class DataContext : IdentityDbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    /// <summary>
    /// UserProfiles
    /// </summary>
    public DbSet<UserProfile> UserProfiles { get; set; }

    /// <summary>
    /// Products
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// On model creating
    /// </summary>
    /// <param name="builder">ModelBuilder</param>
    /// CreatedBy: ThiepTT(30/10/2023)
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<UserProfile>().HasKey(userProfile => userProfile.UserProfileId);

        builder.Entity<UserProfile>().HasIndex(userProfile => userProfile.Email).IsUnique();

        builder.Entity<Product>().HasKey(product => product.ProductId);

        builder.Entity<IdentityUser>().HasIndex(identityUser => identityUser.Email).IsUnique();

        builder.Entity<IdentityUserLogin<string>>().HasKey(identityUserLogin => identityUserLogin.UserId);

        builder.Entity<IdentityUserRole<string>>().HasKey(identityUserRole => new { identityUserRole.UserId, identityUserRole.RoleId });

        builder.Entity<IdentityUserToken<string>>().HasKey(identityUserToken => identityUserToken.UserId);
    }
}