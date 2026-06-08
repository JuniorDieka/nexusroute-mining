using Microsoft.EntityFrameworkCore;
using NexusRoute.Domain.Entities;

namespace NexusRoute.Infrastructure.Data;

public class NexusRouteDbContext : DbContext
{
    public NexusRouteDbContext(DbContextOptions<NexusRouteDbContext> options)
        : base(options)
    {
    }

    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<Telemetry> Telemetry => Set<Telemetry>();
    public DbSet<OreMovementLog> OreMovementLogs => Set<OreMovementLog>();
    public DbSet<Route> Routes => Set<Route>();
    public DbSet<Checkpoint> Checkpoints => Set<Checkpoint>();
    public DbSet<Convoy> Convoys => Set<Convoy>();
    public DbSet<ConvoyCheckpointLog> ConvoyCheckpointLogs => Set<ConvoyCheckpointLog>();
    public DbSet<Alert> Alerts => Set<Alert>();
    public DbSet<ProductionQuota> ProductionQuotas => Set<ProductionQuota>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NexusRouteDbContext).Assembly);

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.AssetCode).IsUnique();
            entity.HasIndex(e => e.Type);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.LastTelemetryTime);
            
            entity.Property(e => e.AssetCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.CurrentLocation).HasMaxLength(200);
        });

        modelBuilder.Entity<Telemetry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.AssetId);
            entity.HasIndex(e => e.Timestamp);
            entity.HasIndex(e => new { e.AssetId, e.Timestamp });
            
            entity.HasOne(e => e.Asset)
                .WithMany(a => a.TelemetryRecords)
                .HasForeignKey(e => e.AssetId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OreMovementLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.AssetId);
            entity.HasIndex(e => e.EventTime);
            entity.HasIndex(e => e.CycleId);
            entity.HasIndex(e => new { e.AssetId, e.EventTime });
            
            entity.Property(e => e.EventType).HasMaxLength(50).IsRequired();
            entity.Property(e => e.SourceLocation).HasMaxLength(200).IsRequired();
            entity.Property(e => e.DestinationLocation).HasMaxLength(200);
            entity.Property(e => e.OperatorName).HasMaxLength(200);
            
            entity.HasOne(e => e.Asset)
                .WithMany(a => a.OreMovementLogs)
                .HasForeignKey(e => e.AssetId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.IsActive);
            
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.StartLocation).HasMaxLength(200).IsRequired();
            entity.Property(e => e.EndLocation).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<Checkpoint>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.RouteId);
            entity.HasIndex(e => new { e.RouteId, e.SequenceNumber });
            
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            
            entity.HasOne(e => e.Route)
                .WithMany(r => r.Checkpoints)
                .HasForeignKey(e => e.RouteId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Convoy>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ConvoyCode).IsUnique();
            entity.HasIndex(e => e.RouteId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.ScheduledDepartureTime);
            
            entity.Property(e => e.ConvoyCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.CargoType).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Status).HasMaxLength(100).IsRequired();
            
            entity.HasOne(e => e.Route)
                .WithMany(r => r.Convoys)
                .HasForeignKey(e => e.RouteId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.LeadAsset)
                .WithMany()
                .HasForeignKey(e => e.LeadAssetId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ConvoyCheckpointLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ConvoyId);
            entity.HasIndex(e => e.CheckpointId);
            entity.HasIndex(e => new { e.ConvoyId, e.CheckpointId });
            
            entity.HasOne(e => e.Convoy)
                .WithMany(c => c.CheckpointLogs)
                .HasForeignKey(e => e.ConvoyId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.Checkpoint)
                .WithMany()
                .HasForeignKey(e => e.CheckpointId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Alert>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.AssetId);
            entity.HasIndex(e => e.ConvoyId);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.IsActive);
            entity.HasIndex(e => new { e.IsActive, e.Severity });
            
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Message).HasMaxLength(1000).IsRequired();
            entity.Property(e => e.AcknowledgedBy).HasMaxLength(200);
            
            entity.HasOne(e => e.Asset)
                .WithMany()
                .HasForeignKey(e => e.AssetId)
                .OnDelete(DeleteBehavior.SetNull);
            
            entity.HasOne(e => e.Convoy)
                .WithMany()
                .HasForeignKey(e => e.ConvoyId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ProductionQuota>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.StartDate);
            entity.HasIndex(e => e.EndDate);
            entity.HasIndex(e => new { e.StartDate, e.EndDate });
            
            entity.Property(e => e.PeriodName).HasMaxLength(200).IsRequired();
        });
    }
}
