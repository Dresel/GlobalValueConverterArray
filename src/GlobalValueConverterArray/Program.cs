// <copyright file="Program.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace GlobalValueConverterArray
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
	using Microsoft.Extensions.DependencyInjection;

	internal class Program
	{
		private static void Main(string[] args)
		{
			ServiceCollection serviceCollection = new ServiceCollection();
			serviceCollection.AddDbContext<MyContext>(options =>
			{
				options.LogTo(Console.WriteLine);

				options.UseNpgsql("Server=localhost;Port=5432;Database=temp;UserID=postgres;Password=postgres;");

				options.ReplaceService<IValueConverterSelector, TypedIdValueConverterSelector>();
			});

			ServiceProvider buildServiceProvider = serviceCollection.BuildServiceProvider();

			MyContext context = buildServiceProvider.GetRequiredService<MyContext>();
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			List<MyEntityB> list = context.MyEntitiesB.ToList();
			context.MyEntitiesA.Where(x => list.Contains(x.EntityB)).ToList();
		}
	}
}