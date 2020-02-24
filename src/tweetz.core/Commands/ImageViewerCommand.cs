﻿using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using tweetz.core.Infrastructure;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    public class ImageViewerCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();
        private Popup? popup;
        private IImageViewerService ImageViewerService { get; }
        private IMessageBoxService MessageBoxService { get; }

        public ImageViewerCommand(IImageViewerService imageViewerService, IMessageBoxService messageBoxService)
        {
            ImageViewerService = imageViewerService;
            MessageBoxService = messageBoxService;
        }

        public CommandBinding CommandBinding()
        {
            App.Current.MainWindow.Closed += MainWindow_Closed;
            return new CommandBinding(Command, CommandHandler);
        }

        private void CommandHandler(object sender, ExecutedRoutedEventArgs ea)
        {
            ea.Handled = true;
            if (popup != null) popup.IsOpen = false;

            try
            {
                var uri = ea.Parameter switch
                {
                    Media media => ImageViewerService.MediaSource(media),
                    string path => new Uri(path),
                    _ => null
                };

                if (uri == null || !(sender is Window window)) return;
                popup = ImageViewerService.CreatePopup(window, uri);
            }
            catch (Exception ex)
            {
                MessageBoxService.ShowMessageBox(ex.Message);
            }
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            if (popup != null) popup.IsOpen = false;
        }
    }
}