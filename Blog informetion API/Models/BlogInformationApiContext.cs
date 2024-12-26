using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Blog_informetion_API.Models;

public partial class BlogInformationApiContext : DbContext
{
    public BlogInformationApiContext()
    {
    }

    public BlogInformationApiContext(DbContextOptions<BlogInformationApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<News> News { get; set; }    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__News__3214EC07FA2F815E");

            entity.HasIndex(e => e.Titulo, "Titulo");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Autor)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.FechaDePublicacion).HasColumnName("Fecha_de_publicacion");
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Url_images)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("Url_images");

            entity.Property(e => e.Cuerpo)
                  .IsUnicode(false)
                  .HasColumnName("Campo");        


           
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
