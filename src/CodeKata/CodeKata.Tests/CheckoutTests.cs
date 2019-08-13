using System;
using System.Collections.Generic;
using Xunit;

namespace CodeKata.Tests
{
    public class CheckoutTests
    {
        [Theory]
        [InlineData(new[] { "A" }, 50)]
        [InlineData(new[] { "A", "B" }, 80)]
        [InlineData(new[] { "A", "B", "A" }, 130)]
        [InlineData(new[] { "A", "B", "A", "A" }, 160)]
        [InlineData(new[] { "A", "B", "A", "A", "B" }, 175)]
        [InlineData(new[] { "A", "B", "A", "A", "B", "A" }, 225)]
        [InlineData(new[] { "A", "B", "A", "A", "B", "A", "A" }, 275)]
        [InlineData(new[] { "A", "B", "A", "A", "B", "A", "A", "A" }, 305)]
        [InlineData(new[] { "A", "A", "A", "A", "A", "A", "A", "A" }, 360)]
        public void GetTotal(ICollection<string> items, decimal total)
        {
            var checkout = new Checkout(new[]
            {
                new Rule
                {
                    Item = "A",
                    NumberOfItems = 1,
                    TotalPrice = 50
                },
                new Rule
                {
                    Item = "B",
                    NumberOfItems = 1,
                    TotalPrice = 30
                },
                new Rule
                {
                    Item = "C",
                    NumberOfItems = 1,
                    TotalPrice = 20
                },
                new Rule
                {
                    Item = "D",
                    NumberOfItems = 1,
                    TotalPrice = 15
                },
                new Rule
                {
                    Item = "A",
                    NumberOfItems = 3,
                    TotalPrice = 130
                },
                new Rule
                {
                    Item = "B",
                    NumberOfItems = 2,
                    TotalPrice = 45
                }
            });

            foreach (var item in items)
            {
                checkout.Scan(item);
            }

            Assert.Equal(total, checkout.GetTotal());
        }

        [Fact]
        public void ScanInvalidItem()
        {
            var rules = new[]
            {
                new Rule
                {
                    Item = "A",
                    NumberOfItems = 1,
                    TotalPrice = 10
                }
            };

            var checkout = new Checkout(rules);

            Assert.Throws<InvalidOperationException>(() => checkout.Scan("B"));
        }

        [Fact]
        public void InvalidSetOfRules()
        {
            var rules = new[]
            {
                new Rule
                {
                    Item = "A",
                    NumberOfItems = 2,
                    TotalPrice = 15
                }
            };

            var checkout = new Checkout(rules);

            checkout.Scan("A");

            Assert.Throws<InvalidOperationException>(() => checkout.GetTotal());
        }
    }
}