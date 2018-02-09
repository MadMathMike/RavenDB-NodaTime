﻿using System.Linq;
using Newtonsoft.Json;
using NodaTime;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Xunit;

namespace Raven.Client.NodaTime.Tests
{
    public class NodaIntervalTests : TestBase
    {
        [Fact]
        public void Can_Use_NodaTime_Interval_In_Document()
        {
            var now = SystemClock.Instance.GetCurrentInstant();
            var start = now - Duration.FromMinutes(5);
            var end = now + Duration.FromMinutes(5);
            var interval = new Interval(start, end);

            using (var documentStore = GetDocumentStore())
            {
                using (var session = documentStore.OpenSession())
                {
                    session.Store(new Foo { Id = "foos/1", Interval = interval });
                    session.SaveChanges();
                }

                using (var session = documentStore.OpenSession())
                {
                    var foo = session.Load<Foo>("foos/1");

                    Assert.Equal(interval, foo.Interval);
                }

                /*
                var json = documentStore.DatabaseCommands.Get("foos/1").DataAsJson;
                System.Diagnostics.Debug.WriteLine(json.ToString(Formatting.Indented));
                var expectedStart = interval.Start.ToString(NodaUtil.Instant.FullIsoPattern.PatternText, null);
                var expectedEnd = interval.End.ToString(NodaUtil.Instant.FullIsoPattern.PatternText, null);
                Assert.Equal(expectedStart, json["Interval"].Value<string>("Start"));
                Assert.Equal(expectedEnd, json["Interval"].Value<string>("End"));
                */
            }
        }

        [Fact]
        public void Can_Use_NodaTime_Interval_In_Dynamic_Index()
        {
            var now = SystemClock.Instance.GetCurrentInstant();
            var start = now - Duration.FromMinutes(5);
            var end = now + Duration.FromMinutes(5);
            var interval1 = new Interval(start, end);
            var interval2 = new Interval(end, end + Duration.FromMinutes(5));

            using (var documentStore = GetDocumentStore())
            {
                using (var session = documentStore.OpenSession())
                {
                    session.Store(new Foo { Id = "foos/1", Interval = interval1 });
                    session.Store(new Foo { Id = "foos/2", Interval = interval2 });
                    session.SaveChanges();
                }

                using (var session = documentStore.OpenSession())
                {
                    var q1 = session.Query<Foo>()
                                    .Customize(x => x.WaitForNonStaleResults())
                                    .Where(x => x.Interval.Start == interval1.Start && x.Interval.End == interval1.End);
                    var results1 = q1.ToList();
                    Assert.Single(results1);

                    var q2 = session.Query<Foo>()
                                    .Customize(x => x.WaitForNonStaleResults())
                                    .Where(x => x.Interval.Start <= now && x.Interval.End > now);
                    var results2 = q2.ToList();
                    Assert.Single(results2);

                    var q3 = session.Query<Foo>()
                                    .Customize(x => x.WaitForNonStaleResults())
                                    .OrderByDescending(x => x.Interval.Start);
                    var results3 = q3.ToList();
                    Assert.Equal(2, results3.Count);
                    Assert.True(results3[0].Interval.Start > results3[1].Interval.Start);
                }
            }
        }

        [Fact]
        public void Can_Use_NodaTime_Interval_In_Static_Index()
        {
            var now = SystemClock.Instance.GetCurrentInstant();
            var start = now - Duration.FromMinutes(5);
            var end = now + Duration.FromMinutes(5);
            var interval1 = new Interval(start, end);
            var interval2 = new Interval(end, end + Duration.FromMinutes(5));

            using (var documentStore = GetDocumentStore())
            {
                documentStore.ExecuteIndex(new TestIndex());

                using (var session = documentStore.OpenSession())
                {
                    session.Store(new Foo { Id = "foos/1", Interval = interval1 });
                    session.Store(new Foo { Id = "foos/2", Interval = interval2 });
                    session.SaveChanges();
                }

                using (var session = documentStore.OpenSession())
                {
                    var q1 = session.Query<Foo, TestIndex>()
                                    .Customize(x => x.WaitForNonStaleResults())
                                    .Where(x => x.Interval.Start == interval1.Start && x.Interval.End == interval1.End);
                    var results1 = q1.ToList();
                    Assert.Single(results1);

                    var q2 = session.Query<Foo, TestIndex>()
                                    .Customize(x => x.WaitForNonStaleResults())
                                    .Where(x => x.Interval.Start <= now && x.Interval.End > now);
                    var results2 = q2.ToList();
                    Assert.Single(results2);

                    var q3 = session.Query<Foo>()
                                    .Customize(x => x.WaitForNonStaleResults())
                                    .OrderByDescending(x => x.Interval.Start);
                    var results3 = q3.ToList();
                    Assert.Equal(2, results3.Count);
                    Assert.True(results3[0].Interval.Start > results3[1].Interval.Start);
                }
            }
        }

        public class Foo
        {
            public string Id { get; set; }
            public Interval Interval { get; set; }
        }

        public class TestIndex : AbstractIndexCreationTask<Foo>
        {
            public TestIndex()
            {
                Map = foos => from foo in foos
                              select new
                              {
                                  Interval_Start = foo.Interval.Start,
                                  Interval_End = foo.Interval.End
                              };
            }
        }
    }
}
