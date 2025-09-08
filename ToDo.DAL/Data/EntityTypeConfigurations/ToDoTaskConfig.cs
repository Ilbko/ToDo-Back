using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.DAL.Entities;

namespace ToDo.DAL.Data.EntityTypeConfigurations;

internal class ToDoTaskConfig : IEntityTypeConfiguration<ToDoTask>
{
    public void Configure(EntityTypeBuilder<ToDoTask> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        entity.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(50);

        entity.Property(e => e.Description)
            .HasMaxLength(250);

        entity.Property(e => e.Deadline)
            .IsRequired();

        entity.Property(e => e.Status)
            .IsRequired();

        entity.HasOne(e => e.User)
            .WithOne()
            .HasForeignKey<ToDoTask>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
