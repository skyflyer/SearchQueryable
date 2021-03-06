using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SearchQueryable.Tests
{
    public class SearchQueryableTests
    {
        public readonly IQueryable<Book> _books;

        public SearchQueryableTests()
        {
            _books = new List<Book>()
            {
                new Book("Romeo And Juliet", "William Shakespeare", 1597, "ISBN 123456"),
                new Book("Othello", "William Shakespeare", 1597, "ISBN 123457"),
                new Book("The Two Noble Kinsmen", "William Shakespeare", 1635, "ISBN 123458"),
                new Book("A New Kind of Science", "Stephen Wolfram", 2002, "ISBN 123459"),
                new Book("The Will To Live", "Invented Person", 1523, "ISBN 1234510"),
            }.AsQueryable();

        }

        [Fact]
        public void CanSearchByTitle()
        {
            var results = _books.Search("Romeo");
            Assert.Single(results, _books.First());
        }

        [Fact]
        public void CanSearchByAuthor()
        {
            var results = _books.Search("Shakespeare");
            Assert.Equal(3, results.Count());
            Assert.Contains(_books.First(), results);
            Assert.Contains(_books.Skip(2).First(), results);
        }

        // [Fact]
        // public void CanSearchByYear()
        // {
        //     var results = _books.Search("2002");
        //     Assert.Single(results, _books.Skip(3).First());
        // }

        [Fact]
        public void CanSearchWithLowercase()
        {
            var results = _books.Search("romeo");
            Assert.Single(results, _books.First());
        }

        [Fact]
        public void CanSearchWithMixedCase()
        {
            var results = _books.Search("RoMeO");
            Assert.Single(results, _books.First());
        }

        [Fact]
        public void CanSearchWithMixedCaseWithPredicateFields()
        {
            var results = _books.Search("RoMeO", p => p.Title);
            Assert.Single(results, _books.First());
        }

        [Fact]
        public void CanSearchWithMixedCaseWithPredicateProperties()
        {
            var results = _books.Search("INVENted", p => p.Author);
            Assert.Single(results, _books.Last());
        }

        [Fact]
        public void CanSearchWithMixedCaseWithPredicateRWProperties()
        {
            var results = _books.Search("ISBN 1234510", p => p.ISBN);
            Assert.Single(results, _books.Last());
        }

        [Fact]
        public void DoesNotBreakOnNullValues()
        {
            var books = _books.ToList();
            books[0].ISBN = null;
            var results = books.AsQueryable().Search("ISBN 1234510");
            Assert.Single(results, _books.Last());
        }

        [Fact]
        public void DoesNotBreakOnNullValuesWithPredicate()
        {
            var books = _books.ToList();
            books[0].ISBN = null;
            var results = books.AsQueryable().Search("ISBN 1234510", p => p.ISBN);
            Assert.Single(results, _books.Last());
        }

        // TODO: search for int23, DateTIme, float, decimal?

        // TODO: add tests for child objects (p => p.Author.FirstName)
    }
}
