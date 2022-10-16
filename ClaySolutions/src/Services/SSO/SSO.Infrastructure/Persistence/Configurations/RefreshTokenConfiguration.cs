using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSO.Domain.Entities;

namespace SSO.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.Property(r => r.Token).IsRequired().IsUnicode();
            builder.Property(r => r.CreatedDate).IsRequired();
            builder.Property(r => r.ExpirationDate).IsRequired();
        }
    }
}
