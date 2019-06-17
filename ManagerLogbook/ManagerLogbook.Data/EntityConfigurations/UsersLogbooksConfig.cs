using ManagerLogbook.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Data.EntityConfiguration
{
    public class UsersLogbooksConfig : IEntityTypeConfiguration<UsersLogbooks>
    {
        public void Configure(EntityTypeBuilder<UsersLogbooks> builder)
        {
            builder.HasKey(x => new { x.UserId, x.LogbookId });
        }
    }
}
