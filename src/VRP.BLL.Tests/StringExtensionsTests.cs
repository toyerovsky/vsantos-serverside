using System;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using VRP.BLL.Extensions;
using VRP.DAL.Enums;

namespace VRP.BLL.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        [TestCase("Taxi", ExpectedResult = GroupType.Taxi)]
        [TestCase("Bar", ExpectedResult = GroupType.Bar)]
        [TestCase("Klub", ExpectedResult = GroupType.Club)]
        [TestCase("Organizacja przestępcza", ExpectedResult = GroupType.Crime)]
        [TestCase("Warsztat", ExpectedResult = GroupType.Workshop)]
        [TestCase("Policja", ExpectedResult = GroupType.Police)]
        [TestCase("Szpital", ExpectedResult = GroupType.Hospital)]
        [TestCase("Wiadomości", ExpectedResult = GroupType.News)]
        public GroupType Should_ReturnProperEnumValue_When_GivenProperDescription(string description)
        {
            return description.FromDescription<GroupType>();
        }
    }
}
