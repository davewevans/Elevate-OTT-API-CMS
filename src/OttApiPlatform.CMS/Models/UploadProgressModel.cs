using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OttApiPlatform.CMS.Models;

public class UploadProgressModel : INotifyPropertyChanged
{
    private double value = 0;

    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static UploadProgressModel CreateUploadProgress()
    {
        return new UploadProgressModel();
    }

    public double Maximum { get; set; } = 100;

    public double Value
    {
        get
        {
            return this.value;
        }

        set
        {
            if (value != this.value)
            {
                this.value = value;
                NotifyPropertyChanged();
            }
        }
    }
}
