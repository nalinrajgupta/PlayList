using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlaylistConsoleApp;
using FluentAssertions;
using System.Linq;

namespace PlaylistConsoleAppTests
{
    [TestClass]
    public class PlayListTests
    {
        #region Create tests

        [TestMethod]
        public void CreateShouldCreatePlaylistSuccessfully()
        {
            // Arrange
            var size = 10;

            // Act
            var playList = PlayList.Create(size);

            // Assert
            playList.CurrentlyPlayedTrackIdentifier.Should().Be(0, "playList just got created.");
            playList.TrackIdentifiers.Count.Should().Be(size);
            for(var index = 0; index < playList.TrackIdentifiers.Count; index++)
            {
                playList.TrackIdentifiers[index].Should().Be(index + 1);
            }
        }

        [TestMethod]
        public void CreateShouldThrowIfSizeOfPlaylistGivenIsOutOfRange()
        {
            // Arrange
            var size = -1;

            // Act
            Action action = () => PlayList.Create(size);

            // Assert
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void CreateShouldAllowCreatingEmptyPlaylist()
        {
            // Arrange
            var size = 0;

            // Act
            var playList = PlayList.Create(size);

            // Assert
            playList.TrackIdentifiers.Should().NotBeNull();
            playList.TrackIdentifiers.Count.Should().Be(0);
            playList.CurrentlyPlayedTrackIdentifier.Should().Be(0);
        }

        #endregion

        #region Insert tests

        [TestMethod]
        public void InsertAtValidPositionShouldBeAbleToInsertSuccessfully()
        {
            // Arrange
            int size = 10, ordinal = 5, trackId = 11;
            var playList = PlayList.Create(size);
            var trackListBeforeInsert = playList.TrackIdentifiers.ToList();

            // Act
            playList.Insert(ordinal, trackId);

            // Assert
            playList.TrackIdentifiers.Count.Should().Be(size + 1);
            playList.CurrentlyPlayedTrackIdentifier.Should().Be(0);
            playList.TrackIdentifiers[ordinal - 1].Should().Be(trackId);
            for(var index = ordinal; index < size + 1; index++)
            {
                playList.TrackIdentifiers[index] = trackListBeforeInsert[index - 1];
            }
        }

        [TestMethod]
        public void InsertAtLastPositionShouldBeAbleToInsertSuccessfully()
        {
            int size = 10, ordinal = 11, trackId = 11;
            var playList = PlayList.Create(size);
            var trackListBeforeInsert = playList.TrackIdentifiers.ToList();

            // Act
            playList.Insert(ordinal, trackId);

            // Assert
            playList.TrackIdentifiers.Count.Should().Be(size + 1);
            playList.CurrentlyPlayedTrackIdentifier.Should().Be(0);
            playList.TrackIdentifiers[ordinal - 1].Should().Be(trackId);
            for (var index = 0; index < size; index++)
            {
                playList.TrackIdentifiers[index].Should().Be(trackListBeforeInsert[index]);
            }
        }

        [TestMethod]
        public void InsertShouldBeAbleToInsertInEmptyPlayList()
        {
            var playList = PlayList.Create(0);
            var trackListBeforeInsert = playList.TrackIdentifiers.ToList();
            var trackId = 11;

            // Act
            playList.Insert(1, trackId);

            // Assert
            playList.TrackIdentifiers.Count.Should().Be(1);
            playList.CurrentlyPlayedTrackIdentifier.Should().Be(0);
            playList.TrackIdentifiers[0].Should().Be(trackId);
        }

        [TestMethod]
        public void InsertShouldNotChangeTheTrackBeingPlayed()
        {
            // Arrange
            int size = 10;
            var playList = PlayList.Create(size);
            playList.Play(3);
            var playedTrackBeforeInsert = playList.CurrentlyPlayedTrackIdentifier;

            // Act
            playList.Insert(5, 11);

            // Assert
            playList.CurrentlyPlayedTrackIdentifier.Should().Be(playedTrackBeforeInsert);

            // Act
            playList.Insert(2, 12);

            // Assert
            playList.CurrentlyPlayedTrackIdentifier.Should().Be(playedTrackBeforeInsert);
        }

        [TestMethod]
        public void InsertShouldThrowIfPositionIsOutOfRange()
        {
            // Arrange
            int size = 10;
            var playList = PlayList.Create(size);

            // Act
            Action action = () => playList.Insert(size + 2, 12);

            // Assert
            action.ShouldThrow<ArgumentOutOfRangeException>();

            // Act
            action = () => playList.Insert(0, 12);

            // Assert
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        #endregion

        #region Delete test

        [TestMethod]
        public void DeleteShouldRemoveTheTrackFromList()
        {
            // Arrange
            var playList = PlayList.Create(10);

            // Act
            playList.Delete(5);

            // Assert
            playList.TrackIdentifiers.Count.Should().Be(9);
            playList.TrackIdentifiers.SingleOrDefault(x => x == 5).Should().Be(0);
        }

        [TestMethod]
        public void DeleteShouldThrowIfOrdinalGivenIsNotInRange()
        {
            // Arrange
            var playList = PlayList.Create(10);

            // Act
            Action action = () => playList.Delete(12);

            // Assert
            action.ShouldThrow<ArgumentOutOfRangeException>();

            // Act
            action = () => playList.Delete(0);

            // Assert
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void DeleteShouldThrowIfTrackIdentifierListIsEmpty()
        {
            // Arrange
            var playList = PlayList.Create(0);

            // Act
            Action action = () => playList.Delete(1);

            // Assert
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void DeleteShouldResetCurrentPlayedTrackIfCurrentPlayedTrackIsDeleted()
        {
            // Arrange
            var playList = PlayList.Create(10);
            playList.Play(5);

            // Act
            playList.Delete(5);

            // Assert
            playList.CurrentlyPlayedTrackIdentifier.Should().Be(0);
        }

        [TestMethod]
        public void DeleteShouldRetainCurrentPlayedTrackIfSomeOtherTrackIsDeleted()
        {
            // Arrange
            var playList = PlayList.Create(10);
            playList.Play(5);

            // Act
            playList.Delete(6);

            // Assert
            playList.CurrentlyPlayedTrackIdentifier.Should().Be(5);

            // Act
            playList.Delete(2);

            // Assert
            playList.CurrentlyPlayedTrackIdentifier.Should().Be(5);
        }

        #endregion

        #region Play Track tests

        [TestMethod]
        public void PlayShouldThrowIfOrdinalIsOutOfRange()
        {
            // Arrange
            var playList = PlayList.Create(10);

            // Act
            playList.Play(5);

            // Assert
            playList.CurrentlyPlayedTrackIdentifier.Should().Be(5);

            // Act
            playList.Play(1);

            // Assert
            playList.CurrentlyPlayedTrackIdentifier.Should().Be(1);

            // Act
            playList.Play(10);

            // Assert
            playList.CurrentlyPlayedTrackIdentifier.Should().Be(10);
        }

        [TestMethod]
        public void PlayShouldThrowIfTrackIdentifierListIsEmpty()
        {
            // Arrange 
            var playList = PlayList.Create(0);

            // Act
            Action action = () => playList.Play(0);

            // Assert
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void PlayShouldThrowIfOrdinalGivenDoesNotExist()
        {
            // Arrange 
            var playList = PlayList.Create(10);

            // Act
            Action action = () => playList.Play(11);

            // Assert
            action.ShouldThrow<ArgumentOutOfRangeException>();

            // Act
            action = () => playList.Play(0);

            // Assert
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        #endregion

        #region Shuffle Tests

        [TestMethod]
        public void ShuffleShouldReturnSuccessfullyEvenIfListIsEmpty()
        {
            // Arrange
            var playList = PlayList.Create(0);

            // Act
            Action action = () => playList.Shuffle();

            // Assert
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void ShuffleShouldReturnSuccessfullyWithFiniteTrackIdentifierList()
        {
            // Arrange
            var playList = PlayList.Create(10);
            var previousTrackIdentifiers = playList.TrackIdentifiers.ToList();

            // Act
            playList.Shuffle();

            // Assert
            playList.TrackIdentifiers.Count.Should().Be(10);
            for(var index = 0; index < 10; index++)
            {
                playList.TrackIdentifiers.SingleOrDefault(x => x == previousTrackIdentifiers[index]).Should().NotBe(0);
            }
        }

        [TestMethod]
        public void ShuffleShouldNotChangeTheCurrentPlayedTrackAndOrdinal()
        {
            // Arrange
            var playList = PlayList.Create(10);
            playList.Play(3);
            var previousTrackIdentifiers = playList.TrackIdentifiers.ToList();

            // Act
            playList.Shuffle();

            // Assert
            playList.TrackIdentifiers.Count.Should().Be(10);
            for (var index = 0; index < 10; index++)
            {
                playList.TrackIdentifiers.SingleOrDefault(x => x == previousTrackIdentifiers[index]).Should().NotBe(0);
            }
        }

        #endregion
    }
}
