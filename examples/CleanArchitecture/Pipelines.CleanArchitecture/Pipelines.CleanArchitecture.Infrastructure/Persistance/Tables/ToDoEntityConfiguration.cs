using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pipelines.CleanArchitecture.Domain;

namespace Pipelines.CleanArchitecture.Infrastructure.Persistance.Tables;

public class ToDoEntityConfiguration: IEntityTypeConfiguration<ToDo>
{
    public void Configure(EntityTypeBuilder<ToDo> builder)
    {
        builder.ToTable("ToDo");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title);

        builder.Ignore(x => x.DomainEvents);
    }
}