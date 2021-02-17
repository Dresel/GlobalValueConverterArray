// <copyright file="MyContext.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace GlobalValueConverterArray
{
	using Microsoft.EntityFrameworkCore;

	public class MyContext : DbContext
	{
		public MyContext(DbContextOptions<MyContext> options) : base(options)
		{
		}

		public DbSet<MyEntityA> MyEntitiesA { get; set; }

		public DbSet<MyEntityB> MyEntitiesB { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			//modelBuilder.Entity<MyEntityA>().HasKey(x => x.Id);
			//modelBuilder.Entity<MyEntityA>().Property(x => x.Id).HasConversion(p => p.Value, p => new MyEntityAId(p));

			//modelBuilder.Entity<MyEntityB>().HasKey(x => x.Id);
			//modelBuilder.Entity<MyEntityB>().Property(x => x.Id).HasConversion(p => p.Value, p => new MyEntityBId(p));
		}
	}
}