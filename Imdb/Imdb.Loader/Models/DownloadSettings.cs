﻿namespace Imdb.Loader.Models;

public class DownloadSettings
{
    public Uri SourceUrl { get; set; }

    public string DownloadLocation { get; set; }

    public ImdbFiles FilesToDownload { get; set; }
}
