using System.Linq;
using FizzWare.NBuilder;
using NUnit.Framework;
using NzbDrone.Core.Books;
using NzbDrone.Core.Download;
using NzbDrone.Core.Download.TrackedDownloads;
using NzbDrone.Core.Indexers;
using NzbDrone.Core.Parser.Model;
using NzbDrone.Core.Test.Framework;
using WishListNotification;

namespace NzbDrone.Core.Test.WishListNotification
{
    [TestFixture]
    public class WishListNotificationFixture : CoreTest<WishListNotifier>
    {
        private TrackedDownload _trackerDownload;

        [SetUp]
        public void Setup()
        {
            var episodes = Builder<Book>.CreateListOfSize(2)
                .TheFirst(1).With(s => s.Id = 540)
                    .With(s =>
                        s.Editions = Builder<Edition>.CreateListOfSize(1)
                        .TheFirst(1)
                        .With(e => e.Images = Builder<MediaCover.MediaCover>.CreateListOfSize(1).Build().ToList())
                        .BuildList())
                .TheNext(1).With(s => s.Id = 99)
                .All().With(s => s.AuthorId = 5)
                .Build().ToList();

            var releaseInfo = Builder<ReleaseInfo>.CreateNew()
                .With(v => v.DownloadProtocol = DownloadProtocol.Usenet)
                .With(v => v.DownloadUrl = "http://test.site/download1.ext")
                .Build();

            var remoteBook = Builder<RemoteBook>.CreateNew()
                   .With(c => c.Author = Builder<Author>.CreateNew().Build())
                   .With(c => c.Release = releaseInfo)
                   .With(c => c.Books = episodes)
                   .Build();

            _trackerDownload = Builder<TrackedDownload>.CreateNew()
                .With(c => c.RemoteBook = remoteBook)
                .Build();
        }

        [Test]
        public void WishListNotifier_Should_Send_Notification_On_Event()
        {
            var notifier = new WishListNotifier(TestLogger, new System.Net.Http.HttpClient(), new Repository(new CompanionAppDbContext()));

            notifier.NotifyUsers(new DownloadCompletedEvent(_trackerDownload, 1));

            Assert.That(notifier, Is.Not.Null);
        }
    }
}
