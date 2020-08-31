using System.Collections.Generic;
using NUnit.Framework;

namespace MainService.Tests
{
    public class DictionaryTests
    {
        [Test]
        public void Merge()
        {
            var a = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>
            {
                {
                    "localhost:7001", new Dictionary<string, Dictionary<string, string>>
                    {
                        {
                            "GET", new Dictionary<string, string>()
                            {
                                {"1", "1"}
                            }
                        },
                        {
                            "POST", new Dictionary<string, string>
                            {
                                {"1", "1"}
                            }
                        }
                    }
                },
            };

            var b = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>()
            {
                {
                    "localhost:7001", new Dictionary<string, Dictionary<string, string>>
                    {
                        {
                            "GET", new Dictionary<string, string>
                            {
                                {"2", "2"}
                            }
                        },
                        {
                            "POST", new Dictionary<string, string>
                            {
                                {"2", "2"}
                            }
                        }
                    }
                }
            };

            var expected = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>()
            {
                {
                    "localhost:7001", new Dictionary<string, Dictionary<string, string>>
                    {
                        {
                            "GET", new Dictionary<string, string>()
                            {
                                {"1", "1"}, {"2", "2"}
                            }
                        },
                        {
                            "POST", new Dictionary<string, string>
                            {
                                {"1", "1"}, {"2", "2"}
                            }
                        }
                    }
                }
            };

            var actual = new List<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>
            {
                a, b
            }.Merge();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}