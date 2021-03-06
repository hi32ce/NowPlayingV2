﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Hardcodet.Wpf.TaskbarNotification;
using MahApps.Metro.Controls;
using NowPlayingCore.NowPlaying;
using NowPlayingV2.Core;
using NowPlayingV2.UI.View;

namespace NowPlayingV2.UI.NotifyIcon
{
    public class NotifyIconManager
    {
        public static NotifyIconManager NotifyIconSingleton = new NotifyIconManager();

        public TaskbarIcon NPIcon = default!;

        public void InitIcon()
        {
            NPIcon = (TaskbarIcon) Application.Current.FindResource("NPIcon");
            ((MenuItem) LogicalTreeHelper.FindLogicalNode(NPIcon.ContextMenu, "OnOpenSetting")).Click +=
                (sender, e) => { UI.MainWindow.OpenSingletonWindow(); };
            ((MenuItem) LogicalTreeHelper.FindLogicalNode(NPIcon.ContextMenu, "OnAppExit")).Click +=
                (sender, e) => { Application.Current.Shutdown(); };
            ((MenuItem) LogicalTreeHelper.FindLogicalNode(NPIcon.ContextMenu, "OnTweetDialog")).Click +=
                (sender, e) => { (new TweetDialog()).Show(); };
            ((MenuItem) LogicalTreeHelper.FindLogicalNode(NPIcon.ContextMenu, "IsAutoTweetEnabledMenuItem"))
                .DataContext = Core.ConfigStore.StaticConfig;
            ((MenuItem) LogicalTreeHelper.FindLogicalNode(NPIcon.ContextMenu, "OnTweetNow")).Click +=
                async (sender, e) =>
                {
                    try
                    {
                        await ManualTweet.RunManualTweet(ConfigStore.StaticConfig);
                        var song = PipeListener.StaticPipeListener!.LastPlayedSong!;
                        NPIcon.ShowBalloonTip("投稿完了", $"{song.Title}\n{song.Artist}\n{song.Album}", BalloonIcon.Info);
                    }
                    catch (Exception exception)
                    {
                        NPIcon.ShowBalloonTip("エラー", exception.Message, BalloonIcon.Info);
                    }
                };
            ((UserControl)LogicalTreeHelper.FindLogicalNode(NPIcon.ContextMenu, "SmallStatusView"))
                .DataContext = null;
            PipeListener.StaticPipeListener!.OnMusicPlay += StaticPipeListenerOnMusicPlay;
        }

        private void StaticPipeListenerOnMusicPlay(SongInfo songInfo)
        {
            // 表示用データを生成する
            var songView = new Model4SongView(songInfo);
            NPIcon.Invoke(() =>
            {
                ((UserControl) LogicalTreeHelper.FindLogicalNode(NPIcon.ContextMenu, "SmallStatusView"))
                    .DataContext = songView;
            });
        }

        public void DeleteIcon() => NPIcon.Dispose();
    }
}