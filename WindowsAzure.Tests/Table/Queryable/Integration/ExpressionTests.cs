﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WindowsAzure.Table;
using WindowsAzure.Tests.Attributes;
using WindowsAzure.Tests.Extensions;
using WindowsAzure.Tests.Samples;
using Xunit;

namespace WindowsAzure.Tests.Table.Queryable.Integration
{
    public sealed class ExpressionTests : CountryTableSetBase
    {
        private const string Germany = "Germany";
        private const string Spain = "Spain";
        private const string Finland = "Finland";
        private const string France = "France";

        public ExpressionTests()
        {
            TableSet<Country> tableSet = GetTableSet();
            tableSet.Add(
                new Country
                    {
                        Area = 357021,
                        Continent = "Europe",
                        TopSecretKey = new byte[] {0xaa, 0xbb, 0xcc},
                        Formed = new DateTime(1871, 1, 18),
                        Id = Guid.NewGuid(),
                        IsExists = true,
                        Name = Germany,
                        Population = 81799600,
                        PresidentsCount = 11
                    });
            tableSet.Add(
                new Country
                    {
                        Area = 505992,
                        Continent = "Europe",
                        TopSecretKey = new byte[] {0xaa, 0xbb, 0xcc},
                        Formed = new DateTime(1812, 1, 1),
                        Id = Guid.NewGuid(),
                        IsExists = false,
                        Name = Spain,
                        Population = 47190493,
                        PresidentsCount = 8
                    });
            tableSet.Add(
                new Country
                    {
                        Area = 674843,
                        Continent = "Europe",
                        TopSecretKey = new byte[] {0xaa, 0xbb, 0xcc},
                        Formed = new DateTime(1792, 1, 1),
                        Id = Guid.NewGuid(),
                        IsExists = true,
                        Name = France,
                        Population = 65350000,
                        PresidentsCount = 24
                    });
            tableSet.Add(
                new Country
                    {
                        Area = 338424,
                        Continent = "Europe",
                        TopSecretKey = new byte[] {0xaa, 0xbb, 0xcc},
                        Formed = new DateTime(1809, 3, 29),
                        Id = Guid.NewGuid(),
                        IsExists = true,
                        Name = Finland,
                        Population = 5421827,
                        PresidentsCount = 12
                    });
        }

        [IntegrationFact]
        public void QueryWithContains()
        {
            // Arrange
            var names = new List<string>
                {
                    Germany,
                    Finland
                };
           TableSet<Country> tableSet = GetTableSet();

           Expression<Func<Country, bool>> hasName = x => names.Contains(x.Name);
            // Act
           List<Country> result = tableSet.Where(hasName).ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            List<string> resultNames = result.Select(p => p.Name).ToList();
            Assert.Contains(Germany, resultNames);
            Assert.Contains(Finland, resultNames);
        }

        [IntegrationFact]
        public void QueryWithContainsAndExists()
        {
            // Arrange
            var names = new List<string>
                {
                    Germany,
                    Finland
                };
            TableSet<Country> tableSet = GetTableSet();

            Expression<Func<Country, bool>> hasName = x => names.Contains(x.Name);
            Expression<Func<Country, bool>> exists = x => x.IsExists;

            var filter = hasName.And(exists);

            // Act
            List<Country> result = tableSet.Where(filter).ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            List<string> resultNames = result.Select(p => p.Name).ToList();
            Assert.Contains(Germany, resultNames);
            Assert.Contains(Finland, resultNames);
        }

        [IntegrationFact]
        public void QueryWithContainsAndComplexFunction()
        {
            // Arrange
            var names = new List<string>
                {
                    Germany,
                    Finland
                };
            TableSet<Country> tableSet = GetTableSet();

            Expression<Func<Country, bool>> hasName = x => names.Contains(x.Name);
            Expression<Func<Country, bool>> exists = x => x.IsExists && x.Population > 1000000;

            var filter = hasName.And(exists);

            // Act
            List<Country> result = tableSet.Where(filter).ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            List<string> resultNames = result.Select(p => p.Name).ToList();
            Assert.Contains(Germany, resultNames);
            Assert.Contains(Finland, resultNames);
        }
    }
}