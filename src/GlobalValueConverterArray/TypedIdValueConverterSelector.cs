// <copyright file="TypedIdValueConverterSelector.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace GlobalValueConverterArray
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
	using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.ValueConversion;

	public class TypedIdValueConverterSelector : NpgsqlValueConverterSelector
	{
		private readonly ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo> converters =
			new ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo>();

		public TypedIdValueConverterSelector(ValueConverterSelectorDependencies dependencies) : base(dependencies)
		{
		}

		public override IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type? providerClrType = null)
		{
			IEnumerable<ValueConverterInfo> baseConverters = base.Select(modelClrType, providerClrType);

			foreach (ValueConverterInfo converter in baseConverters)
			{
				yield return converter;
			}

			Type underlyingModelType = TypedIdValueConverterSelector.UnwrapNullableType(modelClrType) ??
				throw new InvalidOperationException();
			Type? underlyingProviderType = TypedIdValueConverterSelector.UnwrapNullableType(providerClrType);

			if (underlyingProviderType is null || underlyingProviderType == typeof(int))
			{
				bool isTypedIdValue = typeof(TypedIdValueBase).IsAssignableFrom(underlyingModelType);

				if (isTypedIdValue)
				{
					Type converterType = typeof(TypedIdValueConverter<>).MakeGenericType(underlyingModelType);

					yield return this.converters.GetOrAdd((underlyingModelType, typeof(int)), _ =>
					{
						return new ValueConverterInfo(modelClrType, typeof(int),
							valueConverterInfo =>
								Activator.CreateInstance(converterType, valueConverterInfo.MappingHints) as ValueConverter);
					});
				}
			}
		}

		private static Type? UnwrapNullableType(Type? type)
		{
			if (type is null)
			{
				return null;
			}

			return Nullable.GetUnderlyingType(type) ?? type;
		}
	}
}