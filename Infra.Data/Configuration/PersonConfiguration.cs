using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configuration;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("People", "AF");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("uniqueidentifier").ValueGeneratedNever().IsRequired();
        builder.Property(e => e.Name).HasColumnName("Name").HasColumnType("varchar").HasMaxLength(80).IsRequired();
        builder.Property(e => e.Email).HasColumnName("Email").HasColumnType("varchar").HasMaxLength(80).IsRequired();
        builder.Property(e => e.Birthday).HasColumnName("Birthday").HasColumnType("date");
        builder.Property(e => e.PersonalCode).HasColumnName("CPF").HasColumnType("varchar").HasMaxLength(11);
        builder.Property(e => e.Gender).HasColumnName("Gender").IsRequired().HasConversion<string>().HasMaxLength(10);
        builder.Property(e => e.Phone).HasColumnName("Phone").HasColumnType("varchar").HasMaxLength(11);

        builder.HasMany<User>(e => e.Users).WithOne(e => e.Person).HasForeignKey(e => e.PersonId);
    }
}