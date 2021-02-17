// <copyright file="TypedIdValueConverter.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace GlobalValueConverterArray
{
	using System;
	using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

	public class TypedIdValueConverter<TTypedIdValue> : ValueConverter<TTypedIdValue, int> where TTypedIdValue : TypedIdValueBase
	{
		public TypedIdValueConverter(ConverterMappingHints? mappingHints = null) : base(id => id.Value,
			value => TypedIdValueConverter<TTypedIdValue>.Create(value), mappingHints)
		{
		}

		private static TTypedIdValue Create(int id) =>
			Activator.CreateInstance(typeof(TTypedIdValue), id) as TTypedIdValue ?? throw new InvalidOperationException();
	}
}