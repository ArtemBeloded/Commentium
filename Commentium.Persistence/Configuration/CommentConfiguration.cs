using Commentium.Domain.Comments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Commentium.Persistence.Configuration
{
    internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(f => f.AttachedFile)
                .WithOne()
                .HasForeignKey<CommentFile>(cf => cf.CommentId)
                .OnDelete(DeleteBehavior.Cascade);;

            builder.Property(p => p.ParentCommentId)
                .IsRequired(false);

            builder.HasMany(r => r.Replies)
                .WithOne()
                .HasForeignKey(p => p.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
