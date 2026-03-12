using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Infrastructure.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            // Sabit ID'ler (Static olması şart, yoksa yine aynı hatayı alırız)
            var adminRoleId = Guid.Parse("446368C7-23A4-4A8E-9549-9A0E3C62985D");
            var userRoleId = Guid.Parse("149DA092-2376-4F23-AD15-E1A7D1875151");

            builder.HasData(
                new IdentityRole<Guid>
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "9b35f620-f686-42ab-8c92-073c38511dec"
                },
                new IdentityRole<Guid>
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "7b5c69e6-f38f-4464-8a5e-c70bab6a3428"
                }
            );
        }
    }
}
