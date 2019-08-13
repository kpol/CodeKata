using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeKata
{
    public class Checkout
    {
        private readonly Dictionary<string, List<Rule>> _rules;

        private readonly List<string> _items = new List<string>();

        public Checkout(IEnumerable<Rule> rules)
        {
            if (rules == null) throw new ArgumentNullException(nameof(rules));

            _rules = rules.GroupBy(r => r.Item)
                .ToDictionary(k => k.Key, v => v.OrderByDescending(r => r.NumberOfItems).ToList());
        }

        public void Scan(string item)
        {
            if (!_rules.TryGetValue(item, out _))
            {
                throw new InvalidOperationException($"The item '{item}' is not registered within rules.");
            }

            _items.Add(item);
        }

        public decimal GetTotal()
        {
            var groupedItems = _items.GroupBy(i => i);

            decimal total = 0;

            foreach (var groupedItem in groupedItems)
            {
                var rules = _rules[groupedItem.Key];
                var itemsCount = groupedItem.Count();

                foreach (var rule in rules)
                {
                    if (rule.NumberOfItems > itemsCount)
                    {
                        continue;
                    }

                    var numberOfTimesRuleApplied = itemsCount / rule.NumberOfItems;
                    total += rule.TotalPrice * numberOfTimesRuleApplied;
                    itemsCount -= rule.NumberOfItems * numberOfTimesRuleApplied;
                }

                if (itemsCount != 0)
                {
                    throw new InvalidOperationException("Unable to match a rule.");
                }
            }

            return total;
        }
    }
}