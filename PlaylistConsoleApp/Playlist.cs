using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistConsoleApp
{
    // Defines a playlist class.
    public class PlayList
    {
        private PlayList(int id, int size)
        {
            Id = id;
            // Assumtion: we have enough memory to create large objects
            TrackIdentifiers = new List<int>();
            for(var index = 1; index <= size; index++)
            {
                TrackIdentifiers.Add(index);
            }
        }

        private int currentPlayedTrackOrdinal = -1;

        public static PlayList Create(int size)
        {
            // Assuming creating an empty playlist is allowed
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException("size", size, "Size of the playlist must be greater than 1.");
            }

            var random = new Random();
            return new PlayList(random.Next(1, int.MaxValue), size);
        }

        // Id of the playlist
        public int Id
        {
            get;
            set;
        }

        // List of TrackIdentifier presents. TrackIdentifier[x] indicates the trackIdentifier present at ordinal x.  
        public List<int> TrackIdentifiers
        {
            get;
            set;
        }

        public int CurrentlyPlayedTrackIdentifier
        {
            get
            {
                return currentPlayedTrackOrdinal == -1 ? 0 : TrackIdentifiers[currentPlayedTrackOrdinal];
            }
        }

        public void Insert(int ordinal, int trackId)
        {
            if(ordinal < 1 || ordinal > TrackIdentifiers.Count + 1)
            {
                var message = string.Format("Ordinal: {0} should be in range 1 to {1} inclusive.", ordinal, TrackIdentifiers.Count + 1);
                throw new ArgumentOutOfRangeException("ordinal", ordinal, message);
            }

            // want to insert at last. eg. original playlist { 1 2 3 4 }; insert 5 6 -> { 1 2 3 4 6 } 
            if(ordinal == TrackIdentifiers.Count + 1)
            {
                TrackIdentifiers.Add(trackId);
            }
            else
            {
                TrackIdentifiers.Insert(ordinal - 1, trackId);
            }

            // Correct the ordinal of currently played track
            if(ordinal <= currentPlayedTrackOrdinal + 1)
            {
                currentPlayedTrackOrdinal++;
            }
        }

        public void Delete(int ordinal)
        {
            if (ordinal < 1 || ordinal > TrackIdentifiers.Count)
            {
                var message = string.Format("Ordinal: {0} should be in range 1 to {1} inclusive.", ordinal, TrackIdentifiers.Count);
                throw new ArgumentOutOfRangeException("ordinal", ordinal, message);
            }

            TrackIdentifiers.RemoveAt(ordinal - 1);

            if(currentPlayedTrackOrdinal + 1 == ordinal)
            {
                currentPlayedTrackOrdinal = -1;
            }
            else if(currentPlayedTrackOrdinal + 1 > ordinal)
            {
                currentPlayedTrackOrdinal--;
            }
        }

        public void Shuffle()
        {
            var randomGenerator = new Random();
            var shuffledTracks = new List<int>();
            var tempTrackIdentifiers = TrackIdentifiers.ToList(); // this will give back a new list, this is needed to return unmodified list if get operation is requested in between.

            int currentPlayedTrack = -1;
            if(currentPlayedTrackOrdinal != -1)
            {
                currentPlayedTrack = tempTrackIdentifiers[currentPlayedTrackOrdinal];
                tempTrackIdentifiers.RemoveAt(currentPlayedTrackOrdinal);
            }

            while (tempTrackIdentifiers.Count > 0)
            {
                var randomIndex = randomGenerator.Next(0, tempTrackIdentifiers.Count);

                if(shuffledTracks.Count == currentPlayedTrackOrdinal)
                {
                    shuffledTracks.Add(currentPlayedTrack);
                    continue;
                }

                shuffledTracks.Add(tempTrackIdentifiers[randomIndex]);
                tempTrackIdentifiers.RemoveAt(randomIndex);
            }

            TrackIdentifiers = shuffledTracks;
        }

        public void Play(int ordinal)
        {
            // Ordinal starts from 1 to n
            if (ordinal < 1 || ordinal > TrackIdentifiers.Count)
            {
                var message = string.Format("Ordinal: {0} should be in range 1 to {1} inclusive.", ordinal, TrackIdentifiers.Count);
                throw new ArgumentOutOfRangeException("ordinal", ordinal, message);
            }

            currentPlayedTrackOrdinal = ordinal - 1;
        }

        public void PrintPlaylist()
        {
            for (var index = 0; index < TrackIdentifiers.Count; index++)
            {
                if(index == currentPlayedTrackOrdinal)
                {
                    Console.Write("{0}* ", TrackIdentifiers[index]);
                }
                else
                {
                    Console.Write("{0} ", TrackIdentifiers[index]);
                }
            }
        }
    }
}
