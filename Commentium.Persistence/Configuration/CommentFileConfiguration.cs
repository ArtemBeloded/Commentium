using Commentium.Domain.Comments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Commentium.Persistence.Configuration
{
    internal class CommentFileConfiguration : IEntityTypeConfiguration<CommentFile>
    {
        public void Configure(EntityTypeBuilder<CommentFile> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
