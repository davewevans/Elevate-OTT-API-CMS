using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OttApiPlatform.CMS.Models;

public class UploadProgressModel : INotifyPropertyChanged
{
    private double value = 0;

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public static UploadProgressModel CreateUploadProgress()
    {
        return new UploadProgressModel();
    }

    public double Maximum { get; set; } = 100;

    public double Value
    {
        get => this.value;
        set
        {
            if (value < 0 || value > Maximum)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"Value must be between 0 and {Maximum}.");
            }

            if (!(Math.Abs(value - this.value) > double.Epsilon)) return;
            this.value = value;
            NotifyPropertyChanged();
        }
    }
}
