﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NowPlayingCore.Core;
using NowPlayingCore.NowPlaying;

namespace NowPlayingCore.Tsumugi
{
    public class TweetConverter
    {
        public static string SongInfoToString(string pattern, SongInfo songinfo)
        {
            var npstr = pattern;
            npstr = npstr.Replace("$TitleUrlEncoded", Uri.EscapeUriString(songinfo.Title));
            npstr = npstr.Replace("$TitleArtistUrlEncoded", Uri.EscapeUriString(songinfo.Title + ' ' + songinfo.Artist));
            npstr = npstr.Replace("$Title", songinfo.Title);
            npstr = npstr.Replace("$AlbumArtist", songinfo.AlbumArtist);
            npstr = npstr.Replace("$Album", songinfo.Album);
            npstr = npstr.Replace("$PCount", songinfo.TrackCount);
            npstr = npstr.Replace("$Artist", songinfo.Artist);
            npstr = npstr.Replace("$Composer", songinfo.Composer);
            npstr = npstr.Replace("$Year", songinfo.Year);
            npstr = npstr.Replace("$Group", songinfo.Group);
            return npstr;
        }

        public static string SongInfoToString(string pattern, SongInfo songInfo, bool enableAutoShorten, AccountContainer acccon)
        {
            var tweet = SongInfoToString(pattern, songInfo);
            if (enableAutoShorten && acccon.CountText(tweet) > acccon.MaxTweetLength)
            {
                while (!(acccon.MaxTweetLength - acccon.CountText(tweet) >= 3))
                {
                    tweet = tweet.Remove(tweet.Length - 1);
                }
                tweet += "...";
            }
            return tweet;
        }
    }
}
