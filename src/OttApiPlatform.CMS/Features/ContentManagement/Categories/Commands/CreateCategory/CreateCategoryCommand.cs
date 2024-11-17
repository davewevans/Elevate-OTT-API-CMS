﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OttApiPlatform.CMS.Features.ContentManagement.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private string _title = string.Empty;

    public string Title
    {
        get
        {
            return _title;
        }

        set
        {
            if (value != _title)
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }
    }
    
    public string? Description { get; set; }

    public int Position { get; set; } = 1;

    public string? ImageUrl { get; set; }

    public string? SeoTitle { get; set; }

    public string? SeoDescription { get; set; }

    public string? Slug { get; set; }

    public bool IsImageAdded { get; set; }

    // public IFormFile? ImageFile { get; set; }


    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
